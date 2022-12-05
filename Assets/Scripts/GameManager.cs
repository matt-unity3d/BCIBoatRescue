using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Initialize")]
    [SerializeField] private GameObject[] _initializables = Array.Empty<GameObject>();
    [SerializeField] private string _initializeSceneName;
    
    [Header("TestLevel")]
    [SerializeField] private LevelBuilderInstructions _testLevel;

    public static GameManager Instance;

    public Level ActiveLevel { get; private set; }

    //Services
    private InputManager _inputManager;
    private UIManager _uiManager;
    private LevelBuilder _builder;

    private int _movesCount = 0;
    private int _totalRewards = 0;
    private int _rewardsCount = 0;

    private Coroutine _waitForMovementCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            foreach (var initGO in _initializables)
            {
                var initializables = initGO.GetComponents<IInitializable>();
                foreach (var initializable in initializables)
                {
                    initializable.Initialize();
                }
            }
            
            _inputManager = InputManager.Instance;
            _inputManager.OnDirectionTriggered.AddListener(MovePlayers);
            LoadScene(_initializeSceneName);
        }
    }

    #region Managers

    public void RegisterBuilder(LevelBuilder builder)
    {
        _builder = builder;
        if (ActiveLevel == null)
        {
            EnterLevel();
        }
    }
    
    public void UnregisterBuilder(LevelBuilder builder)
    {
        if (_builder == builder)
        {
            _builder = null;
        }
    }

    public void RegisterUIManager(UIManager manager)
    {
        _uiManager = manager;
    }
    
    public void UnregisterUIManager(UIManager manager)
    {
        if (_uiManager == manager)
        {
            _uiManager = null;
        }
    }

    #endregion

    #region SceneManagement
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion
    
    [ContextMenu("Build Test Level")]
    public void EnterLevel()
    {
        Reset();
        
        ActiveLevel = _builder.BuildLevel(_testLevel);
        _uiManager.InitializeLevelDetails(ActiveLevel);
        _totalRewards = ActiveLevel.TotalRewards;

        _inputManager.ToggleStimulusInput(true);
    }

    [ContextMenu("Destroy Test Level")]
    public void LeaveLevel()
    {
        _inputManager.ToggleStimulusInput(false);
        
        _builder.DestroyLevel(ActiveLevel);
        ActiveLevel = null;
    }

    [ContextMenu("Restart Test Level")]
    public void RestartLevel()
    {
        LeaveLevel();
        EnterLevel();
    }

    public void MovePlayers(PlayerMoveDirection direction)
    {
        if (ActiveLevel == null || _waitForMovementCoroutine != null)
        {
            return;
        }
        
        UpdateMoveScore();
        foreach (var player in ActiveLevel.Players)
        {
            player.Move(direction);
        }

        _waitForMovementCoroutine = StartCoroutine(WaitForPlayerMovement());
    }

    public void TriggerLevelWin()
    {
        _uiManager.ToggleGameWin(true);
        _inputManager.ToggleStimulusInput(true);
    }
    
    public void TriggerGameOver()
    {
        _uiManager.ToggleGameOver(true);
        _inputManager.ToggleStimulusInput(true);
    }

    public void UpdateRewardScore()
    {
        ++_rewardsCount;
        _uiManager.UpdateRewardScore(_rewardsCount);

        if (_rewardsCount >= _totalRewards)
        {
            TriggerLevelWin();
        }
    }
    
    public void UpdateMoveScore()
    {
        ++_movesCount;
        _uiManager.UpdateMovementScore(_movesCount);
    }

    private void Reset()
    {
        ActiveLevel = null;
        _movesCount = 0;
        _rewardsCount = 0;
    }

    private IEnumerator WaitForPlayerMovement()
    {
        _inputManager.ToggleStimulusInput(false);
        
        yield return new WaitForSecondsRealtime(0.2f);
        yield return new WaitUntil(()=> !ActiveLevel.PlayersMoving);
        
        _inputManager.ToggleStimulusInput(true);
        _waitForMovementCoroutine = null;
    }
}