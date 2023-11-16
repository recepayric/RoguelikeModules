using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;


    public GameObject textObject;

    private void Awake()
    {
        instance = this;
    }

    public void AddDamageText(GameObject enemy, int damage, bool isCritical)
    {
        var text = Instantiate(textObject);
        text.transform.position = enemy.transform.position + Vector3.up * 0.5f;

        text.GetComponent<TextMeshPro>().text = damage.ToString();
        text.GetComponent<TextMeshPro>().color = isCritical ? Color.red : Color.white;

        text.transform.DOMoveY(1, 0.5f).SetRelative(true).OnComplete(() => { Destroy(text); });
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