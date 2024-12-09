using DG.Tweening;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakePower;
    
    private Tweener _tweener;
    public void Shake(Vector2 direction)
    {
        transform.DOPunchPosition(direction * -_shakePower, _shakeDuration).SetEase(Ease.OutBounce);
        
    }
}
