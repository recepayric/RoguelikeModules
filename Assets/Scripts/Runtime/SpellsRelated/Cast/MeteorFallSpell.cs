using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated.Cast
{
    public class MeteorFallSpell : SpellV2
    {
        public MeteorFallSpell()
        {
            castTime = 1;
        }

        public override void Cast()
        {
            base.Cast();
            CreateMeteor();
        }

        private void CreateMeteor()
        {
            var meteor = BasicPool.instance.Get(PoolKeys.MeteorFallSpell);
            var randPosX = Random.Range(-GameConfig.MapWidth, GameConfig.MapWidth);
            var randPosY = Random.Range(-GameConfig.MapHeight, GameConfig.MapHeight);

            meteor.transform.position = new Vector3(randPosX, randPosY);
        }
    }
}