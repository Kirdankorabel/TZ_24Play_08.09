using UnityEngine;
using UnityEngine.Events;

public class Bottom : MonoBehaviour
{
    public event UnityAction<Pickup> OnPickupCollision;

    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();

        if(pickup && !pickup.IsUsed)
            OnPickupCollision?.Invoke(other.gameObject.GetComponent<Pickup>());
    }
}
