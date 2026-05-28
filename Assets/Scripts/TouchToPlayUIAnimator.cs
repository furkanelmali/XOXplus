using UnityEngine;

/// <summary>
/// Hafif "nefes alan" scale pulse — legacy TouchToPlay.anim yerine.
/// </summary>
[DisallowMultipleComponent]
public class TouchToPlayUIAnimator : MonoBehaviour
{
    [SerializeField] private float pulseScale = 1.06f;
    [SerializeField] private float pulseSpeed = 1.8f;

    private RectTransform rectTransform;
    private Vector3 baseScale;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        baseScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        if (!rectTransform) rectTransform = transform as RectTransform;
        baseScale = rectTransform.localScale;
    }

    private void Update()
    {
        if (!rectTransform) return;

        // 0..1 arası yumuşak salınım (alloc yok)
        var wave = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;
        var mul = Mathf.Lerp(1f, pulseScale, wave);
        rectTransform.localScale = baseScale * mul;
    }

    private void OnDisable()
    {
        if (rectTransform) rectTransform.localScale = baseScale;
    }
}
