using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class FloorCreator : MonoBehaviour
{
    public List<GameObject> floorPrefabs;
    public GameObject floorParent;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int floorWidth;
    public int floorHeight;

    public float floorSizeX;
    public float floorSizeZ;

    [Button]
    public void CreateFloor()
    {
        ClearFloor();
        var xOffset = -floorWidth / 2;
        var yOffset = -floorHeight / 2;
        for (int i = 0; i < floorWidth; i++)
        {
            for (int j = 0; j < floorHeight; j++)
            {
                GameObject floor = (GameObject)PrefabUtility.InstantiatePrefab(floorPrefab);
                var width = floor.GetComponent<Renderer>().bounds.size.x;
                var length = floor.GetComponent<Renderer>().bounds.size.z;

                floorSizeX = width;
                floorSizeZ = length;

                var posX = (i + xOffset) * width;
                var posZ = (j + yOffset) * length;

                floor.transform.SetParent(floorParent.transform);
                floor.transform.position = new Vector3(posX, 0, posZ);
                floorPrefabs.Add(floor);
            }
        }

        CreateWalls();
    }

    private void CreateWalls()
    {
        var xOffset = -floorWidth / 2;
        var yOffset = -floorHeight / 2;

        var minX = -floorWidth / 2;
        var minZ = -floorWidth / 2;

        var maxX = floorWidth - 1 - xOffset;
        var maxZ = floorHeight - 1 - yOffset;

        for (int z = 0; z < floorWidth; z++)
        {
            for (int x = 0; x < floorHeight; x++)
            {
                if (z > 0 && z < floorWidth-1)
                {
                    if (x > 0 && x < floorHeight-1)
                    {
                        continue;
                    }
                }

                GameObject wall = (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);

                if (z > 0 && z < floorWidth - 1)
                {
                    wall.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                }
                
                var posX = (x + xOffset) * floorSizeX;
                posX += x == 0 ? -floorSizeX / 2 : 0;
                posX += x == floorWidth-1 ? floorSizeX / 2 : 0;
                
                var posZ = (z + yOffset) * floorSizeZ;
                
                posZ += z == 0 ? -floorSizeZ / 2 : 0;
                posZ += z == floorWidth-1 ? floorSizeZ / 2 : 0;

                wall.transform.SetParent(floorParent.transform);
                wall.transform.position = new Vector3(posX, 0, posZ);
                floorPrefabs.Add(wall);
            }
        }
    }

    [Button]
    public void ClearFloor()
    {
        for (int i = 0; i < floorPrefabs.Count; i++)
        {
            DestroyImmediate(floorPrefabs[i]);
        }

        floorPrefabs.Clear();
    }
}