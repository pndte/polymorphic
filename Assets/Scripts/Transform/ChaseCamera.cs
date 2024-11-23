using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    private Transform _transform;

    private void Awake()
    {
        Init(); // TODO: Load pool
    }

    public void Init()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        var smoothedPosition = Vector2.Lerp(_transform.position, _target.position, _speed);
        _transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, _transform.position.z);
    }
}
