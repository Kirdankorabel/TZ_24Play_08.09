using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private float _minCarryingDistanseSqr = 0.01f;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        Track.OnTrackMoved += () => MoveAll();
        UIController.OnRestart += () => _trailRenderer.Clear();
    }

    public void MoveAll()
    {
        for (var i = 0; i < _trailRenderer.positionCount; i++)
            if((_trailRenderer.GetPosition(i) - transform.position).sqrMagnitude > _minCarryingDistanseSqr)
                _trailRenderer.SetPosition(i, _trailRenderer.GetPosition(i) - Vector3.forward * TrackInfo.elenemtSize);
    }
}
