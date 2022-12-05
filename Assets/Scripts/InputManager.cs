using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInitializable
{
    public static InputManager Instance;

    public UnityEvent<PlayerMoveDirection> OnDirectionTriggered = new();
    public UnityEvent OnConfirmTriggered = new();
    public UnityEvent OnCancelTriggered = new();

    private P300Controller _bciController;
    private List<SPO> _activeSpoObjects = new();

    public void Initialize()
    {
        Instance = this;
        _bciController = FindObjectOfType<P300Controller>();
        
        DontDestroyOnLoad(this);
    }

    #region BCI Input
    
    public void ToggleStimulusInput(bool enable)
    {
        if (_bciController == null || _bciController.stimOn == enable)
        {
            return;
        }
        
        Debug.Log("<b>Starting Stim</b>");
        Debug.Log($"<b>{_bciController.objectList.Count}</b>");
        _bciController.StartStopStimulus();
    }

    #endregion

    
    #region Unity Input

    public void OnConfirmReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnConfirmTriggered?.Invoke();
        }
    }

    public void OnCancelReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnCancelTriggered?.Invoke();
        }
    }

    public void OnMoveUpReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnDirectionTriggered?.Invoke(PlayerMoveDirection.Up);
        }
    }

    public void OnMoveDownReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnDirectionTriggered?.Invoke(PlayerMoveDirection.Down);
        }
    }

    public void OnMoveLeftReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnDirectionTriggered?.Invoke(PlayerMoveDirection.Left);
        }
    }

    public void OnMoveRightReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            OnDirectionTriggered?.Invoke(PlayerMoveDirection.Right);
        }
    }

    #endregion
}