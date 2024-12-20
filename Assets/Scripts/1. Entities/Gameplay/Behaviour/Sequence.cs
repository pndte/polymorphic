using System;
using System.Collections.Generic;

namespace PEntities.Gameplay.Behaviour
{
    public class Sequence : INode
    {
        private readonly IEnumerable<INode> _chldren;

        public Sequence(IEnumerable<INode> children)
        {
            _chldren = children;
        }

        public NodeState Evaluate()
        {
            foreach (INode node in _chldren)
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