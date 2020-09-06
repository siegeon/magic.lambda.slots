/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using System.Collections.Generic;
using Xunit;
using magic.node.extensions;
using System.Threading.Tasks;

namespace magic.lambda.slots.tests
{
    public class SlotTests
    {
        [Fact]
        public void CreateSlot()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-value:int:57
signal:foo");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public async Task CreateSlotAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
slots.create:foo
   return-value:int:57
wait.signal:foo");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void CreateSlotDelete_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => Common.Evaluate(@"
slots.create:foo
   return-value:int:57
slots.delete:foo
signal:foo"));
        }

        [Fact]
        public void CreateSlotCheckIfExists()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-value:int:57
slots.exists:foo");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void CreateSlotVocabulary()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-value:int:57
slots.vocabulary");
            Assert.NotEmpty(lambda.Children.Skip(1).First().Children);
            Assert.NotEmpty(lambda.Children.Skip(1).First().Children.Where(x => x.GetEx<string>() == "foo"));
        }

        [Fact]
        public void CreateSlotVocabularyWithFilter()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-value:int:57
slots.vocabulary:not-foo");
            Assert.Empty(lambda.Children.Skip(1).First().Children);
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
   return-value:int:57
slots.create:foo
   return-value:int:42
signal:foo");
            Assert.Equal(42, lambda.Children.Skip(2).First().Value);
        }

        [Fact]
        public void ArgumentPassing()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-value:x:@.arguments/*
signal:foo
   foo:int:57");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ReturnValue()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return:foo
signal:foo");
            Assert.Equal("foo", lambda.Children.Skip(1).First().GetEx<string>());
        }

        [Fact]
        public void ReturnValueExpression()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   .foo:foo
   return:x:-
signal:foo");
            Assert.Equal("foo", lambda.Children.Skip(1).First().GetEx<string>());
        }

        [Fact]
        public void ReturnNodes()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return-nodes
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
        public void ReturnNodesImplicitly()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return
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
        public void ReturnNodesAndValueImplicitly()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   return:foo
      foo1:bar1
      foo2:bar2
signal:foo");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
            Assert.Equal("foo", lambda.Children.Skip(1).First().GetEx<string>());
            Assert.Equal("foo1", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("foo2", lambda.Children.Skip(1).First().Children.Skip(1).First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void ReturnNodesImpicitlyExpression()
        {
            var lambda = Common.Evaluate(@"
slots.create:foo
   .foo
      foo1:bar1
      foo2:bar2
   return:x:-/*
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
   return-nodes:x:-/*
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
   return-nodes:x:-/*
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
