/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using magic.node;

namespace magic.lambda.slots.utilities
{
    /*
     * Helper class to allow for slots to return values and children nodes.
     */
    internal class SlotResult
    {
        public SlotResult(Node slotNode)
        {
            Result = new Node(slotNode.Name, slotNode.Value);
        }

        public Node Result { get; }
    }
}