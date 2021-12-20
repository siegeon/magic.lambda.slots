/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.create] slot that creates a dynamic slot, that can be invoked using the [signal] slot.
    /// </summary>
    [Slot(Name = "slots.create")]
    public class Create : ISlot
    {
        readonly IMagicCache _cache;

        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="cache">Cache implementation to use for actually storing slots.</param>
        public Create(IMagicCache cache)
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
            _cache.Upsert(
                input.Get<string>(),
                input.Clone(),
                DateTime.MaxValue,
                true);
        }
    }
}
