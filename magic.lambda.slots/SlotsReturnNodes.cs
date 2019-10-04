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
            // Notice, we store the return value as the value (by reference) of the root node of whatever lambda object we're currently within.
            var root = input;
            while (root.Parent != null)
                root = root.Parent;

            // Figuring out if we should return children or evaluation of expression to caller.
            if (input.Value == null)
                root.Value = input.Children;
            else
                root.Value = input.Evaluate().ToList();
        }
    }
}
