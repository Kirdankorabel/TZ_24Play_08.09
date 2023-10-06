using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Bottom _playerBase;
    [SerializeField] private Holder _holder;
    [SerializeField] private GameObject _character;

    private List<Pickup> _pickups = new List<Pickup>();

    public static event UnityAction OnCollision;
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
        _playerBase.transform.localPosition = Vector3.up / 2f;
        _holder.transform.localPosition = Vector3.up / 2f;
    }

    private void AddPickup(Pickup pickup)
    {
        if (_pickups.Contains(pickup)) return;

        _pickups.Add(pickup);
        pickup.transform.parent = transform;
        pickup.OnWallCollision += () => RemovePickup(pickup);

        for (var i = 0; i < _pickups.Count; i++)
            pickup.transform.localPosition = Vector3.up * i + Vector3.up / 2f;

        _playerBase.transform.localPosition = Vector3.up / 2f;
        _holder.transform.localPosition = Vector3.up * (_pickups.Count);
        OnPickupAdded?.Invoke();
    }

    private void RemovePickup(Pickup pickup)
    {
        OnCollision?.Invoke();
        Handheld.Vibrate();

        foreach (var p in _pickups) 
            p.EnableGravity();

        _pickups.Remove(pickup);
        _holder.EnableGravity();
        Track.Parented(pickup.gameObject);
    }    
}
