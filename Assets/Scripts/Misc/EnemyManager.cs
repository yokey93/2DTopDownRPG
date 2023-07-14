using UnityEngine;
using TMPro;

public class EnemyManager : Singleton<EnemyManager>
{
    private TMP_Text totalEnemyText;
    private int currentEnemiesKilled = 0;
    
    const string TOTAL_E_TEXT = "Total Enemies Text";

    public void UpdateTotalEnemies(){
        currentEnemiesKilled++;
        if (totalEnemyText == null){
            totalEnemyText = GameObject.Find(TOTAL_E_TEXT).GetComponent<TMP_Text>();
        }
        totalEnemyText.text = currentEnemiesKilled.ToString();
    }

    public void ResetEnemiesKilled(){
        currentEnemiesKilled = 0;
        
        if (totalEnemyText == null){
            totalEnemyText = GameObject.Find(TOTAL_E_TEXT).GetComponent<TMP_Text>();
        }
        totalEnemyText.text = currentEnemiesKilled.ToString();
    }
}
