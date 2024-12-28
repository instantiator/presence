using Presence.SocialFormat.Lib.DTO;
using Presence.SocialFormat.Lib.Networks;
using Presence.SocialFormat.Lib.Posts;

namespace Presence.SocialFormat.Lib.Composition;

public abstract class AbstractThreadComposer : IThreadComposer
{
    public PostRenderRules PostRules { get; protected set; }
    public ThreadCompositionRules ThreadRules { get; protected set; }
    public SocialNetwork Network { get; protected set; }

    protected AbstractThreadComposer(SocialNetwork network, ThreadCompositionRules threadRules, PostRenderRules postRules)
    {
        ThreadRules = threadRules;
        PostRules = postRules;
        Network = network;
    }

    public abstract SocialSnippet CreatePostCounter(int index);
    public IEnumerable<CommonPost> Compose(CompositionRequest request)
    {
        var posts = new List<CommonPost>();
        foreach (var nextSnippet in request.Message)
        {
            posts.AddRange(AddSnippet(posts.Count(), posts.LastOrDefault(), nextSnippet, request.Tags));
        }

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
        return posts;
    }
    private IEnumerable<CommonPost> AddSnippet(int nextIndex, CommonPost? currentPost, SocialSnippet nextSnippet, IEnumerable<SocialSnippet> tags)
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
                var snipped = nextSnippet.Truncate(currentPost.MessageSpace, PostRules);
                var additionalPosts = AddSnippet(newPosts.Count + nextIndex, currentPost, snipped, tags);
                newPosts.AddRange(additionalPosts);
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

    public CommonPost CreateNewPost(int index, IEnumerable<SocialSnippet> tags)
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
    public bool Fits(CommonPost currentPost, SocialSnippet nextSnippet)
    {
        return currentPost.MessageSpace >= nextSnippet.Compose(PostRules).Length;
    }
}

/*
Hello, world! This is a multi-snippet message. With 6 snippets. This is snippet 4. /1 #TagOne #TagTwo
*/