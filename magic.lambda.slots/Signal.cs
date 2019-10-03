/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [signal] slot for invoking dynamically created slots, that have been created with the [slot] slot.
    /// </summary>
    [Slot(Name = "signal")]
    public class Signalize : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving slot's lambda.
            var slotName = input.GetEx<string>() ?? throw new ApplicationException("Keyword [signal] requires a value being the name of slot to invoke");
            var slotNode = Slot.GetSlot(slotName);
            var lambda = slotNode.Children.First(x => x.Name == ".lambda");

            // Making sure lambda becomes its own root node.
            // No need to clone, GetSlot has already cloned.
            lambda.UnTie();

            // Preparing arguments, making sure we clon ethem to avoid that enumeration process is aborted.
            var args = new Node(".arguments");
            args.AddRange(input.Children.Select(x => x.Clone()));
            lambda.Insert(0, args);

            // Evaluating lambda of slot.
            signaler.Signal("eval", lambda);

            // Returning any returned nodes from lambda.
            input.Clear();
            input.Value = null;
            if (lambda.Value is IEnumerable<Node> nodes)
                input.AddRange(nodes.ToList());
            else
                input.Value = lambda.Value;
        }
    }
}
