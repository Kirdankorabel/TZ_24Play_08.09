using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Animator _startPanelAnimator;
    [SerializeField] private Animator _restartPanelAnimator;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private float _animTimeDelay = 0.7f;

    public static event Action OnRestart;
    public static event Action OnStarted;

    private void Awake()
    {
        _restartPanelAnimator.SetBool("isEnable", false);
    }

    void Start()
    {
        Holder.OnHolderCollision += () =>
        {
            _restartPanelAnimator.SetBool("isEnable", true);
        };
        _restartButton.onClick.AddListener(() =>
        {
            _restartPanelAnimator.SetBool("isEnable", false);
            StartCoroutine(WaiteCorutine(_animTimeDelay, () => OnRestart?.Invoke()));
        });
        _startButton.onClick.AddListener(() =>
        {
            _startPanelAnimator.SetBool("isEnable", false);
            StartCoroutine(WaiteCorutine(_animTimeDelay, () => OnStarted?.Invoke()));
        });
    }

    private IEnumerator WaiteCorutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
