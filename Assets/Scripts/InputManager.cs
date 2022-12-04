using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInitializable
{
    public static InputManager Instance;
    
    public UnityEvent OnConfirmTriggered = new UnityEvent();
    public UnityEvent OnCancelTriggered = new UnityEvent();

    public void Initialize()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

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
            GameManager.Instance.MovePlayers(PlayerMoveDirection.Up);
        }
    }

    public void OnMoveDownReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            GameManager.Instance.MovePlayers(PlayerMoveDirection.Down);
        }
    }

    public void OnMoveLeftReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            GameManager.Instance.MovePlayers(PlayerMoveDirection.Left);
        }
    }

    public void OnMoveRightReceived(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            GameManager.Instance.MovePlayers(PlayerMoveDirection.Right);
        }
    }
}

public interface IInitializable
{
    public void Initialize();
}