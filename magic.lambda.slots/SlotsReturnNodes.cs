/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.slots.utilities;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.return-nodes] slot for returning nodes from some evaluation object.
    /// </summary>
    [Slot(Name = "slots.return-nodes")]
    public class SlotsReturnNodes : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Peek<SlotResult>("slots.result").Result.AddRange(input.Value == null ? input.Children.ToList() : input.Evaluate());
        }
    }
}
