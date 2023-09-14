using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private GameObject _scelet;
    [SerializeField] private Vector3 _startPosition = new Vector3(-0.031635616f, -0.0815873146f, -0.0336700901f);
    [SerializeField] private Vector3 _rotation = new Vector3(274.448639f, 42.5317841f, 180.768585f);

    private Vector3[] _startPositions;
    private Quaternion[] _startRotations;

    private void Start()
    {
        _startPositions = new Vector3[_rigidbodies.Length];
        _startRotations = new Quaternion[_rigidbodies.Length];
        for (var i = 0; i < _rigidbodies.Length; i++)
        {
            _startPositions[i] = _rigidbodies[i].gameObject.transform.localPosition;
            _startRotations[i] = _rigidbodies[i].gameObject.transform.localRotation;
        }
        DisablePhysics(true);
        Holder.OnHolderCollision += () => DisablePhysics(false);
        UIController.OnRestart += () => DisablePhysics(true);
    }

    private void DisablePhysics(bool enable)
    {
        if (enable)
        {
            _scelet.transform.localPosition = _startPosition;
            _scelet.transform.rotation = Quaternion.Euler(_rotation);
            for (var i = 0; i < _rigidbodies.Length; i++)
            {
                _rigidbodies[i].gameObject.transform.rotation = _startRotations[i];
                _rigidbodies[i].gameObject.transform.localPosition = _startPositions[i];
            }
        }
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = enable;
            rigidbody.velocity = Vector3.zero;
        }
        _animator.enabled = enable;
    }
}
