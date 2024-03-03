using System;
using Data;
using Data.LevelUp;
using Data.WeaponDataRelated;
using Runtime.Enums;
using Runtime.ItemsRelated;
using Runtime.StatValue;
using Runtime.TowerRelated;
using Runtime.UIRelated;
using Runtime.UIRelated.RuneRelated;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        private void Awake()
        {
            Instance = this;
            //Debug.Log("Event Manager is loaded!");
        }
        
        public event UnityAction<bool> SetMonsterSpawning;
        public void SetMonsterSpawn(bool status) => SetMonsterSpawning?.Invoke(status);
        
        public event UnityAction UpdateResCountEvent;
        public void UpdateResCount() => UpdateResCountEvent?.Invoke();

        public event UnityAction<int> CreateTowerEvent;
        public void CreateTower(int tier) => CreateTowerEvent?.Invoke(tier);

        public event UnityAction<int> PrepareTowerEvent;
        public void PrepareTower(int tier) => PrepareTowerEvent?.Invoke(tier);

        public event UnityAction<Tower> UpdateTowerEvent;
        public void UpdateTower(Tower tower) => UpdateTowerEvent?.Invoke(tower);

        public event UnityAction UpgradeTowerEvent;
        public void UpgradeTower() => UpgradeTowerEvent?.Invoke();

        #region Animation Related

        public event UnityAction<float> LiftWandEvent;
        public void LiftWand(float timeToLift) => LiftWandEvent?.Invoke(timeToLift);

        public event UnityAction DownWandEvent;
        public void DownWand() => DownWandEvent?.Invoke();

        #endregion

        #region Gameplay Ralated

        public event UnityAction GameStartEvent;
        public void GameStart() => GameStartEvent?.Invoke();

        public event UnityAction LoadTowerEvent;
        public void LoadTower() => LoadTowerEvent?.Invoke();

        public event UnityAction WeaponsUpdatedEvent;
        public void WeaponsUpdated() => WeaponsUpdatedEvent?.Invoke();
        
        public event UnityAction<bool> GameEndEvent;
        public void GameEnd(bool isWon) => GameEndEvent?.Invoke(isWon);
        
        public event UnityAction RestartGameEvent;
        public void RestartGame() => RestartGameEvent?.Invoke();

        public event UnityAction<GameObject, AllStats, float, float> AddCurseEvent;

        public void AddCurse(GameObject gameObject, AllStats stat, float amount, float time) =>
            AddCurseEvent?.Invoke(gameObject, stat, amount, time);
        
        #endregion

        #region Game Start Related

        public event UnityAction<CharacterDataSo> CharacterSelectedEvent;
        public void CharacterSelected(CharacterDataSo characterDataSo) => CharacterSelectedEvent?.Invoke(characterDataSo);
        
        public event UnityAction<WeaponDataSo> WeaponSelectedEvent;
        public void WeaponSelected(WeaponDataSo weaponDataSo) => WeaponSelectedEvent?.Invoke(weaponDataSo);

        #endregion
        
        #region Level Up Related

        public event UnityAction<LevelUpStats> LevelUpStatSelectedEvent;
        public void LevelUpStatSelected(LevelUpStats levelUpStat) => LevelUpStatSelectedEvent?.Invoke(levelUpStat);

        public event UnityAction<float> OrbCollectedEvent;
        public void OrbCollected(float value) => OrbCollectedEvent?.Invoke(value);

        #endregion

        #region Item Related

        public event UnityAction<Item> ItemBuyEvent;
        public void ItemBuy(Item item) => ItemBuyEvent?.Invoke(item);
        
        public event UnityAction<PoolKeys> WeaponBuyEvent;
        public void WeaponBuy(PoolKeys weaponPoolKey) => WeaponBuyEvent?.Invoke(weaponPoolKey);

        #endregion

        #region Floor Related

        public event UnityAction<int> FloorEndsEvent;
        public void FloorEnds(int floorNum) => FloorEndsEvent?.Invoke(floorNum);

        public event UnityAction FloorStartsEvent;
        public void FloorStarts() => FloorStartsEvent?.Invoke();

        public event UnityAction FloorLoadEvent;
        public void FloorLoad() => FloorLoadEvent?.Invoke();

        public event UnityAction FloorExitEvent;
        public void FloorExit() => FloorExitEvent?.Invoke();


        #endregion

        #region UI Related

        public event UnityAction<int> UpdateFloorTimerEvent;
        public void UpdateFloorTimer(int remainingTime) => UpdateFloorTimerEvent?.Invoke(remainingTime);

        public event UnityAction<int> UpdateFloorNumberEvent;
        public void UpdateFloorNumber(int floorNumber) => UpdateFloorNumberEvent?.Invoke(floorNumber);

        public event UnityAction<Screens, bool> OnOpenScreen;

        public void OpenScreen(Screens sceneToOpen, bool closePreviousScreens) =>
            OnOpenScreen?.Invoke(sceneToOpen, closePreviousScreens);

        public event UnityAction<Screens> OnCloseScreen;
        public void CloseScreen(Screens sceneToClose) => OnCloseScreen?.Invoke(sceneToClose);

        public event UnityAction<float, float, float> UpdateLevelProgressEvent;
        public void UpdateLevelProgress(float percentage, float experience, float neededExperience) =>
            UpdateLevelProgressEvent?.Invoke(percentage, experience, neededExperience);
        
        public event UnityAction<int> SetLevelUpAmountEvent;
        public void SetLevelUpAmount(int levelUpAmount) => SetLevelUpAmountEvent?.Invoke(levelUpAmount);

        public event UnityAction<WeaponUpgradeTreeSo> SetWeaponDataForTreeEvent;
        public void SetWeaponDataForTree(WeaponUpgradeTreeSo weaponTreeDatasSo) => SetWeaponDataForTreeEvent?.Invoke(weaponTreeDatasSo);
        
        public event UnityAction<Weapon> SetWeaponForTreeEvent;
        public void SetWeaponForTree(Weapon weapon) => SetWeaponForTreeEvent?.Invoke(weapon);

        //public event UnityAction<string> CharacterSelectChangedEvent;
        //public void CharacterSelectChanged(string characterName) => CharacterSelectChangedEvent?.Invoke(characterName);
        
        public event UnityAction<PoolKeys> CharacterSelectChangedEvent;
        public void CharacterSelectChanged(PoolKeys poolKey) => CharacterSelectChangedEvent?.Invoke(poolKey);
        
        public event UnityAction<PoolKeys> WeaponSelectChangedEvent;
        public void WeaponSelectChanged(PoolKeys poolKey) => WeaponSelectChangedEvent?.Invoke(poolKey);

        public event UnityAction<bool> SetCharacterCameraStatusEvent;
        public void SetCharacterCameraStatus(bool status) => SetCharacterCameraStatusEvent?.Invoke(status);

        public event UnityAction<RuneInventorySlotUI> HoverRuneEvent;
        public void HoverRune(RuneInventorySlotUI rune) => HoverRuneEvent?.Invoke(rune);

        public event UnityAction CloseRuneDetailsEvent;
        public void CloseRuneDetails() => CloseRuneDetailsEvent?.Invoke();

        public event UnityAction RuneDragStartEvent;
        public void RuneDragStart() => RuneDragStartEvent?.Invoke();

        public event UnityAction RuneDragCancelledEvent;
        public void RuneDragCancelled() => RuneDragCancelledEvent?.Invoke();

        public event UnityAction<RuneInventorySlotUI, RuneInventorySlotUI> RuneReplaceEvent;

        public void RuneReplace(RuneInventorySlotUI newRune, RuneInventorySlotUI oldRune) =>
            RuneReplaceEvent?.Invoke(newRune, oldRune);
        
        #endregion

        public event UnityAction PlayerDiesEvent;
        public void PlayerDies() => PlayerDiesEvent?.Invoke();


        #region Sound Related

        public event UnityAction<Sounds, float> PlaySoundOnceEvent;
        public void PlaySoundOnce(Sounds sound, float volume) => PlaySoundOnceEvent?.Invoke(sound, volume);
        

        #endregion

        #region Static Events

        public static event UnityAction<float> EventSetDistanceBetweenEnemy;
        public void SetDistanceBetweenEnemy(float distance) => EventSetDistanceBetweenEnemy?.Invoke(distance);

        #endregion
    }
}