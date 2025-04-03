using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Runtime.EditorRealted
{
    public class FloorCreator : MonoBehaviour
    {

#if UNITY_EDITOR
        
        public GameObject floorPrefab;

        public int xNum;
        public int zNum;

        public float floorSizeX;
        public float floorSizeZ;

        public float yPos;
        public float scale;

        public bool randomizeRotation = false;
        public bool isWall = false;

        public List<GameObject> floorObjects;

        [Button]
        public void CreateFlor()
        {
            DestroyAllFloors();
            transform.localScale = Vector3.one;
            
            for (int z = 0; z < zNum; z++)
            {
                for (int x = 0; x < xNum; x++)
                {
                    GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(floorPrefab);

                    var posX = floorSizeX * x;
                    var posZ = floorSizeZ * z;
                    
                    floor.transform.SetParent(transform);
                    floor.transform.localPosition = new Vector3(posX, yPos, posZ);

                    if (randomizeRotation)
                    {
                        var randRotation = Random.Range(0, 4);
                        var angle = randRotation * 90;
                        
                        floor.transform.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
                        
                    }else if (isWall)
                    {
                        floor.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

                    }
                    
                    floorObjects.Add(floor);
                }
            }
            
            transform.localScale = Vector3.one*scale;

        }

        public void DestroyAllFloors()
        {
            for (int i = 0; i < floorObjects.Count; i++)
            {
                DestroyImmediate(floorObjects[i]);
            }
            
            floorObjects.Clear();
        }
        
#endif

    }
}