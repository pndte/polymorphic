namespace PEntities.Gameplay.Behaviour
{
    public class RootNode: Node
    {
        private readonly Node _root;
        
        public RootNode(Node root)
        {
            _root = root;
        }

        public override NodeState Evaluate()
        {
            return _root.Evaluate();
        }
    }
}