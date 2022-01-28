/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.create] slot that creates a dynamic slot, that can be invoked using the [signal] slot.
    /// </summary>
    [Slot(Name = "slots.create")]
    public class Create : ISlot, ISlotAsync
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
            SignalAsync(signaler, input).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await _cache.UpsertAsync(
                "slots." + input.Get<string>(),
                HyperlambdaGenerator.GetHyperlambda(input.Children, false),

                // Notice, to avoid funny "locale issues" with locales not having 9999 years, we use 100 years and NOT MaxValue
                DateTime.UtcNow.AddYears(100),
                true);
        }
    }
}
