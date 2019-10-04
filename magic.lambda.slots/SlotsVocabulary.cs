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
    /// [slots.vocabulary] slot that will return the names of all dynamically created slots to caller.
    /// </summary>
    [Slot(Name = "slots.vocabulary")]
    public class SlotsVocabulary : ISlot
    {
        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving slot's lambda, no reasons to clone, GetSlot will clone.
            input.AddRange(SlotsCreate.Slots().Select(x => new Node("", x)));
        }
    }
}
