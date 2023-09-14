using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Track : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PickupCreator _pickupCreator;
    [SerializeField] private WallCreator _wallCreator;
    [SerializeField] private TrackElement _trackGroundPrefab;
    [SerializeField] private Vector3 _elementRespawnPosition = new Vector3(0, -50, 0);
    [SerializeField] private int _elementsCount = 5;

    private static List<TrackElement> _elements = new List<TrackElement>();
    private static float _scaleZ;

    public static event UnityAction OnTrackMoved;

    private void Awake()
    {
        _scaleZ = _trackGroundPrefab.transform.localScale.z;
        UIController.OnRestart += () => Reset();
    }

    private void Start()
    {
        CreateTrack();
        _playerTransform.parent = _elements[0].transform;
    }

    private void Reset()
    {
        TrackElement element;
        _elements[0].Clear();
        for (var i = 0; i < _elementsCount; i++)
        {
            element = _elements[i];
            element.transform.position = Vector3.forward * i * _scaleZ;
            if (i > 0)
                element.Initialize();
        }
    }

    public static void Parented(GameObject child)
    {
        var platform = _elements[(int)(child.transform.position.z / _scaleZ)];
        child.transform.parent = platform.transform;
        if (child.gameObject.GetComponent<Pickup>())
            platform.AddPickup(child);
    }

    private void CreateTrack()
    {
        TrackElement element;
        for (var i = 0; i < _elementsCount; i++)
        {
            element = Instantiate(_trackGroundPrefab, Vector3.forward * i * _scaleZ ,Quaternion.identity, transform);
            _elements.Add(element);
            element.GetComponent<TrackElement>().OnPlayerEnter += () => MoveTrack();
            if(i > 0)
                element.Initialize();
        }
    }

    private void MoveTrack()
    {
        var firstElement = _elements[0];
        for (var i = 1; i < _elementsCount; i++)
        {
            _elements[i].gameObject.transform.position = Vector3.forward * (i - 1) * _scaleZ;
            _elements[i - 1] = _elements[i];
        }
        firstElement.transform.position = _elementRespawnPosition + Vector3.forward * ((_elementsCount - 1) * _scaleZ);
        _elements[_elementsCount - 1] = firstElement;
        firstElement.TargetPosition = Vector3.forward * (_elementsCount - 1) * _scaleZ;

        OnTrackMoved?.Invoke();
        firstElement.Initialize();
    }
}
