using System.Collections.Generic;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.UnityObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views.Screens
{
    public class GamePlayScreensView : MVCView, IPanel
    {
	    public IPanelVo vo { get; set; }
	    
	    //We want this to be initialized by ScreenManager
	    public override bool autoRegisterWithContext { get=>false; }
        
	    public TextMeshProUGUI floorText;
	    public TextMeshProUGUI countdownTimerText;
	    public TextMeshProUGUI floorTimerText;
	    
	    public Image healthFillBar;
	    public Image levelFillBar;
	    public TextMeshProUGUI healthText;
	    public TextMeshProUGUI skullText;
	    public TextMeshProUGUI goldText;
	    public TextMeshProUGUI doubleSkullCountText;
	    public TextMeshProUGUI levelText;
	    public TextMeshProUGUI healthPotionNumber;
	    
	    
	    public TextMeshProUGUI infoText;
	    public TextMeshProUGUI roomNameText;
	    public TextMeshProUGUI killedEnemyText;
	    public TextMeshProUGUI totalEnemyText;

	    public int killedEnemyCount;
	    public int aliveEnemyCount;
	    public int totalEnemyCount;

	    public GameObject bossHealthBar;
	    public Image bossHealthFillBar;
	    public TextMeshProUGUI bossHealthPercentage;
	    public TextMeshProUGUI bossName;

	    public int startLevel;
	    public int levelGain;
	    
	    public Image bloodImage;
	    public Color bloodImageColor;


	    public void UpdateEnemyCount()
	    {
		    var aliveEnemy = totalEnemyCount - killedEnemyCount;

		    killedEnemyText.text = killedEnemyCount + " Killed";
		    totalEnemyText.text = aliveEnemy + " / " + totalEnemyCount;
	    }


	    public void ResetSkills()
	    {
	    }

	    public void SetStartLevel(int pStartLevel)
	    {
		    startLevel = pStartLevel;
	    }
	    
	    public void SetExperienceBar(int level, float experiencePercentage)
	    {
		    levelGain = level - startLevel;
		    levelText.text = "level " + level;

		    if (levelGain > 0)
			    levelText.text += " <color=\"green\">+" + levelGain;
		    
		    

		    levelFillBar.fillAmount = experiencePercentage;
	    }

	    public void UpdateDoubleSkull(int amount)
	    {
		    doubleSkullCountText.text = amount.ToString();
	    }

	    public void SetFloorText(int floor)
	    {
		    floorText.text = "Floor " + floor;
	    }
	    
	    public void UpdateSkull(string skullAmountString)
	    {
		    skullText.text = skullAmountString;
	    }
	    
	    public void UpdateHealth(float currentHealth, float maxHealth)
	    {
		    var healthPercentage = currentHealth / maxHealth;
		    healthFillBar.fillAmount = healthPercentage;

		    int currentHealthInt = (int)currentHealth;
		    int maxHealthInt = (int)maxHealth;
		    healthText.text = currentHealthInt + "/" + maxHealthInt;
	    }
	    
	    public void UpdateBossHealth(float currentHealth, float maxHealth)
	    {
		    var healthPercentage = currentHealth / maxHealth;
		    bossHealthFillBar.fillAmount = healthPercentage;
		    bossHealthPercentage.text = (int)(healthPercentage*100) + "%";
	    }


	    public void SetFloorTimerText(string text)
	    {
		    floorTimerText.text = text;
	    }
	    
	    public void SetCountdownMessage(string message)
	    {
		    countdownTimerText.text = message;
	    }
	    
	    public void TriggerHitEffect()
	    {
		    DOTween.Kill("BloodEffect");
		    bloodImageColor.a = 0.1f;
            
		    float alphaValue = 0.1f;
		    bloodImage.color = bloodImageColor;
            
		    DOTween.To(()=> alphaValue, x=> alphaValue = x, 0, 0.5f).OnUpdate(() =>
		    {
			    if (bloodImage == null)
			    {
				    DOTween.Kill("BloodEffect");
				    return;
			    }

			    bloodImageColor.a = alphaValue;
			    bloodImage.color = bloodImageColor;
		    }).SetId("BloodEffect");
            
	    }

	    public void CancelHitEffect()
	    {
		    bloodImageColor.a = 0;
		    bloodImage.color = bloodImageColor;
		    DOTween.Kill("BloodEffect");
	    }
        
    }
}
