using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CollectCubeEffect : MonoBehaviour
{
    [SerializeField] private GameObject _effectPrefab;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _defaultPoolCapacity;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private float _lifetime = 2f;

    private ObjectPool<GameObject> _pool;

    void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            Instantiate,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultPoolCapacity,
            _maxPoolSize);
    }

    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    public void Spawn()
    {
        StartCoroutine(ParticleLifeCorutine());
    }

    private IEnumerator ParticleLifeCorutine()
    {
        var go = _pool.Get();
        go.transform.position = transform.position + _offset;
        Track.Parented(go);
        yield return new WaitForSeconds( _lifetime);
        _pool.Release(go);
    }

    #region pool methods
    private GameObject Instantiate()
    {
        var gameObject = Instantiate(_effectPrefab);
        return gameObject;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnReleas(GameObject gameObject)
    {
        gameObject.transform.parent = null;
        gameObject.SetActive(false);
    }

    private void OnDestroyElement(GameObject gameObject) { }
    #endregion
}
