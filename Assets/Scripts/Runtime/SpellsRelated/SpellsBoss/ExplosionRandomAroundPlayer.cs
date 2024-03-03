using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated.SpellsBoss
{
    public class ExplosionRandomAroundPlayer : SpellV2
    {
        public int explosionNumber = 10;
        public float playerRadius = 5;
        
        public ExplosionRandomAroundPlayer()
        {
        }

        public override void Cast()
        {
            base.Cast();
            for (int i = 0; i < explosionNumber; i++)
            {
                CreateExplosions();
            }
        }

        private void CreateExplosions()
        {
            var randPosX = Random.Range(-playerRadius, playerRadius);
            var randPosY = Random.Range(-playerRadius, playerRadius);

            var pos = DictionaryHolder.Player.transform.position + new Vector3(randPosX, randPosY);
            var canPlace = CheckIfWithinScreen(pos.x, pos.y);

            if (canPlace)
            {
                var explosion = BasicPool.instance.Get(PoolKeys.ExplosionBoss1);
                explosion.transform.position = pos;
            }
        }
        
        private bool CheckIfWithinScreen(float x, float y)
        {
            var boundX = x < GameConfig.MapWidth && x > -GameConfig.MapWidth;
            var boundY = y < GameConfig.MapWidth && y > -GameConfig.MapWidth;

            return boundX && boundY;
        }
    }
}