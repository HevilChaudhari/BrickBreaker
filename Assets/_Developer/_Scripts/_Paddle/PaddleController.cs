using UnityEngine;
using UnityEngine.EventSystems;

public class PaddleController : MonoBehaviour
{
    private float _PADDLE_MOVING_LIMIT = (Screen.width / 2) - 10f;

    [Header("User Input Manager Reference")]
    [SerializeField] private UserInputManager _userInputManager;

    [Space]
    [Header("Paddle Movement Factors")]
    [SerializeField] private float _movSpeed = 10f;
    [SerializeField] private float _tiltSensitivity = 5f;

    [SerializeField] private Renderer _renderer;

    //Camera Access for touch control
    private Camera _camera;

    private bool _isPlaying = false;

    //Paddle Current Position
    private Vector3 _paddleCurrentPosition;

    //::Region:- Monobehavior Callbacks:://
    #region Monobehaviour Callbacks

    private void Awake()
    {
        _camera = Camera.main;
        _paddleCurrentPosition = transform.position;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChange -= GameManager_OnGameStateChange;
    }

    void Update()
    {
        if (_userInputManager.IsStartButtonTriggered() && GameManager.Instance.GetCurrentGameState() == GameState.PreGame) 
        {
            GameManager.Instance.UpdateGameState(GameState.Playing);
        }

        if (_isPlaying)
        {
            if (GameManager.Instance.GetCurrentControlMode() == ControlMode.Touch && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Touch control active");
                HandleTouchControl();
            }

            if(GameManager.Instance.GetCurrentControlMode() == ControlMode.Tilt)
            {
                Debug.Log("Tilt control active");
                HandleAccelarometerControl();
            }
        }

    }

    #endregion
    //

    //::Region:- Private Methods:://
    #region Private Methods

    //Triggers when GameState Change
    private void GameManager_OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.PreGame || gameState == GameState.Reset || gameState == GameState.LevelSelect)
        {
            transform.position = _paddleCurrentPosition;
        }

        _isPlaying = /*gameState == GameState.PreGame || */gameState == GameState.Playing;
    }

    //Handle A/D control
    private void HandleDefaultControl()
    {

        Vector2 controlInputValue = _userInputManager.GetControlInputValue();

        if (controlInputValue == Vector2.zero)
            return;

        Vector3 targetPosition = transform.position + new Vector3(controlInputValue.x * _movSpeed * Time.deltaTime, 0f, 0f);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -_PADDLE_MOVING_LIMIT, _PADDLE_MOVING_LIMIT);
        transform.position = targetPosition;
    }

    //Handle Touch control
    private void HandleTouchControl()
    {
        // Get touch input from user
        Vector2 touchInput = _userInputManager.GetTouchInputValue();

        // Convert screen position to world position
        Vector3 targetPosition = _camera.ScreenToWorldPoint(new Vector3(touchInput.x, touchInput.y, _camera.nearClipPlane));

        // Maintain paddle's y position
        targetPosition.y = transform.position.y;

        // Get screen bounds in world coordinates
        float screenHalfWidth = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float paddleHalfWidth = _renderer.bounds.extents.x;

        // Clamp x position to keep the paddle within screen bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, -screenHalfWidth + paddleHalfWidth, screenHalfWidth - paddleHalfWidth);

        // Apply the new position
        transform.position = targetPosition;
    }


    //Control Paddle using Accelerometer
    private void HandleAccelarometerControl()
    {
        
        float tilt = _userInputManager.GetAccelrometerInputValue();
        

        float moveAmount = tilt * _movSpeed * _tiltSensitivity * Time.deltaTime;
        Vector3 targetPosition = transform.position + new Vector3(moveAmount, 0f, 0f);

        // Get screen bounds in world coordinates
        float screenHalfWidth = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float paddleHalfWidth = _renderer.bounds.extents.x;

        // Clamp x position to keep the paddle within screen bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, -screenHalfWidth + paddleHalfWidth, screenHalfWidth - paddleHalfWidth);

        
        transform.position = targetPosition;
    }

    #endregion
    //
}



