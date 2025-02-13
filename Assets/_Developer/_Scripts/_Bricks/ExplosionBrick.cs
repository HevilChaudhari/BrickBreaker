using UnityEngine;

public class ExplosionBrick : MonoBehaviour, IBreakable, IExplodable
{
    [Header("Explotion Factors")]
    [SerializeField] private int _explosionCount = 2;
    [SerializeField] private float _explosionRadius = 0.2f;

    [SerializeField] private LayerMask _brickLayer;

    [SerializeField] private GameObject _breakVisual;

    [SerializeField] private ParticleSystem _explosionParticle;

    private int _explosionCounter;
    private bool _hasExploded = false; // Prevents multiple explosions

    private void Start()
    {
        _explosionCounter = _explosionCount;
        GameManager.Instance.RegisterBrick();
    }

    #region Interface  Methods

    public void OnCollide()
    {
        Debug.Log("On Collide");

        if (_explosionCounter > 0)
        {
            _explosionCounter--;
        }
        else
        {
            Explode();
        }
    }

    public void OnBrickExplode()
    {
        Explode();
    }

    private void Explode()
    {
        if (_hasExploded) return; // Prevents repeated explosions
        _hasExploded = true;

        // Detect nearby bricks
        Collider2D[] brickColliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _brickLayer);

        foreach (Collider2D collider in brickColliders)
        {
            if (collider.TryGetComponent(out IExplodable explode) && collider.gameObject != gameObject)
            {
                explode.OnBrickExplode();
            }
        }

        GameManager.Instance.SetCurrentScore(GameManager.Instance.GetCurrentScore() + 200);
        GameManager.Instance.BrickDestroyed();
        _breakVisual.SetActive(true);
        _explosionParticle.Play();
        AudioManager.Instance.PlayBrickBreakSound();
        Invoke(nameof(DestroyGameObject), 0.1f);
    }


    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    #endregion
}
