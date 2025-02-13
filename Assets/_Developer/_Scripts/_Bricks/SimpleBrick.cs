using UnityEngine;

public class SimpleBrick : MonoBehaviour , IBreakable , IExplodable
{
    [SerializeField] private GameObject _breakVisual;

    private void Start()
    {
        GameManager.Instance.RegisterBrick();
    }

    #region Interface  Methods

    public void OnCollide()
    {
        GameManager.Instance.SetCurrentScore(GameManager.Instance.GetCurrentScore() + 100);
        GameManager.Instance.BrickDestroyed();
        _breakVisual.SetActive(true);
        AudioManager.Instance.PlayBrickBreakSound();
        Invoke(nameof(DestroyGameObject), 0.1f);
    }

    public void OnBrickExplode()
    {
        GameManager.Instance.SetCurrentScore(GameManager.Instance.GetCurrentScore() + 100);
        GameManager.Instance.BrickDestroyed();
        _breakVisual.SetActive(true);
        AudioManager.Instance.PlayBrickBreakSound();
        Invoke(nameof(DestroyGameObject), 0.1f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    #endregion
}
