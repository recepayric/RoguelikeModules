using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Test_Runtime
{
    public class ProjectileEmitter : MonoBehaviour
    {
        public EmitType emitType;
        public GameObject projectilePrefab;
        public int projectileCount;
        public float projectileSpeed;
        public float projectileTime;
        public float emitTime;
        public float emitTimer;
        public bool isEmitting = true;

        [ShowIf("emitType", EmitType.Circle)] public float projectileMinDistance;

        [Button]
        public void PauseEmitter()
        {
            isEmitting = false;
        }

        [Button]
        public void ResumeEmitter()
        {
            isEmitting = true;
        }


        private void Update()
        {
            emitTimer -= isEmitting ? Time.deltaTime : 0;

            if (emitTimer <= 0)
            {
                emitTimer = emitTime;
                if (emitType == EmitType.Straight)
                    ShootStraight();
                else if (emitType == EmitType.Circle)
                    ShootCircle();
            }
        }

        [Button]
        public void ShootStraight()
        {
            if (projectilePrefab == null) return;


            var totalAngle = Mathf.PI * 2;
            var angleBetweenProjectiles = 10f;
            var middlePoint = angleBetweenProjectiles * (projectileCount - 1);
            var halfMiddle = middlePoint / 2;
            var startingAngle = -halfMiddle;

            for (int i = 0; i < projectileCount; i++)
            {
                var angleToAdd = startingAngle + i * angleBetweenProjectiles;
                var projectile = Instantiate(projectilePrefab, transform);
                projectile.transform.position = transform.position;
                projectile.transform.Rotate(new Vector3(0, 0, 1), angleToAdd);
                var scriptt = projectile.GetComponent<TestProjectile>();

                if (scriptt == null)
                {
                    Debug.LogWarning("Projectile has no TestProjectile script!");
                    Destroy(projectile);
                }
                else
                {
                    scriptt.lifetime = projectileTime;
                    scriptt.speed = projectileSpeed;
                }
            }
        }

        public void ShootCircle()
        {
            var angleBetween = Mathf.PI * 2 / projectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                var angle = angleBetween * i;
                var projectile = Instantiate(projectilePrefab, transform);

                var x = projectileMinDistance * Mathf.Cos(angle);
                var y = projectileMinDistance * Mathf.Sin(angle);

                var pos = transform.position + new Vector3(x, y);
                var posNormalised = pos.normalized;
                projectile.transform.position = pos;
                projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg));
                //projectile.transform.right = posNormalised - projectile.transform.position;

                var scriptt = projectile.GetComponent<TestProjectile>();

                if (scriptt == null)
                {
                    Debug.LogWarning("Projectile has no TestProjectile script!");
                    Destroy(projectile);
                }
                else
                {
                    scriptt.lifetime = projectileTime;
                    scriptt.speed = projectileSpeed;
                }
            }
        }
    }

    public enum EmitType
    {
        Straight,
        Circle,
    }
}