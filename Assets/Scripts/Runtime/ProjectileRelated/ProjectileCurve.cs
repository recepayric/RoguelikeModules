using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.ProjectileRelated
{
    
    public class ProjectileCurve : MonoBehaviour
    {
        public AnimationCurve curveX;
        public AnimationCurve curveY;

        public float turnCompleteTime;

        public GameObject initialPos;
        public GameObject targetPos;

        [OnValueChanged("UpdateObject")]
        [Range(0f, 1f)]
        public float time;
        
        [Button]
        public void RandomiseCurve()
        {

            if (curveY.keys.Length < 4)
            {
                var missing = 4 - curveY.keys.Length;
                for (int i = 0; i < missing; i++)
                {
                    curveY.AddKey(Random.Range(0f, 1f), 1);
                }
            }
            
            
            var keys = curveY.keys;
            
            keys[0].time = 0;
            keys[0].value = 0;

            var val = Random.Range(0.8f, 1f);
            var half = Random.Range(0f, val);
            
            
            keys[1].time = half;
            keys[1].value = Random.Range(3, 5);
            
            keys[2].time = val;
            keys[2].value = 0;
            
            keys[3].time = 1;
            keys[3].value = 0;

            curveY.keys = keys;
        }

        public void UpdateObject()
        {
            //Debug.Log("Changed");
            var diff = targetPos.transform.position - initialPos.transform.position;
            
            transform.right = targetPos.transform.position - initialPos.transform.position;
            
            var top = transform.up*curveY.Evaluate(time);
            var targetPoss = initialPos.transform.position + diff * (curveX.Evaluate(time)) - top;

            //transform.right = targetPos.transform.position - initialPos.transform.position;
            transform.right = targetPoss - transform.position;
            transform.position = targetPoss;
        }
    }
}