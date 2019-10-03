/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.math.tests
{
    public class MathTests
    {
        [Fact]
        public void CreateSlot()
        {
            var lambda = Common.Evaluate(@"
slot:foo
   .lambda
      return-value:int:57
signal:foo");
            Assert.Equal(57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void OverwriteSlot()
        {
            var lambda = Common.Evaluate(@"
slot:foo
   .lambda
      return-value:int:57
slot:foo
   .lambda
      return-value:int:42
signal:foo");
            Assert.Equal(42, lambda.Children.Skip(2).First().Value);
        }
    }
}
