using UnityEngine;

namespace Runtime.Minions
{
    public class MinionEventMiddleman : MonoBehaviour
    {

        public Minion minion;

        public void ActivateWeapon()
        {
            minion.ActivateWeapon();
        }

        public void DeactivateWeapon()
        {
            minion.DeactivateWeapon();
            HitAnimationEnded();
        }

        public void HitAnimationStarted()
        {
            minion.isHitAnimationEnded = false;
        }
   
        
        public void HitAnimationEnded()
        {
            minion.isHitAnimationEnded = true;
        }

        
    
    }
}
