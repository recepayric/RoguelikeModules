using UnityEngine;

namespace Runtime.AnimationHelpers
{
    public class EnemyAnimationHelper : MonoBehaviour
    {

        public Enemy enemy;
        
        public void CreateProjectile()
        {
            enemy.CreateProjectile();
        }

        public void TriggerAttack()
        {
            enemy.TriggerAttack();
        }
    }
}