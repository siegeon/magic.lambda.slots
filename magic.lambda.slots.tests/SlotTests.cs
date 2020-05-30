/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;

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
signal:foo");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void CreateSlotCheckIfExists()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:int:57
slots.exists:foo");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void CreateSlotVocabulary()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:int:57
slots.vocabulary");
            Assert.NotEmpty(lambda.Children.Skip(1).First().Children);
            Assert.NotEmpty(lambda.Children.Skip(1).First().Children.Where(x => x.GetEx<string>() == "foo"));
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
signal:foo");
            Assert.Equal(42, lambda.Children.Skip(2).First().Value);
        }

        [Fact]
        public void ArgumentPassing()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   slots.return-value:x:@.arguments/*
signal:foo
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
signal:foo");
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
signal:foo");
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
signal:foo
signal:foo");
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
