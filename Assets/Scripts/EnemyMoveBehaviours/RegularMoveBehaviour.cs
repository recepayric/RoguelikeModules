using UnityEngine;

namespace EnemyMoveBehaviours
{
    [CreateAssetMenu(fileName = "Data", menuName = "EnemyMove/RegularMove", order = 0)]
    public class RegularMoveBehaviour : ScriptableObject
    {
        public GameObject playerObject;

        public void Move(float speed, Transform transform)
        {
            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = playerObject.transform.position.x;
            var playerY = playerObject.transform.position.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);
            
            var deltaX = Time.deltaTime*speed*Mathf.Cos(angle);
            var deltaY = Time.deltaTime*speed*Mathf.Sin(angle);

            transform.position += new Vector3(deltaX, deltaY, 0);
        }
    }
}
