using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WallCreator : Singleton<WallCreator>
{
    [SerializeField] private Walls _walls;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<GameObject> _pool;
    private int _elenemtSize;
    private int _pickupCount;
    private int _lastWall;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            InstantiateObstacle,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultCapacity,
            _maxSize);

        _elenemtSize = TrackInfo.elenemtSize;
        _pickupCount = TrackInfo.pickupCount;
    }

    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    public List<GameObject> CreateWall(Transform trackElementTransform)
    {
        var wallId = Random.Range(0, _walls.GetWallInfo.Count);
        if (_lastWall == wallId)
            wallId = Random.Range(0, _walls.GetWallInfo.Count);
        _lastWall = wallId;

        GameObject wallElement;
        var wall = new List<GameObject>();
        var wallInfo = _walls.GetWallInfo[wallId];
        var wallPosition = Vector3.forward * _elenemtSize / _pickupCount * (_pickupCount - 1);
        for (var i = 0; i < wallInfo.elements.Count; i++)
        {
            wallElement = _pool.Get();
            wallElement.transform.localPosition = trackElementTransform.position + wallPosition + wallInfo.elements[i].position;
            wallElement.transform.localScale = new Vector3(1, wallInfo.elements[i].height, 1);
            wallElement.transform.parent = trackElementTransform;
            wall.Add(wallElement);
        }

        return wall;
    }

    public void ClearWall(IEnumerable<GameObject> wall)
    {
        foreach (var wallElement in wall)
            _pool.Release(wallElement);
    }

    #region pool methods
    private GameObject InstantiateObstacle()
    {
        var obstacle = Instantiate(_wallPrefab);
        return obstacle;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.transform.parent = null;
        gameObject.SetActive(true);
    }

    private void OnReleas(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroyElement(GameObject gameObject) { }
    #endregion
}
