/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

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
            if (input.Children.Any())
                throw new ApplicationException("Slot [return-value] cannot have children nodes");

            // Notice, we store the return value as the value (by reference) of the root node of whatever lambda object we're currently within.
            var root = input;
            while (root.Parent != null)
                root = root.Parent;
            root.Value = input.GetEx<object>();
        }
    }
}
