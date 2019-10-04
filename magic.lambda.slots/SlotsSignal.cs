/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.signal] slot for invoking dynamically created slots, that have been created with the [slots.create] slot.
    /// </summary>
    [Slot(Name = "slots.signal")]
    public class SlotsSignal : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving slot's lambda, no reasons to clone, GetSlot will clone.
            var lambda = SlotsCreate.GetSlot(input.Get<string>());

            // Preparing arguments, if there are any.
            if (input.Children.Any())
                lambda.Insert(0, new Node(".arguments", null, input.Children.ToList()));

            // Evaluating lambda of slot.
            signaler.Signal("eval", lambda);

            // Making sure we return any return values, if any, to the caller.
            input.Clear();
            input.Value = null;
            if (lambda.Value is IEnumerable<Node> nodes)
                input.AddRange(nodes.ToList());
            else
                input.Value = lambda.Value;
        }
    }
}
