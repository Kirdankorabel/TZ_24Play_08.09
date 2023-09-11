using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PickupCreator : Singleton<PickupCreator>
{
    [SerializeField] private GameObject _pickupPrefab;
    [SerializeField] private int _defaultPoolCapacity;
    [SerializeField] private int _maxPoolSize;

    private ObjectPool<GameObject> _pool;

    void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            InstantiateObstacle,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultPoolCapacity,
            _maxPoolSize);
    }

    public List<GameObject> CreatePlatforms(Transform trackElementTransform)
    {
        GameObject pickup;
        List<GameObject> pickups = new List<GameObject>();
        var offset = TrackInfo.elenemtSize / (TrackInfo.pickupCount + 2);
        for (var i = 0; i < TrackInfo.pickupCount; i++)
        {
            pickup = _pool.Get();
            pickup.transform.position = trackElementTransform.position + new Vector3(Random.Range(TrackInfo.min, TrackInfo.max + 1), 0.5f, (i + 1) * offset);
            pickup.transform.parent = trackElementTransform;
            pickups.Add(pickup);
        }
        return pickups;
    }

    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    public void ClearPlatforms(IEnumerable<GameObject> platforms)
    {
        foreach (var platform in platforms)
            _pool.Release(platform);
    }

    #region pool methods
    private GameObject InstantiateObstacle()
    {
        var obstacle = Instantiate(_pickupPrefab);
        return obstacle;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnReleas(GameObject gameObject)
    {
        gameObject.transform.parent = null;
        gameObject.GetComponent<Pickup>().Reset();
        gameObject.SetActive(false);
    }

    private void OnDestroyElement(GameObject gameObject) { }
    #endregion
}