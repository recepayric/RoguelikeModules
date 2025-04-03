using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Utility
{
    public class ArvaveUtility
    {
        public static float GetAnimationSpeed(AnimationClip clip, float wantedTime)
        {
            return clip.length / wantedTime;
        }

        public static float CalculateIncreaseMultiplier(float increase)
        {
            var multiplier = 1 + (increase / 100f);
            
            if (multiplier <= 0)
                multiplier = 0.01f;

            return multiplier;
        }

        public static void SwitchAnimation(Animator animator, string animationName, float timeToSwitch, int layerIndex = 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            float currentLength = stateInfo.length;
            animator.CrossFade(animationName, timeToSwitch/currentLength);
        }

        public static void SetParticleEmission(List<ParticleSystem> particleSystems, bool status)
        {
            foreach (var particle in particleSystems)
            {
                var emission = particle.emission;
                emission.enabled = status;

                if (status)
                    particle.Play();
                else
                    particle.Stop();
            }
        }
        
        public static bool CalculateThrowAngles(
            float initialHeight,
            float targetHeight,
            float horizontalDistance,
            float initialSpeed,
            out float angleDegLow,
            out float angleDegHigh
        )
        {
            angleDegLow = angleDegHigh = 0f;

            float deltaY = targetHeight - initialHeight;
            float d = horizontalDistance;
            float v = initialSpeed;
            float g = Physics.gravity.magnitude; // Unity's gravity (positive)

            // Quadratic coefficients: a*u² + b*u + c = 0
            float a = (g * d * d) / (2 * v * v);
            float b = -d;
            float c = deltaY + a;

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                // No valid angle (velocity too low or distance too far)
                return false;
            }

            float sqrtD = Mathf.Sqrt(discriminant);
            float u1 = (-b + sqrtD) / (2 * a); // High arc
            float u2 = (-b - sqrtD) / (2 * a); // Low arc

            angleDegHigh = Mathf.Atan(u1) * Mathf.Rad2Deg;
            angleDegLow = Mathf.Atan(u2) * Mathf.Rad2Deg;

            return true;
        }
    }
}