using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Helpers;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Post;

namespace Presence.SocialFormat.Lib.Thread.Composition;

public abstract class AbstractThreadComposer : IThreadComposer
{
    private static Dictionary<SocialNetwork, int> _counters = new Dictionary<SocialNetwork, int>();

    public ThreadComposerIdentity Identity { get; protected set; }
    public PostRenderRules PostRules { get; protected set; }
    public ThreadCompositionRules ThreadRules { get; protected set; }

    protected AbstractThreadComposer(SocialNetwork network, ThreadCompositionRules threadRules, PostRenderRules postRules)
    {
        ThreadRules = threadRules;
        PostRules = postRules;
        Identity = new ThreadComposerIdentity() { Network = network, Index = GetCounter(network) };
    }

    public int GetCounter(SocialNetwork network)
    {
        if (!_counters.ContainsKey(network))
        {
            _counters[network] = 0;
        }
        return _counters[network]++;
    }

    public abstract SocialSnippet CreatePostCounter(int index);
    public ComposedThread Compose(ThreadCompositionRequest request)
    {
        var success = false;
        var posts = new List<CommonPost>();
        Exception? exception = null;
        try
        {
            foreach (var nextSnippet in request.Message)
            {
                posts.AddRange(AddSnippet(posts.Count(), posts.LastOrDefault(), nextSnippet, request.Tags));
            }
            UpliftPostImages(posts);
            ArrangePostImages(posts);

            // if only 1 post, remove the counter from all posts
            if (posts.Count < 2 && ThreadRules.OnlyCountThreads)
            {
                posts = posts.Select(post =>
                {
                    post.Prefix = post.Prefix.Where(s => s.SnippetType != SnippetType.Counter);
                    post.Suffix = post.Suffix.Where(s => s.SnippetType != SnippetType.Counter);
                    return post;
                }).ToList();
            }
            success = true;
        }
        catch (Exception e)
        {
            exception = e;
            success = false;
        }
        // if the last post is empty - remove it
        if (posts.Last().Message.Count() == 0 && posts.Last().Images.Count() == 0)
        {
            posts.Remove(posts.Last());
        }
        return new ComposedThread(Identity, posts, success, exception);
    }

    protected void UpliftPostImages(IEnumerable<CommonPost> posts)
    {
        foreach (var (post, index) in posts.Select((p, i) => (p, i)))
        {
            var snippet_images = post.Message.Where(snippet => snippet.SnippetType == SnippetType.Image);
            var snippets_with_images = post.Message.Where(snippet => snippet.Images.Any());

            // append images to post
            post.Images.AddRange(snippet_images.Select(snippet => CommonPostImage.From(snippet)));
            post.Images.AddRange(snippets_with_images.SelectMany(snippet => snippet.Images.Select(image => CommonPostImage.From(image))));

            // remove snippet images
            post.Message = post.Message.Where(s => s.SnippetType != SnippetType.Image);
            snippets_with_images.ToList().ForEach(s => s.Images.Clear());
        }
    }

    protected void ArrangePostImages(List<CommonPost> posts)
    {
        int index = 0;
        while (index < posts.Count())
        {
            var post = posts[index];
            if (post.Images.Count() > ThreadRules.MaxImagesPerPost)
            {
                switch (ThreadRules.ImageOverflowRule)
                {
                    case ImageOverflowRule.DropImages:
                        post.Images = post.Images.Take(ThreadRules.MaxImagesPerPost).ToList();
                        break;
                    case ImageOverflowRule.OverflowIntoNextPost:
                        var overflowIntoNext = post.Images.Skip(ThreadRules.MaxImagesPerPost).ToList();
                        post.Images = post.Images.Take(ThreadRules.MaxImagesPerPost).ToList();
                        if (index + 1 < posts.Count)
                            posts[index + 1].Images.AddRange(overflowIntoNext);
                        else
                            posts.Insert(index+1, CommonPost.ImageOverflowPost(index + 1, PostRules, overflowIntoNext, ThreadRules.ImageOverflowText, post.Prefix, post.Suffix));
                        break;
                    case ImageOverflowRule.OverflowInsertNewPost:
                        var overflowInsert = post.Images.Skip(ThreadRules.MaxImagesPerPost).ToList();
                        post.Images = post.Images.Take(ThreadRules.MaxImagesPerPost).ToList();
                        posts.Insert(index + 1, CommonPost.ImageOverflowPost(index + 1, PostRules, overflowInsert, ThreadRules.ImageOverflowText, post.Prefix, post.Suffix));
                        break;
                }
            }
            index++;
        }

        // renumber
        posts.Select((p, i) => (p, i)).ToList().ForEach((pi) => pi.p.Index = pi.i);
    }

