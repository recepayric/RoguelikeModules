using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.RuneRelated
{
    public class RuneScreenUI : MonoBehaviour, IOpenable
    {
        public GameObject runesParent;
        public RuneDataSo RuneDataSo;
        public List<RectTransform> runeSlots;

        public List<Rune> availableRunes;
        public List<RuneInventorySlotUI> runeObjects;
        public List<RuneEquipSlot> runeEquipSlots;
        public GameObject RunePrefab;

        public GameObject RuneDetails;
        public TextMeshProUGUI runeName;
        public TextMeshProUGUI runeDetails;

        public List<RuneInventorySlotUI> SortedRuneList;
        public bool isDraggingRune = false;

        public RuneEquipSlot equippableSlot;
        public RuneInventorySlotUI currentlySelectedRune;
        public BodyParts selectedBodyPart;


        public void SortRunes()
        {
            SortedRuneList = new List<RuneInventorySlotUI>();
            List<RuneInventorySlotUI> tempList = new List<RuneInventorySlotUI>();
            List<RuneInventorySlotUI> tempList2 = new List<RuneInventorySlotUI>();
            tempList2.AddRange(runeObjects.FindAll(t => t.gameObject.activeSelf && !t.rune.isEquipped));
            //Sort Baesd on their levels!
            for (int i = 0; i < tempList2.Count; i++)
            {
                var currentRune = tempList2[i];
                for (int j = i + 1; j < tempList2.Count; j++)
                {
                    if (currentRune.rune.rune_level < tempList2[j].rune.rune_level)
                        currentRune = tempList2[j];
                }

                tempList2.Remove(currentRune);
                tempList.Add(currentRune);
                i--;
            }

            //Sort based on their rarities!
            int index = 0;
            for (int i = 4; i >= 0; i--)
            {
                var rarity = (ItemRarity)i;

                for (int j = 0; j < tempList.Count; j++)
                {
                    if (tempList[j].rune.runeRarity == rarity)
                        SortedRuneList.Add(tempList[j]);
                }
            }
        }

        [Button]
        public void ArrangeRunes()
        {
            SortRunes();
            for (int i = 0; i < SortedRuneList.Count; i++)
            {
                SortedRuneList[i].transform.position = runeSlots[i].transform.position;
            }
            
            for (int i = 0; i < runeEquipSlots.Count; i++)
            {
                if(runeEquipSlots[i].slottedRune != null)   
                    runeEquipSlots[i].slottedRune.transform.position = runeEquipSlots[i].transform.position;
            }

            Debug.Log("Runes are arranged!");
        }

        [Button]
        public void CreateRunes()
        {
            var runeNumber = GetTotalRunes();
            var listNumber = runeObjects.Count;

            var availableNumber = Mathf.Min(runeNumber, listNumber);
            var missing = runeNumber - listNumber;
            var exceeded = listNumber - runeNumber;

            int index = 0;
            //Set Available ones
            for (int i = 0; i < availableNumber; i++)
            {
                runeObjects[index].gameObject.SetActive(true);
                runeObjects[i].SetRune(availableRunes[i]);
                index++;
            }

            //Create Missing ones
            for (int i = 0; i < missing; i++)
            {
                var rn = Instantiate(RunePrefab, runesParent.transform);
                var sc = rn.GetComponent<RuneInventorySlotUI>();

                sc.SetRune(availableRunes[index]);
                runeObjects.Add(sc);

                index++;
            }

            //Remove Exceeded ones
            for (int i = 0; i < exceeded; i++)
            {
                runeObjects[index].gameObject.SetActive(false);
                index++;
            }
            Debug.Log(runeNumber + "   " + exceeded + "   " + missing + "   " + availableNumber);
            Debug.Log("Runes are created");
        }

        private int GetTotalRunes()
        {
            availableRunes.Clear();
            var bodyPartGeneral = BodyPartsGenral.Leg;

            if (selectedBodyPart == BodyParts.LeftLeg || selectedBodyPart == BodyParts.RightLeg)
                bodyPartGeneral = BodyPartsGenral.Leg;
            else if (selectedBodyPart == BodyParts.LeftArm || selectedBodyPart == BodyParts.RightArm)
                bodyPartGeneral = BodyPartsGenral.Arm;
            else if (selectedBodyPart == BodyParts.Head)
                bodyPartGeneral = BodyPartsGenral.Head;
            else if (selectedBodyPart == BodyParts.Torso)
                bodyPartGeneral = BodyPartsGenral.Torso;
            
            
            availableRunes.AddRange(RuneDataSo.runes.FindAll(t =>
            {
                var checkBodyPart = t.BodyPart == bodyPartGeneral;
                var isEquipped = t.isEquipped;
                var correctPart = t.equippedBodyPart == selectedBodyPart;

                if (!checkBodyPart) return false;
                if (!isEquipped) return true;
                if (!correctPart) return false;
                
                return true;
            }));
            return availableRunes.Count;
        }

        public void SelectBodyPart(int bodyPartNumber)
        {
            var bodyPart = (BodyParts)bodyPartNumber;

            if (selectedBodyPart == bodyPart) return;

            selectedBodyPart = bodyPart;
            CreateRunes();
            Debug.Log("Clicked to " + bodyPart);

            for (int i = 0; i < runeEquipSlots.Count; i++)
            {
                runeEquipSlots[i].SetSlot(bodyPart);
            }
            
            ArrangeRunes();
        }

        public void OpenRuneDetailsPage(RuneInventorySlotUI rune)
        {
            if (isDraggingRune) return;

            currentlySelectedRune = rune;
            RuneDetails.SetActive(true);
            RuneDetails.GetComponent<RectTransform>().anchoredPosition =
                rune.GetComponent<RectTransform>().anchoredPosition;


            //Set Text Details
            var text = "Rarity: " + rune.rune.runeRarity;
            text += "\nRune Level: " + rune.rune.rune_level;
            text += "\nBody Part: " + rune.rune.BodyPart;
            text += "\n";

            for (int i = 0; i < rune.rune.stats.Count; i++)
            {
                text += "\n+" + rune.rune.stats[i].currentValue + " " + rune.rune.stats[i].stat;
            }

            runeDetails.text = text;
        }

        public void SetHoveredSlot(RuneEquipSlot slot)
        {
            equippableSlot = slot;
        }
        
        public void HideRuneDetailsPage()
        {
            RuneDetails.SetActive(false);
        }

        public void RuneDragStart()
        {
            HideRuneDetailsPage();
            isDraggingRune = true;
        }

        public void RuneDragCancelled()
        {
            isDraggingRune = false;
            
            if(equippableSlot != null)
                EquipRune();
        }

        public void EquipRune()
        {
            equippableSlot.SetRune(currentlySelectedRune.rune);
            ArrangeRunes();
        }

        private void AddEvents()
        {
            EventManager.Instance.HoverRuneEvent += OpenRuneDetailsPage;
            EventManager.Instance.CloseRuneDetailsEvent += HideRuneDetailsPage;
            EventManager.Instance.RuneDragStartEvent += RuneDragStart;
            EventManager.Instance.RuneDragCancelledEvent += RuneDragCancelled;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.HoverRuneEvent -= OpenRuneDetailsPage;
            EventManager.Instance.CloseRuneDetailsEvent -= HideRuneDetailsPage;
            EventManager.Instance.RuneDragStartEvent -= RuneDragStart;
            EventManager.Instance.RuneDragCancelledEvent -= RuneDragCancelled;
        }

        public void OnOpened()
        {
            AddEvents();
            CreateRunes();
            ArrangeRunes();
        }

        public void OnClosed()
        {
            RemoveEvents();
        }
        public RuneInventorySlotUI GetRuneInventoryUI(Rune rune)
        {
            Debug.Log("rune: " + rune.rune_id);
            return runeObjects.Find(t => t.rune.rune_id == rune.rune_id);
        }
    }

    public enum BodyParts
    {
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg,
        Head,
        Torso
    }

    public enum BodyPartsGenral
    {
        Arm,
        Leg,
        Head,
        Torso
    }
}