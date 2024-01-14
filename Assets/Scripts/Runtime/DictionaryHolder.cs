using System.Collections.Generic;
using Runtime.Collectables;
using Runtime.Effects;
using Runtime.Interfaces;
using Runtime.Minions;
using Runtime.TowerRelated;
using Runtime.WeaponRelated;
using UnityEngine;

namespace Runtime
{
    public class DictionaryHolder
    {
        public static Player Player;
        public static Dictionary<GameObject, Explosion> Explosions = new Dictionary<GameObject, Explosion>();
        public static Dictionary<GameObject, Projectile> Projectiles = new Dictionary<GameObject, Projectile>();
        public static Dictionary<GameObject, Minion> Minions = new Dictionary<GameObject, Minion>();
        public static Dictionary<GameObject, Enemy> Enemies = new Dictionary<GameObject, Enemy>();
        public static Dictionary<GameObject, Collectable> Collectables = new Dictionary<GameObject, Collectable>();
        public static Dictionary<GameObject, IDamageable> Damageables = new Dictionary<GameObject, IDamageable>();
        public static Dictionary<GameObject, RotatingMeleeWeapons> RotatingMeleeWeapons = new Dictionary<GameObject, RotatingMeleeWeapons>();


        public static Tower CurrentTower;
    }
}