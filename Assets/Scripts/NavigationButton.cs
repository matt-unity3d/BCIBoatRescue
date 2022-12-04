using System;
using UnityEngine;
using UnityEngine.Events;

public class NavigationButton : MonoBehaviour
{
    private enum ButtonType
    {
        Select,
        Cancel,
    }

    [SerializeField] private ButtonType _type;
    [SerializeField] private UnityEvent _onInputReceived;

    private bool _registerOnStart;

    private void Start()
    {
        if (_registerOnStart)
        {
            Register();
        }
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            Register();
        }
        else
        {
            _registerOnStart = true;
        }
    }
    
    private void OnDisable()
    {
        Unregister();
    }

    private void OnInputTriggered()
    {
        _onInputReceived?.Invoke();
    }

    private void Register()
    {
        switch (_type)  
        {
            case ButtonType.Select:
                InputManager.Instance.OnConfirmTriggered.AddListener(OnInputTriggered);
                break;
            case ButtonType.Cancel:
                InputManager.Instance.OnCancelTriggered.AddListener(OnInputTriggered);
                break;
        }
    }

    private void Unregister()
    {
        switch (_type)  
        {
            case ButtonType.Select:
                InputManager.Instance.OnConfirmTriggered.RemoveListener(OnInputTriggered);
                break;
            case ButtonType.Cancel:
                InputManager.Instance.OnCancelTriggered.RemoveListener(OnInputTriggered);
                break;
        }
    }
}