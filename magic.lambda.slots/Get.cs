/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.get] slot for retrieving slot that has been created with the [slots.create] slot.
    /// </summary>
    [Slot(Name = "slots.get")]
    public class Get : ISlot
    {
        readonly IMagicCache _cache;

        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="cache">Cache implementation to use for actually storing slots.</param>
        public Get(IMagicCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var lambda = (_cache.Get(input.Get<string>(), true) as Node).Clone();
            input.AddRange(lambda.Children.ToList());
        }
    }
}
