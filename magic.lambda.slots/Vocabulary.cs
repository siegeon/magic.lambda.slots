/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [slots.vocabulary] slot that will return the names of all dynamically created slots to caller.
    /// </summary>
    [Slot(Name = "slots.vocabulary")]
    public class Vocabulary : ISlot
    {
        readonly IMagicCache _cache;

        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="cache">Cache implementation to use for actually storing slots.</param>
        public Vocabulary(IMagicCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var filter = ".slot" + input.GetEx<string>();
            input.Value = null;
            var list = _cache
                .Items(filter)
                .Select(x => x.Key.Substring(5))
                .ToList();
            list.Sort((lhs, rhs) => string.Compare(lhs, rhs, System.StringComparison.InvariantCulture));
            var whitelist = signaler.Peek<List<Node>>("whitelist");
            input.AddRange(list
                .Where(x => whitelist == null || whitelist.Any(x2 => x2.Name == "signal" && x2.Get<string>() == x))
                .Select(x => new Node("", x)));
        }
    }
}
