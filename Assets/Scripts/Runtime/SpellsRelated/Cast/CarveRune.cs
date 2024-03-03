using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated.Cast
{
    public class CarveRune : SpellV2
    {

        public CarveRune()
        {
            castTime = 1;
        }

        public override void Cast()
        {
            base.Cast();
            CreateRune();
        }

        private void CreateRune()
        {
            var rune = BasicPool.instance.Get(PoolKeys.Explosion_Rune_1);
            var randPosX = Random.Range(-GameConfig.MapWidth, GameConfig.MapWidth);
            var randPosY = Random.Range(-GameConfig.MapHeight, GameConfig.MapHeight);

            rune.transform.position = new Vector3(randPosX, randPosY);
            var runeSC = DictionaryHolder.Runes[rune];
            runeSC.Prepare();
        }
    }
}