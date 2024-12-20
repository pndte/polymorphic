namespace PEntities.Gameplay.Behaviour
{
    public class RootNode : INode
    {
        private readonly INode _root;

        public RootNode(INode root)
        {
            _root = root;
        }

        public NodeState Evaluate()
        {
            return _root.Evaluate();
        }
    }
}