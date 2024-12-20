using System;
using System.Collections.Generic;

namespace PEntities.Gameplay.Behaviour
{
    public class Sequence : INode
    {
        private readonly IEnumerable<INode> _children;

        public Sequence(IEnumerable<INode> children)
        {
            _children = children;
        }

        public NodeState Evaluate()
        {
            foreach (INode node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        return NodeState.Running;
                    default:
                        throw new Exception("Unexpected node state in sequence");
                }
            }

            return NodeState.Success;
        }
    }
}