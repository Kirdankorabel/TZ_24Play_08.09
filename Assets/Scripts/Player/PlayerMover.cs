using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveEffect;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _speed = 10f;

    private bool _isPlaying = false;
    private float _trackWidth;
    private float _minPositionX, _maxPositionX;

    void Start()
    {
        Holder.OnHolderCollision += () => _isPlaying = false;
        UIController.OnRestart += () => Reset();
        UIController.OnStarted += () =>
        {
            Reset();
            _moveEffect.Play();
        };
        SetStartState();
    }

    private void FixedUpdate()
    {
        if (!_isPlaying)
            return;
        float positionx = (Input.mousePosition.x) / Screen.width * _trackWidth - _trackWidth / 2f;
        positionx = positionx > _maxPositionX ? _maxPositionX : positionx;
        positionx = positionx < _minPositionX ? _minPositionX : positionx;

        transform.position += Vector3.forward * Time.deltaTime * _speed;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionx, transform.position.y, transform.position.z), _speed * Time.deltaTime);
    }

    private void Reset()
    {
        _isPlaying = true;
        transform.position = _startPosition;
    }

    private void SetStartState()
    {
        _moveEffect.Stop();
        _minPositionX = TrackInfo.min;
        _maxPositionX = TrackInfo.max;
        _trackWidth = _maxPositionX - _minPositionX + 1;
        _startPosition = transform.position;
    }
}
