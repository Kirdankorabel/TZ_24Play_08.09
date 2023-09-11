using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TrackElement : MonoBehaviour
{
    [SerializeField] private float _takeoffSpeed = 5f;

    private int _id;
    private Vector3 _targetPosition;
    private List<GameObject> walls;
    private List<GameObject> pickups;

    public event UnityAction<int> OnPlayerEnter;

    public int ID
    {
        get { return _id; }
        set { _id = _id == 0 ? value : _id; }
    }

    public Vector3 TargetPosition
    {
        set
        {
            _targetPosition = value;
            StartCoroutine(TakeoffCorutine());
        }
    }

    public void AddPickup(GameObject pickup)
    {
        pickups.Add(pickup);
    }

    public void Initialize()
    {
        if (pickups != null)
            PickupCreator.Instance.ClearPlatforms(pickups.Where(platform => platform.transform.parent == transform));
        if (walls != null)
            WallCreator.Instance.ClearWall(walls);

        pickups = PickupCreator.Instance.CreatePlatforms(transform);
        walls = WallCreator.Instance.CreateWall(transform);
    }

    public void Clear()
    {
        if (pickups != null)
            PickupCreator.Instance.ClearPlatforms(pickups.Where(platform => platform.transform.parent == transform));
        if (walls != null)
            WallCreator.Instance.ClearWall(walls);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMover>() && other.transform.parent != transform)
        {
            other.transform.parent = transform;
            OnPlayerEnter?.Invoke(ID);
        }
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
