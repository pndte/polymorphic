using UnityEngine;

public class ToMouseRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = Vector2.Lerp(transform.up, direction, 0.125f);
    }
}
