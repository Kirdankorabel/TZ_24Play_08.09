using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private float _delayTime = 0.7f;

    private Rigidbody _rb;
    private bool _isUsed;

    public event UnityAction OnWallCollision;

    public bool IsUsed => _isUsed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _isUsed = true;
            OnWallCollision?.Invoke();
            OnWallCollision = null;
        }
    }

    public void Reset()
    {
        _isUsed = false;
    }

    public void EnablePhysics()
    {
        if (!_rb.isKinematic)
            StartCoroutine(EnablePhysicsCorutine());
    }

    private IEnumerator EnablePhysicsCorutine()
    {
        _rb.isKinematic = true;
        yield return new WaitForSeconds(_delayTime);
        _rb.isKinematic = false; 
    }
}