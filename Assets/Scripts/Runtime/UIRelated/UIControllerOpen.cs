using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated
{
    public class UIControllerOpen : MonoBehaviour
    {
        public GameObject activeScreen;
        public GameObject weaponUpgradeScreen;
        public GameObject floorScreen;
        public GameObject characterSelectScreen;
        public GameObject mapSelectScreen;
        public GameObject levelUpScreen;
        public GameObject marketScreen;
        public GameObject gameEndScreen;
        public GameObject runeScreen;

        private Dictionary<Screens, GameObject> _gameScreens;

        private void Start()
        {
            AddScreensIntoDictionary();
            AddEvents();
        }

        public void OnOpenScreen(Screens screen, bool destroyPreviousScreen)
        {
            Debug.Log("Opening a new screen!!!");
            //Destroy Previous Screens
            if (destroyPreviousScreen)
            {
                foreach (var sc in _gameScreens)
                {
                    if(sc.Key == screen)
                        continue;
                    
                    //todo Get its interface and call "Closed" method.
                    sc.Value.SetActive(false);
                }
            }
            
            //Open new screen
            _gameScreens[screen].SetActive(true);
            var scOpenable = _gameScreens[screen].GetComponent<IOpenable>();
            if (scOpenable != null)
            {
                
                    scOpenable.OnOpened();
                
            }
            //todo Get its interface and call "Opened" method.
        }
        
        public void OnCloseScreen(Screens screen)
        {
            //Open new screen
            var scOpenable = _gameScreens[screen].GetComponent<IOpenable>();
            if(scOpenable != null)
                scOpenable.OnClosed();
            
            _gameScreens[screen].SetActive(false);
            //todo Get its interface and call "Closed" method.
        }
        
        
        private void AddScreensIntoDictionary()
        {
            _gameScreens ??= new Dictionary<Screens, GameObject>();
            _gameScreens.Add(Screens.WeaponUpgrade, weaponUpgradeScreen);
            _gameScreens.Add(Screens.Floor, floorScreen);
            _gameScreens.Add(Screens.CharacterSelect, characterSelectScreen);
            _gameScreens.Add(Screens.MapSelect, mapSelectScreen);
            _gameScreens.Add(Screens.LevelUp, levelUpScreen);
            _gameScreens.Add(Screens.Market, marketScreen);
            _gameScreens.Add(Screens.GameEnd, gameEndScreen);
            _gameScreens.Add(Screens.Rune, runeScreen);
        }

        private void OnGameStart()
        {
            Debug.Log("UI Controller - Game Start!");
        }

        private void AddEvents()
        {
            EventManager.Instance.OnOpenScreen += OnOpenScreen;
            EventManager.Instance.OnCloseScreen += OnCloseScreen;
            EventManager.Instance.GameStartEvent += OnGameStart;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.OnOpenScreen -= OnOpenScreen;
            EventManager.Instance.OnCloseScreen -= OnCloseScreen;
            EventManager.Instance.GameStartEvent -= OnGameStart;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }

    public enum Screens
    {
        WeaponUpgrade,
        Floor,
        CharacterSelect,
        MapSelect,
        LevelUp,
        Market,
        GameEnd,
        Rune
    }
}