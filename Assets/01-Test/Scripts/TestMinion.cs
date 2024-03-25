using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Test_Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TestMinion : MonoBehaviour
{

    public GameObject[] projectiles;
    public GameObject targetEnemy;
    public GameObject targetHitPoint;
    public GameObject projectileSpawnPoint;
    public int tier;
    public bool homingProjectile;
    public bool isMage;
    public bool isArcher;
    public bool isSwordsman;
    public bool randomProjectile;
    public int projectileCount;
    public float angleBetweenProjectiles;
    public float projectileTime;
    public float projectileSpeed;
    public float projectileTurnTime;
    public float emittingSpeed;
    private float emittingSpeedOld;
    public Animator animator;

    private List<TestProjectile> _testProjectiles = new List<TestProjectile>();
    

    private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
    private static readonly int IsShooting = Animator.StringToHash("IsShooting");

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool(IsShooting, true);
    }
    
    [Button]
    public void PauseEmitter()
    {
        animator.SetBool(IsShooting, false);
    }

    [Button]
    public void ResumeEmitter()
    {
        animator.SetBool(IsShooting, true);
    }
    
    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.LookAt(targetEnemy.transform);
        }
        
        if (emittingSpeedOld != emittingSpeed)
        {
            animator.SetFloat(AnimationSpeed, emittingSpeed);
            emittingSpeedOld = emittingSpeed;
        }
    }

    public void Shoot()
    {
        if(isMage)
            ShootStraight();
        else if (isArcher)
        {
            for (int i = 0; i < _testProjectiles.Count; i++)
            {
                _testProjectiles[i].isShot = true;
            }
        
            _testProjectiles.Clear();
        }
    }

    public void Slash()
    {
        var projectilePrefab = projectiles[tier - 1];
        if (projectilePrefab == null) return;

        var projectile = Instantiate(projectilePrefab, transform);
        var scriptt = projectile.GetComponent<TestProjectile>();
        if (scriptt == null)
        {
            Debug.LogWarning("Projectile has no TestProjectile script!");
            Destroy(projectile);
        }
        else
        {
            scriptt.lifetime = projectileTime;
            scriptt.speed = 0;
        }
    }

    public void PrepareArrows()
    {
        ShootStraight();
    }
    
    [Button]
    public void ShootStraight()
    {
        var projectilePrefab = projectiles[tier - 1];
        if (projectilePrefab == null) return;


        var totalAngle = Mathf.PI * 2;
        var middlePoint = angleBetweenProjectiles * (projectileCount - 1);
        var halfMiddle = middlePoint / 2;
        var startingAngle = -halfMiddle;

        for (int i = 0; i < projectileCount; i++)
        {
            var angleToAdd = startingAngle + i * angleBetweenProjectiles;
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.transform.position = projectileSpawnPoint.transform.position;
            //projectile.transform.Rotate(new Vector3(0, 1, 0), angleToAdd);
            var scriptt = projectile.GetComponent<TestProjectile>();

            if (scriptt == null)
            {
                Debug.LogWarning("Projectile has no TestProjectile script!");
                Destroy(projectile);
            }
            else
            {
                scriptt.SetRotation(angleToAdd);
                scriptt.lifetime = projectileTime;
                scriptt.speed = projectileSpeed;
                scriptt.isHoming = homingProjectile;
                //scriptt.targetPoint = targetHitPoint;
                scriptt.SetTargetPoint(targetHitPoint);
                scriptt.turnTime = projectileTurnTime;
                
                if (isArcher)
                    _testProjectiles.Add(scriptt);
                else
                    scriptt.isShot = true;
            }

           
        }
    }
}
