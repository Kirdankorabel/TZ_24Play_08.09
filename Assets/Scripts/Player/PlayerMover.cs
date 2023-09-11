using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private ParticleSystem _moveEffect;

    private bool _isPlaying = false;
    private float _trackWidth;

    void Start()
    {
        _moveEffect.Stop();
        Holder.OnHolderCollision += () => _isPlaying = false;
        UIController.OnRestart += () => Reset();
        UIController.OnStarted += () =>
        {
            Reset();
            _moveEffect.Play();
        };
        _trackWidth = TrackInfo.max + TrackInfo.min + 1;
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!_isPlaying)
            return;
        transform.position += Vector3.forward * Time.deltaTime * _speed;
        float positionx = 0;
        positionx = (Input.mousePosition.x) / Screen.width * 5f - 2.5f;
        if (Input.touchCount > 0)
        {
            positionx = (Input.GetTouch(0).position.x) / Screen.width * 5f - 2.5f;
            Debug.LogError(positionx);
        }
        positionx = positionx > 2 ? 2 : positionx;
        positionx = positionx < -2 ? -2 : positionx;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionx, transform.position.y, transform.position.z), _speed * Time.deltaTime);
    }

    private void Reset()
    {
        _isPlaying = true;
        transform.position = _startPosition;
    }
}
