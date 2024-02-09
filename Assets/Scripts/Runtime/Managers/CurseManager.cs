using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Structs;
using UnityEngine;

namespace Runtime.Managers
{
    public class CurseManager : MonoBehaviour
    {
        //private Dictionary<GameObject, CurseToAdd> _cursables;
        public List<CurseToAdd> _curses = new List<CurseToAdd>();
        
        private void OnAddCurse(GameObject gameObject, AllStats statToCurse, float amount, float time)
        {
            var canAdd = CanAddCurse(statToCurse);
            
            Debug.Log("Can Add: " + canAdd);
            if(!canAdd) return;
            
            var curse = new CurseToAdd(gameObject, statToCurse, amount, time);
            _curses.Add(curse);

            var cursable = DictionaryHolder.Cursable[gameObject];
            cursable.AddCurse(statToCurse, -amount);
        }

        private void Update()
        {
            for (int i = 0; i < _curses.Count; i++)
            {
                var curse = _curses[i];
                curse.remainingTime -= Time.deltaTime;

                if (curse.remainingTime <= 0)
                {
                    EndCurse(curse.GameObject, curse.statToAffect, curse.curseAmount);
                    _curses.RemoveAt(i);
                    i--;
                }
                else
                {
                    _curses[i] = curse;
                }
            }
        }

        private void EndCurse(GameObject gameObject, AllStats stat, float amount)
        {
            var cursable = DictionaryHolder.Cursable[gameObject];
            cursable.RemoveCurse(stat, amount);
        }

        private void Awake()
        {
            EventManager.Instance.AddCurseEvent += OnAddCurse;
        }


        private void OnDestroy()
        {
            EventManager.Instance.AddCurseEvent -= OnAddCurse;
        }
        
        private bool CanAddCurse(AllStats stat)
        {
            if (_curses == null)
                _curses = new List<CurseToAdd>();
            
            var curse = _curses.Find(t => t.statToAffect == stat);
            return curse.statToAffect == AllStats.None;
        }
    }
}