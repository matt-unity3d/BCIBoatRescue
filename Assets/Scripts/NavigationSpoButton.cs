using UnityEngine;

public class NavigationSpoButton : SpoButton
{
    [SerializeField] private PlayerMoveDirection _direction;
    
    protected override void Register()
    {
        InputManager.Instance.RegisterSpoObject(this);
    }

    protected override void Unregister()
    {
        if (InputManager.Instance == null)
        {
            return;
        }
        
        InputManager.Instance.UnregisterSpoObject(this);
    }
    
    public override void OnSelection()
    {
        GameManager.Instance.MovePlayers(_direction);
        Debug.Log($"SPO Object selected: {gameObject.name}. {_direction}");
        _onInputReceived?.Invoke();
    }
}