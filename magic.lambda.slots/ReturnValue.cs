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
    /// [return-value] slot for returning a piece of value from some evaluation object.
    /// </summary>
    [Slot(Name = "return-value")]
    public class ReturnValue : ISlot
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

            var root = input;

            // Notice, we store the return value as the value (by reference) of the root node of whatever lambda object we're currently within.
            while (root.Parent != null)
                root = root.Parent;
            root.Value = input.GetEx<object>();
        }
    }
}
