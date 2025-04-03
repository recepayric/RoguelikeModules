using CodeStage.AntiCheat.ObscuredTypes;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Modifier;
using Runtime.Enums;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Controller
{
    public class CalculateStatsCommand : Command
    {
        [Inject] public GameObject EntityObject { get; set; }
        [Inject] public IStatModel StatModel { get; set; }
        [Inject] public IPlayerModel PlayerModel { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }
        public override void Execute()
        {
            var playerVo = PlayerModel.PlayerVos[EntityObject];
            var stats = StatModel.GetStats(EntityObject);
            var statConversions = StatModel.StatConversions;
            
            stats.ResetItemStats();
            playerVo.playerModifiers.itemModifiers.Clear();
            
            //Add stats from items
            
            ObscuredFloat moveSpeedFromDex = stats.GetBaseStat(AllStats.Dexterity) * statConversions.dexterityToMoveSpeed;
            ObscuredFloat attackSpeedFromDex = stats.GetBaseStat(AllStats.Dexterity) * statConversions.dexterityToAttackSpeed;
            ObscuredFloat evasionFromDex = stats.GetBaseStat(AllStats.Dexterity) * statConversions.dexterityToEvasion;
            ObscuredFloat maxHealthFromStr = stats.GetBaseStat(AllStats.Strength) * statConversions.strengthToMaxHealth;
            ObscuredFloat defenceFromStr = stats.GetBaseStat(AllStats.Strength) * statConversions.strengthToDefence;
            ObscuredFloat healthRegenFromStr = stats.GetBaseStat(AllStats.Strength) * statConversions.strengthToHealthRegen;

            stats.SetSecondaryStat(AllStats.MoveSpeedIncrease, attackSpeedFromDex);
            stats.SetSecondaryStat(AllStats.AttackSpeed, moveSpeedFromDex);
            stats.SetSecondaryStat(AllStats.Evasion, evasionFromDex);
            stats.SetSecondaryStat(AllStats.MaxHealth, maxHealthFromStr);
            stats.SetSecondaryStat(AllStats.Armor, defenceFromStr);
            stats.SetSecondaryStat(AllStats.HealthRegen, healthRegenFromStr);
            
            stats.CalculateFinalStats();
            stats.StatsChanged();
            
            EntitySignals.StatsUpdatedSignal.Dispatch();
        }
    }
}