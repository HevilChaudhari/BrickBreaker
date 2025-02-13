using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct PanelStruct
    {
        public GameState state;
        public GameObject panel;
    }

    //
    [Header("List Of Panels")]
    [SerializeField]private List<PanelStruct> panelStates;

    [Space]
    [Header("TextField Reference")]
    [SerializeField] private TextMeshProUGUI _scoreTxt;

    [Space]
    [Header("ControlChange UI Buttons Reference")]
    [SerializeField] private GameObject _touchControlBtn;
    [SerializeField] private GameObject _tiltControlBtn;
    [SerializeField] private GameObject _SoundBtn;
    
    //Store the panel with GameState as a key
    private Dictionary<GameState, GameObject> panelDictionary = new Dictionary<GameState, GameObject>();


    //::Region:- Monobehaviour Callbacks:://
    #region Monobehaviour Callbacks

    private void Awake()
    {
        foreach (var panelState in panelStates)
        {
            panelDictionary[panelState.state] = panelState.panel;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Gamemanager_OnGameStateChange;
        GameManager.Instance.OnBrickDestroy += GameManager_OnBrickDestroy;
        GameManager.Instance.OnControlChanged += GameManager_OnControlChanged;
        GameManager.Instance.OnSoundSwitchChanged += GameManager_OnSoundSwitchChanged;
    }



    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChange -= Gamemanager_OnGameStateChange;
        GameManager.Instance.OnControlChanged -= GameManager_OnControlChanged;
        GameManager.Instance.OnBrickDestroy -= GameManager_OnBrickDestroy;
    }

    private void Start()
    {
        UpdateControlUI();
        UpdateMusicUI();
    }

    #endregion
    //

    #region Private Methods

    private void GameManager_OnSoundSwitchChanged()
    {
        UpdateMusicUI();
    }

    private void GameManager_OnControlChanged()
    {
        UpdateControlUI();
    }

    private void GameManager_OnBrickDestroy()
    {
        UpdateScore();
    }

    private void Gamemanager_OnGameStateChange(GameState gameState)
    {
        ChangePanel(gameState);
        UpdateScore();
    }


    private void ChangePanel(GameState state)
    {
        // Disable all panels first
        foreach (var panels in panelDictionary.Values)
        {
            panels.SetActive(false);
        }

        
        if (panelDictionary.TryGetValue(state, out GameObject panel))
        {
            panel.SetActive(true);
        }

    }

    private void UpdateScore()
    {
        _scoreTxt.text = GameManager.Instance.GetCurrentScore().ToString();
    }

    private void UpdateControlUI()
    {

            _touchControlBtn.transform.GetChild(0).gameObject.SetActive((ControlMode)PlayerPrefs.GetInt(GameManager._CONTROL_MODE_STRING) == 0);
            _tiltControlBtn.transform.GetChild(0).gameObject.SetActive((ControlMode)PlayerPrefs.GetInt(GameManager._CONTROL_MODE_STRING) != 0);

    }

    private void UpdateMusicUI()
    {

        _SoundBtn.transform.GetChild(0).gameObject.SetActive(PlayerPrefs.GetInt(GameManager._SOUND_SWITCH_STRING) != 0);
    }

    #endregion

    #region Listener Methods

    public void OnPlayButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.LevelSelect);
    }

    public void OnResumeButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void OnResetButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Reset);
    }

    public void OnPauseButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void OnBackButtonPressed()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void OnSelectionButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.LevelSelect);
    }

    public void OnControlButtonChanged(int controlIndex)
    {
        GameManager.Instance.SetCurrentControlMode((ControlMode)controlIndex);
    }

    public void OnMusicButtonClicked()
    {
        if (PlayerPrefs.GetInt(GameManager._SOUND_SWITCH_STRING) == 0)
        {
            GameManager.Instance.SetSoundSwitch((SoundSwitch)1);
        }
        else
        {
            GameManager.Instance.SetSoundSwitch((SoundSwitch)0);
        }
    }

    #endregion

}