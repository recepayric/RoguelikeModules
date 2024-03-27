using Data;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class PlayerCreator : MonoBehaviour
    {
        public CharacterDataSo characterDataSo;
        private Player _playerScript;
        public Health health;
        public PlayerMovement3D playerMovement;
        public SpecialModifierHelper specialModifierHelper;
        public PlayerLevel playerLevel;

        [Button]
        public void Create()
        {
            AddPlayerBaseScript();
            AddHealth();
            AddMovement();
            AddSpecialModifierHelper();
            AddPlayerLevel();
            
            Initialise();
        }

        private void Initialise()
        {
            _playerScript.characterDataSo = characterDataSo;
            _playerScript.InitialiseVariables();
        }
        
        
        private void AddPlayerBaseScript()
        {
            var oldPlayerScript = GetComponent<Player>();
            if (oldPlayerScript != null)
                DestroyImmediate(oldPlayerScript);

            transform.AddComponent<Player>();
            _playerScript = GetComponent<Player>();
        }
        
        private void AddPlayerLevel()
        {
            var oldPlayerLevel = GetComponent<PlayerLevel>();
            if (oldPlayerLevel != null)
                DestroyImmediate(oldPlayerLevel);

            transform.AddComponent<PlayerLevel>();
            playerLevel = GetComponent<PlayerLevel>();
        }
        
        private void AddSpecialModifierHelper()
        {
            var oldSpecialModifierHelper = GetComponent<SpecialModifierHelper>();
            if (oldSpecialModifierHelper != null)
                DestroyImmediate(oldSpecialModifierHelper);

            transform.AddComponent<SpecialModifierHelper>();
            specialModifierHelper = GetComponent<SpecialModifierHelper>();
        }
        

        private void AddHealth()
        {
            var oldHealthComponent = GetComponent<Health>();
            if (oldHealthComponent != null)
                DestroyImmediate(oldHealthComponent);

            transform.AddComponent<Health>();
            health = GetComponent<Health>();
        }
        
        private void AddMovement()
        {
            var oldMovement = GetComponent<PlayerMovement3D>();
            if (oldMovement != null)
                DestroyImmediate(oldMovement);

            transform.AddComponent<PlayerMovement3D>();
            playerMovement = GetComponent<PlayerMovement3D>();
        }

    }
}