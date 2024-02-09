using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndUI : MonoBehaviour, IOpenable
    {
        public GameObject VictoryText;
        public GameObject DefeatText;

        public GameEndStats gameEndStats;
        public GameEndWeaponsUI gameEndWeaponsuI;

        public void ShowStats()
        {
            gameEndStats.ShowStats();
        }

        public void ShowWeapons()
        {
            gameEndWeaponsuI.SetWeapons();
        }
        
        public void ShowItems()
        {
            gameEndWeaponsuI.SetWeapons();
        }

        public void RestartButton()
        {
            EventManager.Instance.RestartGame();
        }

        public void MenuButton()
        {
            EventManager.Instance.OpenScreen(Screens.CharacterSelect, true);
        }


        private void OnGameEnd(bool isWon)
        {
            if (isWon)
            {
                VictoryText.SetActive(true);
                DefeatText.SetActive(false);
            }
            else
            {
                VictoryText.SetActive(false);
                DefeatText.SetActive(true);
            }
        }

        private void AddEvents()
        {
            EventManager.Instance.GameEndEvent += OnGameEnd;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.GameEndEvent -= OnGameEnd;
        }

        public void OnOpened()
        {
            AddEvents();
            ShowStats();
            ShowWeapons();
        }

        public void OnClosed()
        {
            RemoveEvents();
        }
    }
}