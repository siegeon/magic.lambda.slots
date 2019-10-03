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
using magic.lambda.slots.utilities;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slot] slot that creates a dynamic slot, that can be invoked using the [signal] slot.
    /// </summary>
    [Slot(Name = "slot")]
    public class Slot : ISlot
    {
        readonly static Synchronizer<Dictionary<string, Node>> _slots = new Synchronizer<Dictionary<string, Node>>(new Dictionary<string, Node>());

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (!input.Children.Any(x => x.Name == ".lambda"))
                throw new ApplicationException("Keyword [slot] requires at least a [.lambda] children node");

            if (input.Children.Any((x => x.Name != ".lambda" && x.Name != ".arguments")))
                throw new ApplicationException("Keyword [slot] can only handle [.lambda] and [.arguments] children nodes");

            var slotNode = new Node();
            slotNode.AddRange(input.Children.Select(x => x.Clone()));
            _slots.Write((slots) => slots[input.GetEx<string>()] = slotNode);
        }

        #region [ -- Private and internal helper methods -- ]

        /*
         * Reutrns the named slot to caller.
         */
        internal static Node GetSlot(string name)
        {
            return _slots.Read((slots) => slots[name].Clone());
        }

        #endregion
    }
}
