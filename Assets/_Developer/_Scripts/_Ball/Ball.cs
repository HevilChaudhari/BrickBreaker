using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float _BALL_OFFSET_FROM_PADDLE = 0.35f;
    private const float _RAY_DISTANCE = 0.2f;

    [Header("Paddle Transform Reference")]
    [SerializeField] Transform _paddleTransform;

    [Space]
    [Header("Ball Movement Factors")]
    [SerializeField] private float _movSpeed = 8f;
    [SerializeField] private float _maxBounceAngle = 75f;

    [Space]
    [Header("Define Bounce Layer")]
    [SerializeField] private LayerMask _bounceLayer;

    private Vector2 direction; // refernce for direction


    //::Enum:- Ball can Bounce with this type of Collisionypes:://
    private enum CollisionType 
    {   None, 
        Brick, 
        Paddle, 
        Wall, 
        DeathZone 
    }

    //::Region:- Monobehaviour Callbacks:://
    #region Monobehaviour Callbacks

    //===Start Method===//
    void Start()
    {
        direction = Vector2.up;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
    }




    //===Update Method===//
    void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() == GameState.PreGame || GameManager.Instance.GetCurrentGameState() == GameState.LevelSelect)
        {
            transform.position = new Vector3(_paddleTransform.position.x, _paddleTransform.position.y + _BALL_OFFSET_FROM_PADDLE, 0);

        }
        else if(GameManager.Instance.GetCurrentGameState() == GameState.Playing)
        {
            MoveBall();
        }
    }
    #endregion
    //

    //::Region:- Private Methods:://
    #region Private Methods

    //Handle Ball Movement
    private void MoveBall()
    {
        
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, direction, _RAY_DISTANCE, _bounceLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + (Vector3.left * 0.1f), direction, _RAY_DISTANCE, _bounceLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + (Vector3.right * 0.1f), direction, _RAY_DISTANCE, _bounceLayer);

        
        RaycastHit2D hit = upHit.collider ? upHit : (leftHit.collider ? leftHit : rightHit);

        if (hit.collider != null)
        {
            CollisionType collisionType = GetCollisionType(hit.collider);
            AudioManager.Instance.PlayBallSound();
            switch (collisionType)
            {
                case CollisionType.Brick:
                    HandleBrickCollision(hit);
                    break;
                case CollisionType.Paddle:
                    HandlePaddleCollision(hit);
                    break;
                case CollisionType.Wall:
                    HandleWallCollision(hit);
                    break;
                case CollisionType.DeathZone:
                    HandleDeathZoneCollision();
                    return;
            }
        }

        
        // Move the ball
        transform.position += (Vector3)(direction.normalized * _movSpeed * Time.deltaTime);
    }

    //Return Collisiontype enum base on Gameobject tags
    private CollisionType GetCollisionType(Collider2D collider)
    {
        if (collider.CompareTag("SimpleBrick") || collider.CompareTag("MultihitBrick") || collider.CompareTag("ExplosiveBrick")) return CollisionType.Brick;
        if (collider.CompareTag("Paddle")) return CollisionType.Paddle;
        if (collider.CompareTag("Wall")) return CollisionType.Wall;
        if (collider.CompareTag("DeathZone")) return CollisionType.DeathZone;

        return CollisionType.None;
    }

    //Handles Logics when collide with brick
    private void HandleBrickCollision(RaycastHit2D hit)
    {
        //Debug.Log("Brick Collided");
        if (hit.collider.TryGetComponent(out IBreakable breakable))
            breakable.OnCollide();
        direction = Vector2.Reflect(direction, hit.normal);
    }

    //Handles Logics when collide with Paddle
    private void HandlePaddleCollision(RaycastHit2D hit)
    {
        //Debug.Log("Paddle Collided");
        float hitPoint = (transform.position.x - hit.collider.transform.position.x) / hit.collider.bounds.size.x;
        float angle = hitPoint * _maxBounceAngle;
        direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    //Handles Logics when collide with Wall
    private void HandleWallCollision(RaycastHit2D hit)
    {
        //Debug.Log("Wall Collided");
        direction = Vector2.Reflect(direction, hit.normal);
    }

    //Handles Logics when collide with Deathzone
    private void HandleDeathZoneCollision()
    {
        Debug.Log("DeathZone Collided");

        if(GameManager.Instance.GetCurrentGameState() == GameState.Playing)
                GameManager.Instance.UpdateGameState(GameState.GameLose);
    }

    //Trigger When GameState changes
    private void GameManager_OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.LevelSelect)
        {
            transform.position = new Vector3(_paddleTransform.position.x, _paddleTransform.position.y + _BALL_OFFSET_FROM_PADDLE, 0);
        }
    }
    #endregion
    //


}
