/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.exists] slot that will check if a dynamic slot exists or not.
    /// </summary>
    [Slot(Name = "slots.exists")]
    public class SlotsExists : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving slot's lambda, no reasons to clone, GetSlot will clone.
            input.Value = SlotsCreate.SlotExists(input.Get<string>());
        }
    }
}
