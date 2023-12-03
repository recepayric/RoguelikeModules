using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Weapons/Upgrade Tree", order = 1)]
    public class WeaponUpgradeTreeSo : SerializedScriptableObject
    {
        public List<UpgradeTreeNode> TreeNodes;
    }

    public class UpgradeTreeNode
    {
        public string ID;
        public string NodeName;
        public string NodeText;
        public WeaponTypes WeaponType;
        public UpgradeType UpgradeType;

        [ShowIf("UpgradeType", UpgradeType.Element)]
        public Elements ElementToAdd;

        [ShowIf("UpgradeType", UpgradeType.Stats)]
        public AllStats Stat;

        [ShowIf("UpgradeType", UpgradeType.Stats)]
        public float IncreaseAmount;

        [ShowIf("UpgradeType", UpgradeType.SpecialModifier)]
        public SpecialModifiers Modifier;
        
        [Header("Connection")] public List<string> PreviousNodeIDs;


        [Header("Status")] private bool _isActive;
        public bool IsActive => _isActive;
    }

    public enum UpgradeType
    {
        Stats,
        SpecialModifier, 
        Element
    }

    public enum Elements
    {
        Fire,
        Ice,
        Lightning
    }
}