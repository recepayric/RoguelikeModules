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

    private List<GameObject> damageTexts = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void AddDamageText(GameObject enemy, float damage, bool isCritical)
    {
        var text = Instantiate(textObject);
        text.transform.position = enemy.transform.position + Vector3.up * 1.5f;

        text.GetComponent<TextMeshPro>().text = damage.ToString();
        text.GetComponent<TextMeshPro>().color = isCritical ? Color.red : Color.white;

        text.transform.DOMoveY(1, 0.5f).SetRelative(true).OnComplete(() => { DestroyText(text); });
        
        damageTexts.Add(text);
    }

    private void DestroyText(GameObject textObject)
    {
        damageTexts.Remove(textObject);
        Destroy(textObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < damageTexts.Count; i++)
        {
            damageTexts[i].transform.LookAt(Camera.main.transform);
        }
        
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