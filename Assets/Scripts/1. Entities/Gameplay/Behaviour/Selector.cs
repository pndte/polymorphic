using System;
using System.Collections.Generic;

namespace PEntities.Gameplay.Behaviour
{
    public class Selector : INode
    {
        private readonly IEnumerable<INode> _chldren;
        
        public Selector(IEnumerable<INode> children)
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
                        continue;
                    case NodeState.Success:
                        return NodeState.Success;
                    case NodeState.Running:
                        return  NodeState.Running;
                    default:
                        throw new Exception("Unexpected node state in selector");
                }
            }

            return NodeState.Failure;
        }
    }
}