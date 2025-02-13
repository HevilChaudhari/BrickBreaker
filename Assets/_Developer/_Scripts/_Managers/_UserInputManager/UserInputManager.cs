using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private float _accelerationSpeed = 10f;
    private UserInputAction _userInputAction;
    private bool _isGameStarted = false;

    #region Monobehaviour Callbacks

    private void Awake()
    {
        _userInputAction = new();
        _userInputAction.Paddle.Enable();
        _userInputAction.Paddle.TiltControl.Enable();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
        _userInputAction.Paddle.GameStart.canceled += GameStart_Oncanceled;


    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChange -= GameManager_OnGameStateChange;
        _userInputAction.Paddle.GameStart.canceled -= GameStart_Oncanceled;
    }

    
    #endregion

    #region Private Methods

    private void GameStart_Oncanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isGameStarted = true;
    }

    private void GameManager_OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.PreGame || gameState == GameState.Playing)
        {
            _isGameStarted = false;
        }
    }

    #endregion

    public Vector2 GetControlInputValue()
    {
        return _userInputAction.Paddle.DefaultControl.ReadValue<Vector2>();
    }

    public float GetAccelrometerInputValue()
    {
        return Input.acceleration.x;
    }

    public Vector2 GetTouchInputValue()
    {
        return _userInputAction.Paddle.TouchControl.ReadValue<Vector2>();
    } 

    public bool IsStartButtonTriggered()
    {
        return _isGameStarted;
    }

 
}
