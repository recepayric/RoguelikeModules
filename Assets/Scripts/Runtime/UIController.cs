using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Managers;
using Runtime.UIRelated;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject collectableCollectPlace;
    
    public GameObject textObject;

    private void Awake()
    {
        instance = this;
    }

    public void AddDamageText(GameObject enemy, float damage, bool isCritical)
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
            EventManager.Instance.OpenScreen(Screens.Floor, true);
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
            EventManager.Instance.OpenScreen(Screens.WeaponUpgrade, true);
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
            EventManager.Instance.OpenScreen(Screens.CharacterSelect, true);
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            EventManager.Instance.OpenScreen(Screens.MapSelect, true);
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
            EventManager.Instance.OpenScreen(Screens.Market, true);
        
        if(Input.GetKeyDown(KeyCode.Alpha6))
            EventManager.Instance.OpenScreen(Screens.Rune, true);
    }
}