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

    public void AssignStimulusObjects()
    {
        if (_bciController == null)
        {
            return;
        }
        
        _bciController.objectList = _activeSpoObjects
                                    .Select(o => o.gameObject)
                                    .ToList();
    }
    
    public void ToggleStimulusInput(bool enable)
    {
        if (_bciController == null)
        {
            return;
        }
        
        if (enable)
        {
            AssignStimulusObjects();
            _bciController.StimulusOn();
        }
        else
        {
            _bciController.StimulusOff();
        }
    }
    
    public void RegisterSpoObject(SPO spo)
    {
        _activeSpoObjects.Add(spo);
    }

    public void UnregisterSpoObject(SPO spo)
    {
        _activeSpoObjects.Remove(spo);
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