/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.caching.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [signal] slot for invoking dynamically created slots, that have been created with the [slots.create] slot.
    /// </summary>
    [Slot(Name = "signal")]
    public class SignalSlot : ISlot, ISlotAsync
    {
        readonly IMagicCache _cache;

        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="cache">Cache implementation to use for actually storing slots.</param>
        public SignalSlot(IMagicCache cache)
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
            // Making sure we're able to handle returned values and nodes from slot implementation.
            var result = new Node();
            signaler.Scope("slots.result", result, () =>
            {
                // Evaluating lambda of slot, making sure we temporary clear any existing [whitelist] declarations.
                var lambda = GetLambda(signaler, input);
                signaler.Scope("whitelist", null, () =>
                {
                    signaler.Signal("eval", lambda);
                });

                input.Clear();
                input.Value = result.Value;
                input.AddRange(result.Children.ToList());
            });
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        /// <result>Arguments to slot.</result>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Making sure we're able to handle returned values and nodes from slot implementation.
            var result = new Node();
            await signaler.ScopeAsync("slots.result", result, async () =>
            {
                // Evaluating lambda of slot, making sure we temporary clear any existing [whitelist] declarations.
                var lambda = GetLambda(signaler, input);
                await signaler.ScopeAsync("whitelist", null, async () =>
                {
                    await signaler.SignalAsync("eval", lambda);
                });

                input.Clear();
                input.Value = result.Value;
                input.AddRange(result.Children.ToList());
            });
        }

        #region [ -- Private helper methods -- ]

        Node GetLambda(ISignaler signaler, Node input)
        {
            var name = input.GetEx<string>();

            var whitelist = signaler.Peek<List<Node>>("whitelist");
            if (whitelist != null && !whitelist.Any(x => x.Name == "signal" && x.Get<string>() == name))
                throw new HyperlambdaException($"Dynamic slot [{name}] does not exist in scope");

            var lambda = (_cache.Get(name, true) as Node).Clone();

            // Preparing arguments, if there are any.
            if (input.Children.Any())
                lambda.Insert(0, new Node(".arguments", null, input.Children.ToList()));

            return lambda;
        }

        #endregion
    }
}
