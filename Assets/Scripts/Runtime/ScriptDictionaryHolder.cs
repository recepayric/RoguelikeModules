using System.Collections.Generic;
using Runtime.Collectables;
using Runtime.Effects;
using Runtime.Minions;
using Runtime.TowerRelated;
using UnityEngine;

namespace Runtime
{
    public class ScriptDictionaryHolder
    {
        public static Player Player;
        public static Dictionary<GameObject, Explosion> Explosions = new Dictionary<GameObject, Explosion>();
        public static Dictionary<GameObject, Projectile> Projectiles = new Dictionary<GameObject, Projectile>();
        public static Dictionary<GameObject, Minion> Minions = new Dictionary<GameObject, Minion>();
        public static Dictionary<GameObject, Enemy> Enemies = new Dictionary<GameObject, Enemy>();
        public static Dictionary<GameObject, Collectable> Collectables = new Dictionary<GameObject, Collectable>();


        public static Tower CurrentTower;
    }
}