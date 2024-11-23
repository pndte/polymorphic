using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KeyboardMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _directionIncreaseSpeed = 0.05f;

    private Rigidbody2D _physics;
    private Vector2 _cachedDirection = Vector2.zero;

    private void Awake()
    {
        Init(); // TODO: move to Load Pool
    }

    public void Init()
    {
        _physics = GetComponent<Rigidbody2D>();
    }

    private void Move(Vector2 direction)
    {
        _physics.velocity = direction * _speed;
    }

    void Update()
    {
        DefineDirection();
    }

    private void FixedUpdate()
    {
        Move(_cachedDirection);
    }

    private void DefineDirection()
    {
        if (Input.GetKey(KeyCode.A)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x - _directionIncreaseSpeed, -1, 0);
        else if (Input.GetKey(KeyCode.D)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x + _directionIncreaseSpeed, 0, 1);
        else _cachedDirection.x = 0;

        if (Input.GetKey(KeyCode.S)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y - _directionIncreaseSpeed, -1, 0);
        else if (Input.GetKey(KeyCode.W)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y + _directionIncreaseSpeed, 0, 1);
        else _cachedDirection.y = 0;
    }
}