    protected IEnumerable<CommonPost> AddSnippet(int nextIndex, CommonPost? currentPost, SocialSnippet nextSnippet, IEnumerable<SocialSnippet> tags)
    {
        var newPosts = new List<CommonPost>();
        if (currentPost == null)
        {
            currentPost = CreateNewPost(newPosts.Count + nextIndex, tags);
            newPosts.Add(currentPost);
        }

        if (nextSnippet.SnippetType == SnippetType.Break)
        {
            currentPost.MarkedComplete = true;
            currentPost = CreateNewPost(newPosts.Count + nextIndex, tags);
            newPosts.Add(currentPost);
            return newPosts;
        }

        // easy case - if the snippet fits in the current post, add it
        if (currentPost.Fits(nextSnippet))
        {
            currentPost.Message = currentPost.Message.Append(nextSnippet);
        }
        // the snippet didn't fit, so follow some strategies to snip it / add more posts
        else
        {
            if (nextSnippet.MayDivide)
            {
                // cut the current snippet into 2 parts
                var parts = nextSnippet.Divide(currentPost.MessageSpace, PostRules);

                // add part 1 to the current post
                if (parts.Item1 != null)
                {
                    currentPost.Message = currentPost.Message.Append(parts.Item1);
                }
                else
                {
                    currentPost.MarkedComplete = true;
                }

                // if there's a second piece, add that separately
                if (parts.Item2 != null)
                {
                    // add a new post for part 2
                    currentPost = CreateNewPost(newPosts.Count + nextIndex, tags);
                    newPosts.Add(currentPost);

                    var additionalPosts = AddSnippet(newPosts.Count + nextIndex, currentPost, parts.Item2!, tags);
                    newPosts.AddRange(additionalPosts);
                }
            }
            else if (nextSnippet.MayTruncate)
            {
                // truncate the snippet and add to the current post
                var truncated = nextSnippet.Truncate(currentPost.MessageSpace, PostRules);
                if (truncated != null)
                {
                    var additionalPosts = AddSnippet(newPosts.Count + nextIndex, currentPost, truncated, tags);
                    newPosts.AddRange(additionalPosts);
                }
                else
                {
                    // snippet truncation didn't work - push the current post and try with a fresh one
                    currentPost = CreateNewPost(newPosts.Count + nextIndex, tags);
                    newPosts.Add(currentPost);

                    // if the new post can't fit the snippet, throw an exception - this is a fresh blank post and the snippet won't resize
                    if (!currentPost.Fits(nextSnippet)) { throw new Exception("Snippet too large for blank post, cannot resize"); }
                    var additionalPosts = AddSnippet(newPosts.Count + nextIndex, currentPost, nextSnippet, tags);
                    newPosts.AddRange(additionalPosts);
                }
            }
            else
            {
                // snippet won't divide or truncate - push the current post and try with a fresh one
                currentPost = CreateNewPost(newPosts.Count + nextIndex, tags);
                newPosts.Add(currentPost);

                // if the new post can't fit the snippet, throw an exception - this is a fresh blank post and the snippet won't resize
                if (!currentPost.Fits(nextSnippet)) { throw new Exception("Snippet too large for blank post, cannot resize"); }
                var additionalPosts = AddSnippet(newPosts.Count + nextIndex, currentPost, nextSnippet, tags);
                newPosts.AddRange(additionalPosts);
            }
        }
        return newPosts;
    }

    protected CommonPost CreateNewPost(int index, IEnumerable<SocialSnippet> tags)
    {
        var prefixSnippets = new IEnumerable<SocialSnippet>[] {
            ThreadRules.PostCounterPrefix ? new[] { CreatePostCounter(index) } : []
        }.SelectMany(ss => ss);

        var suffixSnippets = new IEnumerable<SocialSnippet>[]
        {
            ThreadRules.PostCounterSuffix ? new[] { CreatePostCounter(index) } : [],
            ThreadRules.TagsOnAllPosts || (ThreadRules.TagsOnFirstPost && index == 0) ? tags : [],
        }.SelectMany(ss => ss);

        var post = new CommonPost(index, PostRules) { Prefix = prefixSnippets, Suffix = suffixSnippets };

        if (post.MessageSpace <= PostRules.WordSpace.Length + PostRules.MinAcceptableSpace)
        {
            throw new Exception($"Insufficient space in new post: {post.MessageSpace} (min = {PostRules.MinAcceptableSpace})");
        }

        return post;
    }
    protected bool Fits(CommonPost currentPost, SocialSnippet nextSnippet)
    {
        return currentPost.MessageSpace >= nextSnippet.Compose(PostRules).Length;
    }
}
