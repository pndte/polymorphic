using System.Collections.Generic;

namespace PEntities.Gameplay.Behaviour
{
    public abstract class Node
    {
        protected NodeState State;
        protected readonly IList<Node> Children; // TODO: nullable mb?

        protected Node(IList<Node> children)
        {
            Children = children;
        }

        protected Node()
        {
            Children = new List<Node>();
        }
        
        public abstract NodeState Evaluate();
    }
}