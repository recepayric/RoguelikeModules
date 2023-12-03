using System;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.WeaponRelated
{
    public class WeaponUpgradeTree : MonoBehaviour
    {
        public int freeStatPoints;
        public int totalStatPoints;

        //Types = melee, ranged, magic, trap
        //Change their element or make them elemental!
        //Upgrade their basic stats like attack speed, range, damage...
        //Change their appearance
        
        //Has requirement upgrade

        [Button]
        public void RefreshTree()
        {
            
        }

        public void AddStatPoint(int amount)
        {
            freeStatPoints += amount;
        }
    }

   

}