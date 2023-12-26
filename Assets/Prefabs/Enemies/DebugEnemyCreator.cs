using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugEnemyCreator : MonoBehaviour
{

    public GameObject parent;
    public GameObject enemyToCreate;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public List<GameObject> enemies;

    public int createAmount;
    [Button]
    public void CreateEnemy()
    {
        for (int i = 0; i < createAmount; i++)
        {
            var enemy = Instantiate(enemyToCreate);
            var x = Random.Range(xMin, xMax);
            var y = Random.Range(yMin, yMax);
        
            enemy.transform.SetParent(parent.transform);
            enemy.transform.position = new Vector3(x, y, 0);
        
            enemies.Add(enemy);
        }
        
    }

    [Button]
    public void ResetEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }

        enemies.Clear();
    }

    [Button]
    public void RandomiseEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var x = Random.Range(xMin, xMax);
            var y = Random.Range(yMin, yMax);
            enemies[i].transform.position = new Vector3(x, y, 0);
        }
    }

    [Button]
    public void TurnOffAnimators()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var animator = enemies[i].GetComponent<Animator>();
            animator.enabled = false;
        }
    }

    [Button]
    public void TurnOnAnimators()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var animator = enemies[i].GetComponent<Animator>();
            animator.enabled = true;
        }
    }

    [Button]
    public void TurnOffFlames()
    {
        var flames = GameObject.FindGameObjectsWithTag("HeadFlame");
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].gameObject.SetActive(false);
        }
    }
    [Button]
    public void TurnOnFlames()
    {
        var flames = GameObject.FindGameObjectsWithTag("HeadFlame");
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].gameObject.SetActive(true);
        }
    }

    [Button]
    public void TurnOffRigidbody()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var rg = enemies[i].GetComponent<Rigidbody2D>();
            rg.isKinematic = true;
        }
    }

    [Button]
    public void TurnOnRigidbody()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var rg = enemies[i].GetComponent<Rigidbody2D>();
            rg.isKinematic = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
