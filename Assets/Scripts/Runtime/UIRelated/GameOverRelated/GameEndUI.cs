using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndUI : MonoBehaviour, IOpenable
    {
        public GameObject VictoryText;
        public GameObject DefeatText;

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
        }

        public void OnClosed()
        {
            RemoveEvents();
        }
    }
}