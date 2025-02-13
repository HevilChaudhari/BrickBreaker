using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton Implementation

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(nameof(GameManager));
                    _instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    #endregion

    public const string _CONTROL_MODE_STRING = "ControlMode";
    public const string _SOUND_SWITCH_STRING = "SoundSwitch";

    public event Action<GameState> OnGameStateChange;
    public event Action OnBrickDestroy;
    public event Action OnControlChanged;
    public event Action OnSoundSwitchChanged;


    [SerializeField] private GameState _currentGameState;
    [SerializeField] private ControlMode _currentControlMode;
    [SerializeField] private SoundSwitch _currentSoundSwitch;

    private int _currentGameLevelIndex;
    private int _brickCount;
    private int _score;

    //::Region:-Monobehaviour Callbacks:://
    #region Monobehaviour Callbacks

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        Application.targetFrameRate = 60;
        _currentControlMode = (ControlMode)PlayerPrefs.GetInt(_CONTROL_MODE_STRING, 0);
        _currentSoundSwitch = (SoundSwitch)PlayerPrefs.GetInt(_SOUND_SWITCH_STRING, 0);
    }

    private void Start()
    {
        _currentGameState = GameState.MainMenu;
        UpdateGameState(_currentGameState);

        _brickCount = 0;
        _score = 0;

    }

    #endregion
    //

    //::Region:-Public Methods:://
    #region Public Methods

    public void UpdateGameState(GameState newGameState)
    {
        Debug.Log("GameState Changed: " + newGameState);

        if (newGameState == GameState.Reset)
            ResetBrickCount();

        if (newGameState == GameState.PreGame)
            SetCurrentScore(0);

        _currentGameState = newGameState;
        OnGameStateChange?.Invoke(_currentGameState);
    }

    public void RegisterBrick()
    {
        _brickCount++;
    }

    public void BrickDestroyed()
    {
        _brickCount--;
        if(_brickCount <= 0)
            UpdateGameState(GameState.GameWin);

        OnBrickDestroy?.Invoke();
    }

    public GameState GetCurrentGameState() { return _currentGameState; }

    public ControlMode GetCurrentControlMode() { return _currentControlMode; }

    public SoundSwitch GetSoundSwitch() { return _currentSoundSwitch; }

    public int GetCurrentLevelIndex() { return _currentGameLevelIndex; }

    public int GetCurrentScore() { return _score; }

    public void SetCurrentScore(int score) { _score = score; }

    public void SetCurrentLevelIndex(int index) { _currentGameLevelIndex = index; }

    public void SetCurrentControlMode(ControlMode newControlMode) 
    {
        Debug.Log("<color = Green> Set current control mode</color>: " + newControlMode);
        _currentControlMode = newControlMode;
        PlayerPrefs.SetInt(_CONTROL_MODE_STRING, (int)_currentControlMode);
        OnControlChanged?.Invoke();
    }

    public void SetSoundSwitch(SoundSwitch soundSwitch)
    {
        _currentSoundSwitch = soundSwitch;
        PlayerPrefs.SetInt(_SOUND_SWITCH_STRING, (int)_currentSoundSwitch);
        OnSoundSwitchChanged?.Invoke();

     }

    #endregion
    //

    private void ResetBrickCount()
    {
        _brickCount = 0;
    }

}


public enum GameState
{
    None,
    MainMenu,
    LevelSelect,
    PreGame,
    Playing,
    Reset,
    Paused,
    GameWin,
    GameLose,
}

public enum ControlMode
{
    Touch = 0,
    Tilt = 1,
}

public enum SoundSwitch
{
    On,
    Off
}
