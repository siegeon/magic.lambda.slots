/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.slots.utilities;

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
            // Making sure we're able to handle returned values and nodes from slot implementation.
            var result = new SlotResult(input);
            signaler.Scope("slots.result", result, () =>
            {
                // Retrieving slot's lambda, no reasons to clone, GetSlot will clone.
                var lambda = SlotsCreate.GetSlot(input.Get<string>());

                // Preparing arguments, if there are any.
                if (input.Children.Any())
                    lambda.Insert(0, new Node(".arguments", null, input.Children.ToList()));

                // Evaluating lambda of slot.
                signaler.Signal("eval", lambda);

                // Clearing Children collection, since it might contain input parameters.
                input.Clear();

                /*
                * Simply setting value and children to "slots.result" stack object, since its value
                * was initially set to its old value during startup of method.
                */
                input.Value = result.Result.Value;
                input.AddRange(result.Result.Children.ToList());
            });
        }
    }
}
