using Presence.SocialFormat.Lib.Helpers;

namespace Presence.SocialFormat.Lib.Tests;

[TestClass]
[TestCategory("Unit")]
public class ExtensionTests
{
    [TestMethod]
    public void IEnumerableExtension_CanIntersperse_WithPrimitives()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var interspersed = items.Intersperse(0);
        Assert.AreEqual(new List<int> { 1, 0, 2, 0, 3, 0, 4, 0, 5 }, interspersed, new IEnumerableEqualityComparer<int>());
    }

    public class TestObject
    {
        public required int Id { get; init; }
        public required string Name { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj == null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            var other = obj as TestObject;
            if (other == null) { return false; }
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }
    }

    [TestMethod]
    public void IEnumerableExtension_CanIntersperse_WithObjects()
    {
        var items = new List<TestObject> { new TestObject { Id = 1, Name = "One" }, new TestObject { Id = 2, Name = "Two" }, new TestObject { Id = 3, Name = "Three" } };
        var interspersed = items.Intersperse(new TestObject { Id = 0, Name = "Zero" });
        Assert.AreEqual(new List<TestObject> { new TestObject { Id = 1, Name = "One" }, new TestObject { Id = 0, Name = "Zero" }, new TestObject { Id = 2, Name = "Two" }, new TestObject { Id = 0, Name = "Zero" }, new TestObject { Id = 3, Name = "Three" } }, interspersed, new IEnumerableEqualityComparer<TestObject>());
    }

}