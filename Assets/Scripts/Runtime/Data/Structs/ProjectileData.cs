using System;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Structs
{
    [Serializable]
    public class ProjectileData
    {
        [Title("PoolKeys")]
        public PoolKey poolKey;
        public PoolKey muzzleKey;
        public PoolKey arrowMarkerKey;
        
        [Title("Game Objects")]
        public GameObject projectileObject;
        public GameObject projectileSource;
        public GameObject projectileParent;
        public GameObject targetObject;
        
        
        public EntityType targetType;
        
        public Vector3 initialPosition;
        public Vector3 direction;
        public Vector3 targetPosition;
        
        public float extraAngle;
        public float speed;
        public float damage;
        public float minDistance;
        public float maxDistance;
        public float minSpeedMultiplier;
        public float maxSpeedMultiplier;
        public float laserRange;
        
        public int bounceNumber;
        public int pierceNumber;
        
        public bool followTarget;
        public bool hasArrowMarker;
        public bool overrideMaxDistance;
        public bool overrideSpeedMultiplier;
        public bool updateLaserTarget;
    }
}