using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Total Gold Text";


    // CALL EVERYTIME COIN IS ACQUIRED IN PICKUP.CS
    public void UpdateCurrentGold(){

        currentGold += 1;

        if (goldText == null){
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3"); // D3 is an overload that requires the string to have at least 3 characters

    }
}
