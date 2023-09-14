using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TrackElement : MonoBehaviour
{
    [SerializeField] private float _takeoffSpeed = 5f;

    private List<GameObject> _walls;
    private List<GameObject> _pickups;
    private Vector3 _targetPosition;

    public event UnityAction OnPlayerEnter;

    public Vector3 TargetPosition
    {
        set
        {
            _targetPosition = value;
            StartCoroutine(TakeoffCorutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.transform.parent != transform)
        {
            other.transform.parent = transform;
            OnPlayerEnter?.Invoke();
        }
    }

    public void AddPickup(GameObject pickup)
    {
        _pickups.Add(pickup);
    }

    public void Initialize()
    {
        if (_pickups != null)
            PickupCreator.Instance.ClearPlatforms(_pickups.Where(platform => platform.transform.parent == transform));
        if (_walls != null)
            WallCreator.Instance.ClearWall(_walls);

        _pickups = PickupCreator.Instance.CreatePlatforms(transform);
        _walls = WallCreator.Instance.CreateWall(transform);
    }

    public void Clear()
    {
        if (_pickups != null)
            PickupCreator.Instance.ClearPlatforms(_pickups.Where(platform => platform.transform.parent == transform));
        if (_walls != null)
            WallCreator.Instance.ClearWall(_walls);
    }

    private IEnumerator TakeoffCorutine()
    {
        var startSQRDistance = (_targetPosition - transform.position).sqrMagnitude;
        while (transform.position != _targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _takeoffSpeed
                * (_targetPosition - 0.6f * transform.position).sqrMagnitude / startSQRDistance);
            yield return null;
        }
    }
}
