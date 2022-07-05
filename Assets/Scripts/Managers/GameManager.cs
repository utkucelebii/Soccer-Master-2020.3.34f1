using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int currentLevel;
    public int currency;

    public void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        currency = PlayerPrefs.GetInt("Currency");
    }


    public void LevelUp()
    {
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.SetInt("Currency", currency + 200);
    }
}
