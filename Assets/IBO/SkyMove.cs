using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SkyMove : MonoBehaviour
{
    private float _duration = 600f;
    public GameObject MoonRef;
    
    
    [Header("Skybox Exposure Settings")]
    public float baseExposure = 0.35f;
    public float initialGlowExposure = 0.42f; // Ön aydınlanma (hafif ve uzun süren)
    public float smallFluctuationMin = 0.45f;
    public float smallFluctuationMax = 0.55f;
    public float maxFlashExposure = 0.75f; // Ana yıldırım parlaması
    public float finalDecayExposure = 0.38f;

    [Header("Directional Light Settings")]
    public Light directionalLight;
    public float baseIntensity = 0.0f;
    public float initialGlowIntensity = 0.4f;
    public float smallFluctuationIntensityMin = 0.3f;
    public float smallFluctuationIntensityMax = 1.0f;
    public float maxFlashIntensity = 1.5f;
    public float finalDecayIntensity = 0.3f;

    [Header("Lightning Timing Settings")]
    public float minInterval = 5f; // Min süre
    public float maxInterval = 12f; // Max süre
    public int minFluctuations = 5; // Küçük dalgalanma sayısı (yavaşlaması için artırıldı)
    public int maxFluctuations = 10;
    
    [Header("Wind For Fire Settings")]

    public ParticleSystem[] fireParticles; // Birden fazla Particle System desteği
    public int orbitalXIndex = 0; // Orbital X kullanan partikülün index'i (ilk partikül için 0)
    
    [Header("Orbital X Settings")]
    public float minOrbitalX = -2f;
    public float maxOrbitalX = 0f;

    [Header("Linear X Settings")]
    public float minLinearX = -0.25f;
    public float maxLinearX = 0f;

    [Header("Timing Settings")]
    public float transitionTimeMin = 1.5f;
    public float transitionTimeMax = 4f;
    public float holdTime = 1f; // Bekleme süresi (0'a döndükten sonra)
    
    
    void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 360);
        StartCoroutine(RotateSkybox());
        StartCoroutine(LightningRoutine());
        MoveMoon();
        
        // for (int i = 0; i < fireParticles.Length; i++)
        // {
        //     if (i == orbitalXIndex)
        //         StartCoroutine(WindEffectRoutine_OrbitalX(fireParticles[i]));
        //     else
        //         StartCoroutine(WindEffectRoutine_LinearX(fireParticles[i]));
        // }
    }
    
    // **Orbital X Kullanan Particle System için**
    IEnumerator WindEffectRoutine_OrbitalX(ParticleSystem particleSystem)
    {
        var velocityModule = particleSystem.velocityOverLifetime;

        while (true)
        {
            float targetValue = Random.Range(minOrbitalX, maxOrbitalX);
            float transitionDuration = Random.Range(transitionTimeMin, transitionTimeMax);

            float currentValue = velocityModule.orbitalX.constant;

            // **Önce rastgele bir değere yumuşak geçiş**
            DOTween.To(() => currentValue,
                    x => velocityModule.orbitalX = new ParticleSystem.MinMaxCurve(x),
                    targetValue,
                    transitionDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    // **Hedefe ulaştığında, 2 kat hızlı bir şekilde 0'a geri dönüş**
                    DOTween.To(() => targetValue,
                            x => velocityModule.orbitalX = new ParticleSystem.MinMaxCurve(x),
                            0f,
                            transitionDuration * 0.5f) // **Geri dönüş süresi 2 kat hızlı**
                        .SetEase(Ease.InOutSine);
                });

            yield return new WaitForSeconds(transitionDuration + (transitionDuration * 0.5f)); // **İlk geçiş + dönüş süresi**
            yield return new WaitForSeconds(holdTime); // **0'da bekleme süresi**
        }
    }

    
    // **Linear X Kullanan Particle System için**
    IEnumerator WindEffectRoutine_LinearX(ParticleSystem particleSystem)
    {
        var velocityModule = particleSystem.velocityOverLifetime;

        while (true)
        {
            float targetValue = Random.Range(minLinearX, maxLinearX);
            float transitionDuration = Random.Range(transitionTimeMin, transitionTimeMax);

            float currentValue = velocityModule.y.constant;

            // **Önce rastgele bir değere yumuşak geçiş**
            DOTween.To(() => currentValue,
                    x => velocityModule.y = new ParticleSystem.MinMaxCurve(x),
                    targetValue,
                    transitionDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    // **Hedefe ulaştığında, 2 kat hızlı bir şekilde 0'a geri dönüş**
                    DOTween.To(() => targetValue,
                            x => velocityModule.y = new ParticleSystem.MinMaxCurve(x),
                            0f,
                            transitionDuration * 0.5f) // **Geri dönüş süresi 2 kat hızlı**
                        .SetEase(Ease.InOutSine);
                });

            yield return new WaitForSeconds(transitionDuration + (transitionDuration * 0.5f)); // **İlk geçiş + dönüş süresi**
            yield return new WaitForSeconds(holdTime); // **0'da bekleme süresi**
        }
    }
    IEnumerator RotateSkybox()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < _duration)
            {
                float rotationValue = Mathf.Lerp(360, 0, elapsedTime / _duration);
                RenderSettings.skybox.SetFloat("_Rotation", rotationValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            RenderSettings.skybox.SetFloat("_Rotation", 360);
        }
    }

    IEnumerator LightningRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            int fluctuationCount = Random.Range(minFluctuations, maxFluctuations);

            // **1. Atmosferde Hafif Bir Aydınlanma (Ön Parıltı)**
            RenderSettings.skybox.DOFloat(initialGlowExposure, "_Exposure", 0.25f) // **Ön aydınlanma artık daha yavaş**
                .SetEase(Ease.InOutQuad);
            directionalLight.DOIntensity(initialGlowIntensity, 0.25f)
                .SetEase(Ease.InOutQuad);

            yield return new WaitForSeconds(0.3f); // Ön parıltıyı daha uzun tuttuk

            // **2. Küçük Dalgalanmalar (5-10 kere rastgele ışık değişimi)**
            for (int i = 0; i < fluctuationCount; i++)
            {
                float fluctuationDuration = Random.Range(0.1f, 0.2f); // **Daha uzun dalgalanmalar**
                float randomExposure = Random.Range(smallFluctuationMin, smallFluctuationMax);
                float randomIntensity = Random.Range(smallFluctuationIntensityMin, smallFluctuationIntensityMax);

                RenderSettings.skybox.DOFloat(randomExposure, "_Exposure", fluctuationDuration)
                    .SetEase(Ease.InOutSine);
                directionalLight.DOIntensity(randomIntensity, fluctuationDuration)
                    .SetEase(Ease.InOutSine);

                yield return new WaitForSeconds(fluctuationDuration + Random.Range(0.05f, 0.1f)); // **Rastgeleleşmiş aralık**
            }

            // **3. ASIL ANA PATLAMA (Yıldırımın en parlak anı)**
            RenderSettings.skybox.DOFloat(maxFlashExposure, "_Exposure", 0.1f)
                .SetEase(Ease.OutExpo);
            directionalLight.DOIntensity(maxFlashIntensity, 0.1f)
                .SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(0.1f);

            // **4. Hafif sönüş (Kademeli fading ve yansımalar)**
            RenderSettings.skybox.DOFloat(smallFluctuationMax, "_Exposure", 0.3f) // **Daha uzun fading**
                .SetEase(Ease.InOutSine);
            directionalLight.DOIntensity(smallFluctuationIntensityMax, 0.3f)
                .SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(0.3f);

            RenderSettings.skybox.DOFloat(smallFluctuationMin, "_Exposure", 0.25f)
                .SetEase(Ease.InOutSine);
            directionalLight.DOIntensity(smallFluctuationIntensityMin, 0.25f)
                .SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(0.25f);

            // **5. Son fading (ışığın atmosferde kaybolması)**
            float finalFadeTime = Random.Range(0.25f, 0.35f); // **Daha yavaş fading süresi**
            RenderSettings.skybox.DOFloat(finalDecayExposure, "_Exposure", finalFadeTime)
                .SetEase(Ease.OutQuad);
            directionalLight.DOIntensity(finalDecayIntensity, finalFadeTime)
                .SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(finalFadeTime);

            // **6. Eski haline dönüş (Tam sönüş)**
            RenderSettings.skybox.DOFloat(baseExposure, "_Exposure", 0.2f) // **Daha uzun geçiş**
                .SetEase(Ease.InOutQuad);
            directionalLight.DOIntensity(baseIntensity, 0.2f)
                .SetEase(Ease.InOutQuad);
        }
    }
    private void MoveMoon()
    {
        MoonRef.transform.DOMoveX(MoonRef.transform.position.x - 30f, _duration/2f).SetEase(Ease.Linear);
    }
    
}
