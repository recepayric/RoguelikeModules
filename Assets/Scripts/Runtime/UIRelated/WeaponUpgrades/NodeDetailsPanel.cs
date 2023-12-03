using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.WeaponUpgrades
{
    public class NodeDetailsPanel : MonoBehaviour
    {
        public TextMeshProUGUI nodeName;
        public List<TextMeshProUGUI> nodeAttributes;
        public int attributeCount = 0;

        public void Clear()
        {
            attributeCount = 0;
            for (int i = 0; i < nodeAttributes.Count; i++)
            {
                nodeAttributes[i].text = "";
            }
        }

        public void AddAttribute(string attribute)
        {
            nodeAttributes[attributeCount].text = attribute;
            attributeCount++;
        }

        public void SetName(string name)
        {
            nodeName.text = name;
        }
    }
}