using CodeStage.AntiCheat.ObscuredTypes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "ArvaveGames/Stat/Stat Conversion", order = 0)]
    public class RD_StatConversions : SerializedScriptableObject
    {
        public ObscuredFloat intelligenceToMagicDamage;
        public ObscuredFloat strengthToMaxHealth;
        public ObscuredFloat strengthToDefence;
        public ObscuredFloat strengthToHealthRegen;
        public ObscuredFloat dexterityToMoveSpeed;
        public ObscuredFloat dexterityToAttackSpeed;
        public ObscuredFloat dexterityToEvasion;
    }
}