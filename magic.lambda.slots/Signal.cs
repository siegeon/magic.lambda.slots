/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [signal] slot for invoking dynamically created slots, that have been created with the [slots.create] slot.
    /// </summary>
    [Slot(Name = "signal")]
    public class SignalSlot : ISlot, ISlotAsync
    {
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
                throw new ArgumentException($"Dynamic slot [{name}] does not exist in scope");
            var lambda = Create.GetSlot(name);

            // Preparing arguments, if there are any.
            if (input.Children.Any())
                lambda.Insert(0, new Node(".arguments", null, input.Children.ToList()));

            return lambda;
        }

        #endregion
    }
}
