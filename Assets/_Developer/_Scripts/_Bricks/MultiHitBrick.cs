using UnityEngine;

public class MultiHitBrick : MonoBehaviour , IBreakable , IExplodable
{
    [SerializeField] private int _hitPoints = 2;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _breakVisual;

    private int _hit = 0;

    private void Start()
    {
        _hit = _hitPoints;
        GameManager.Instance.RegisterBrick();
    }

    #region Interface  Methods

    public void OnCollide()
    {
        if(_hitPoints > 0)
        {
            _hitPoints--;
            _spriteRenderer.sprite = _sprites[_hitPoints];
            
        }
        else
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.GetCurrentScore() + 150);
            GameManager.Instance.BrickDestroyed();
            _breakVisual.SetActive(true);
            AudioManager.Instance.PlayBrickBreakSound();
            Invoke(nameof(DestroyGameObject), 0.1f);
        }
    }

    public void OnBrickExplode()
    {
        if (_hitPoints > 0)
        {
            _hitPoints--;
            _spriteRenderer.sprite = _sprites[_hitPoints];

        }
        else
        {
            GameManager.Instance.SetCurrentScore(GameManager.Instance.GetCurrentScore() + 150);
            GameManager.Instance.BrickDestroyed();
            _breakVisual.SetActive(true);
            AudioManager.Instance.PlayBrickBreakSound();
            Invoke(nameof(DestroyGameObject), 0.1f);
        }
    }

    private void DestroyGameObject()
    {
            Destroy(gameObject);
    }

    #endregion
}
