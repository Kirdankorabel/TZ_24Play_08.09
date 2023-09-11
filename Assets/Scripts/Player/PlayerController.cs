using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Base _playerBase;
    [SerializeField] private Holder _holder;
    [SerializeField] private GameObject _character;

    private List<Pickup> _pickups = new List<Pickup>();

    public UnityEvent OnPickupAdded;

    private void Start()
    {
        _playerBase.OnPickupCollision += (pickup) => AddPickup(pickup);
        UIController.OnRestart += () => Reset();
        Holder.OnHolderCollision += () =>
        {
            while(_pickups.Count > 0)
                RemovePickup(_pickups[0]);
        };
    }

    private void Reset()
    {
        _playerBase.transform.localPosition = Vector3.up * 0.5f;
        _holder.transform.localPosition = Vector3.up * 0.5f;
    }

    private void AddPickup(Pickup pickup)
    {
        _playerBase.transform.localPosition = Vector3.up * 0.5f;
        if (_pickups.Contains(pickup)) return;
        _pickups.Add(pickup);
        pickup.transform.parent = transform;
        for(var i = 0; i < _pickups.Count; i++)
        {
            pickup.transform.localPosition = Vector3.up * (i + 0.5f);
        }
        _holder.transform.localPosition = Vector3.up * (_pickups.Count);
        pickup.OnWallCollision += () => RemovePickup(pickup);
        OnPickupAdded?.Invoke();
    }

    private void RemovePickup(Pickup pickup)
    {
        Handheld.Vibrate();
        _pickups.Remove(pickup);
        foreach (var p in _pickups) 
            p.EnablePhysics();
        _holder.EnablePhysics();
        Track.Parented(pickup.gameObject);
    }    
}
