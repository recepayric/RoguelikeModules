using UnityEngine;
using DG.Tweening;

public class LightFlicker : MonoBehaviour
{
    public Light lightToFlicker;
    public float minIntensity = 1f;
    public float maxIntensity = 4f;
    public float flickerDurationMin = 0.2f;
    public float flickerDurationMax = 0.5f;
    
    void Start()
    {
        StartFlickering();
    }

    void StartFlickering()
    {
        Sequence flickerSequence = DOTween.Sequence();
        flickerSequence.Append(lightToFlicker.DOIntensity(maxIntensity, Random.Range(flickerDurationMin,flickerDurationMax)))
            .Append(lightToFlicker.DOIntensity(minIntensity, Random.Range(flickerDurationMin,flickerDurationMax)))
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
}