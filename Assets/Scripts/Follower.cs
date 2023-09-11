using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private bool _fixXY;
    private Vector3 _offset;
    void Start()
    {
        _offset = transform.position - _targetTransform.position;
    }

    private void Update()
    {
        if (!_fixXY)
            transform.position = _targetTransform.position + _offset;
        else
            transform.position = _targetTransform.position.z * Vector3.forward + _offset;
    }
}
