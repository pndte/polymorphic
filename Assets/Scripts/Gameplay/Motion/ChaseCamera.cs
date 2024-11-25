using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Motion
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private GameCameraConfig _config;
        private Transform _transform;

        [Inject]
        private void Construct(GameCameraConfig config)
        {
            _config = config;
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var smoothedPosition = Vector2.Lerp(_transform.position, _target.position, _config.ChaseSpeed);
            _transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, _transform.position.z);
        }
    }
}
