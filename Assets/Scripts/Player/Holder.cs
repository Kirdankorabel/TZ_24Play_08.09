using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Holder : MonoBehaviour
{
    [SerializeField] private float _delayTime = 0.7f;

    private Rigidbody _rb;

    public static event UnityAction OnHolderCollision;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
            OnHolderCollision?.Invoke();
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
