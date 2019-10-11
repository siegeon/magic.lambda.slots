/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@gaiasoul.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.return-value] slot for returning a piece of value from some evaluation object.
    /// </summary>
    [Slot(Name = "slots.return-value")]
    public class ReturnValue : ISlot
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
