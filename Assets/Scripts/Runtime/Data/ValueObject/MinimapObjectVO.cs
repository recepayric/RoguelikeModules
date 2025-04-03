using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class MinimapObjectVO
    {
        public MinimapObjectType type;
        public GameObject minimapObject;
        public BoxCollider boxCollider;

        public Sprite minimapIcon;
        public float minimapIconSize;
    }

    public enum MinimapObjectType
    {
        None,
        Base,
        Object,
        Player,
        Enemy
    }
}