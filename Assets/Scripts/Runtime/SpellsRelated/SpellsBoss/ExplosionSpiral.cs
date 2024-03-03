using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated.SpellsBoss
{
    public class ExplosionSpiral : SpellV2
    {
        public int explosionNumber = 50;
        public float firstRadius = 3;
        public float radiusIncrease = 5f;
        public float angleIncrease = 2f;
        int spiralArm = 2;

        public ExplosionSpiral()
        {
        }

        public override void Cast()
        {
            base.Cast();
            for (int i = 0; i < explosionNumber; i++)
            {
            }

            explosionNumber = explosionNumber2;
            firstRadius = firstRadius2;
            radiusIncrease = radiusIncrease2;
            angleIncrease = angleIncrease2;
            spiralArm = spiralArm2;

            CreateExplosions();
        }

        private void CreateExplosions()
        {
            //var randPosX = Random.Range(-playerRadius, playerRadius);
            //var randPosY = Random.Range(-playerRadius, playerRadius);

            var angle = 0f;
            angle = 0;
            var angleBetween = Mathf.PI*2/spiralArm;


            for (int i = 0; i < explosionNumber; i++)
            {
                var i1 = i;
                DOVirtual.DelayedCall(0.01f * i, () =>
                {
                    int ola = i1;
                    var r = Mathf.Sqrt((ola * angleIncrease) + 1);
                    angle += +Mathf.Asin(1 / r);
                    
                    for (int j = 0; j < spiralArm; j++)
                    {
                        float xx = Mathf.Cos((angleBetween * j) + angle) * (firstRadius + r * radiusIncrease);
                        float yy = Mathf.Sin((angleBetween * j) + angle) * (firstRadius + r * radiusIncrease);
                        var finalAngle = j * angle + angleIncrease * ola;
                        var radius = firstRadius + radiusIncrease * ola;
                        var posX = radius * Mathf.Cos(finalAngle);
                        var posY = radius * Mathf.Sin(finalAngle);

                        var pos = source.gameObject.transform.position + new Vector3(xx, yy);
                        var canPlace = CheckIfWithinScreen(pos.x, pos.y);

                        if (canPlace)
                        {
                            var explosion = BasicPool.instance.Get(PoolKeys.ExplosionBoss1);
                            explosion.transform.position = pos;
                        }
                    }
                });
            }
        }

        private bool CheckIfWithinScreen(float x, float y)
        {
            var boundX = x < GameConfig.MapWidth && x > -GameConfig.MapWidth;
            var boundY = y < GameConfig.MapWidth && y > -GameConfig.MapWidth;

            //return true;
            return boundX && boundY;
        }
    }
}