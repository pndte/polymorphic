using PEntities.Gameplay.Behaviour;

namespace PUseCases.Gameplay.AI
{
    public class Shooter : INode
    {
        public NodeState Evaluate()
        {
            return NodeState.Failure;
        }
    }
}