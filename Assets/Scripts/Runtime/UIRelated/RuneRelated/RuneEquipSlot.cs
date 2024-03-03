using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.RuneRelated
{
    public class RuneEquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public RuneScreenUI RuneScreenUI;
        public RuneDataSo RuneDataSo;
        public Rune equippedRune;
        public RectTransform RectTransform;
        public BodyParts currentBodyPart;
        public int levelReq;
        public int slotNum;
        public bool isSlotOpen;

        public RuneInventorySlotUI slottedRune;
        
        public void Start()
        {
            RectTransform = GetComponent<RectTransform>();
            isSlotOpen = true;
        }

        public void SetSlot(BodyParts bodyPart)
        {
            currentBodyPart = bodyPart;
            var equippedRune = RuneDataSo.runeSlots[bodyPart][slotNum].equippedRune;
            levelReq = RuneDataSo.runeSlots[bodyPart][slotNum].levelRequirement;
            this.equippedRune = equippedRune;

            if (slottedRune != null)
            {
                slottedRune.StartRaycastTarget();
                slottedRune = null;
            }
            
            if (equippedRune != null)
            {
                slottedRune = RuneScreenUI.GetRuneInventoryUI(this.equippedRune);
                Debug.Log("Slotted Rune"  + slottedRune);
                this.equippedRune = slottedRune.rune;
                slottedRune.StopRaycastTarget();
            }
        }

        public void SetRune(Rune rune)
        {
            if(equippedRune != null)
                equippedRune.isEquipped = false;
            
            equippedRune = rune;
            equippedRune.isEquipped = true;
            equippedRune.equippedBodyPart = currentBodyPart; 
            RuneDataSo.runeSlots[currentBodyPart][slotNum].equippedRune = equippedRune;
            slottedRune = RuneScreenUI.GetRuneInventoryUI(equippedRune);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("On Top OF a RUNE SLOT!!!");
            RuneScreenUI.SetHoveredSlot(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            RuneScreenUI.SetHoveredSlot(null);
        }
    }
}