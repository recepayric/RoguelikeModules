using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated
{
    public class Spell : MonoBehaviour, ICastable, IPoolObject
    {
        protected Owners Owner;
        public bool FollowsOwner { get; set; }
        public ISpellCaster OwnerScript { get; set; }

        public virtual void Cast()
        {
            
        }

        public virtual void Activate()
        {
            
        }

        public virtual void DeActivate()
        {
            
        }

        public virtual void Prepare()
        {
            
        }

        public virtual void StartSpell(){
            
        }
        public virtual void StopSpell()
        {
            
        }

        public virtual void SetPosition(Vector3 targetPosition)
        {
            
        }

        public virtual void SetOwner(Owners owner)
        {
            Owner = owner;
        }

        public virtual void SetOwnerScript(ISpellCaster spellCaster)
        {
            OwnerScript = spellCaster;
        }

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
            
        }

        public void OnGet()
        {
        }
    }
}