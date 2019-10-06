/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.slots.utilities;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.return-value] slot for returning a piece of value from some evaluation object.
    /// </summary>
    [Slot(Name = "slots.return-value")]
    public class SlotsReturnValue : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Peek<Node>("slots.result").Value = input.GetEx<object>();
        }
    }
}
