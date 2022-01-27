/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.exists] slot that will check if a dynamic slot exists or not.
    /// </summary>
    [Slot(Name = "slots.exists")]
    public class Exists : ISlot, ISlotAsync
    {
        readonly IMagicCache _cache;

        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="cache">Cache implementation to use for actually storing slots.</param>
        public Exists(IMagicCache cache)
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
            input.Value = await _cache.GetAsync("slots." + input.Get<string>(), true) != null;
        }
    }
}
