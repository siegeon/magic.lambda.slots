/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.slots.tests
{
    public class SlotTests
    {
        [Fact]
        public void CreateSlot()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:int:57
slots.signal:foo");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void CreateAndRetrieveSlot()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   .foo:bar
slots.get:foo");
            Assert.Single(lambda.Children.Skip(1).First().Children);
            Assert.Equal(".foo", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("bar", lambda.Children.Skip(1).First().Children.First().Value);
        }

        [Fact]
        public void OverwriteSlot()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:int:57
slots.create:foo
   slots.return-value:int:42
slots.signal:foo");
            Assert.Equal(42, lambda.Children.Skip(2).First().Value);
        }

        [Fact]
        public void ArgumentPassing()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:x:@.arguments/*
slots.signal:foo
   foo:int:57");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ReturnNodes()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-nodes
      foo1:bar1
      foo2:bar2
slots.signal:foo");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
            Assert.Equal("foo1", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("foo2", lambda.Children.Skip(1).First().Children.Skip(1).First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void ReturnNodesFromExpression()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   .result
      foo1:bar1
      foo2:bar2
   slots.return-nodes:x:-/*
slots.signal:foo");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
            Assert.Equal("foo1", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("foo2", lambda.Children.Skip(1).First().Children.Skip(1).First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void ReturnNodesFromExpressionInvokedMultipleTimes()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   .result
      foo1:bar1
      foo2:bar2
   slots.return-nodes:x:-/*
slots.signal:foo
slots.signal:foo");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
            Assert.Equal("foo1", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("foo2", lambda.Children.Skip(1).First().Children.Skip(1).First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Value);
            Assert.Equal(2, lambda.Children.Skip(2).First().Children.Count());
            Assert.Equal("foo1", lambda.Children.Skip(2).First().Children.First().Name);
            Assert.Equal("foo2", lambda.Children.Skip(2).First().Children.Skip(1).First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(2).First().Children.First().Value);
            Assert.Equal("bar2", lambda.Children.Skip(2).First().Children.Skip(1).First().Value);
        }
    }
}
