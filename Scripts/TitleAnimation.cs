using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float pulseSpeed = 1f;
    public float scaleAmount = 0.1f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Sin(Time.time * pulseSpeed) * scaleAmount + 1f;
        transform.localScale = originalScale * scale;
    }
}
