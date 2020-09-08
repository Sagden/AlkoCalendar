using UnityEngine;
using DG.Tweening;

public interface IAnimatedPanel
{
    void Show();
    void Hide();
    void Toggle();

    bool visible { get; set; }
}

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedPanel : MonoBehaviour, IAnimatedPanel
{

    private CanvasGroup m_canvasGroup;
    private CanvasGroup canvasGroup { get { if (m_canvasGroup == null) m_canvasGroup = GetComponent<CanvasGroup>(); return m_canvasGroup; } }

    public float animationTime = 0.15f;
    public float scale = 1;
    [Range(0, 1)]
    public float scaleMP = 0.8f;

    public bool visible
    {
        get { return gameObject.activeSelf; }
        set { if (visible != value) Toggle(); }
    }

    private void Reset()
    {
        scale = transform.localScale.x;
    }

    public void Show()
    {
        CompleteTweens();

        if (gameObject.activeSelf) return;

        gameObject.SetActive(true);

        canvasGroup.alpha = 0;
        transform.localScale = Vector3.one * scale * scaleMP;

        canvasGroup.DOFade(1, animationTime);
        transform.DOScale(scale, animationTime);
    }

    public void Hide()
    {
        CompleteTweens();

        if (!gameObject.activeSelf) return;

        canvasGroup.DOFade(0, animationTime).OnComplete(() => gameObject.SetActive(false));
        transform.DOScale(scale * scaleMP, animationTime);
    }

    public void Toggle()
    {
        CompleteTweens();

        if (visible) Hide(); else Show();
    }

    private void CompleteTweens()
    {
        DOTween.Kill(canvasGroup, true);
        DOTween.Kill(transform, true);
    }
}
