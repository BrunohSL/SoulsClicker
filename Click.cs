using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Click {
    public string playerName;
    public string description;
    public int level;                      // Actual level

    public double initialProduction;       // Production value of the first upgrade
    public double initialCost;             // First value after buying
    public double growthRate;              // Rate of growth, used to calculate the cost of the next upgrde

    public Value actualProduction;         // Holds the actual production value and its scale
    public Value nextProduction;           // Holds the next production value and its scale
    public Value nextCost;                 // Holds the next cost value and its scale

    public Button upgradeButton;

    public Text actualProductionText;
    public Text nextCostText;
    public Text nextProductionText;
    public Text playerNameText;
    public Text levelText;
    public Text descriptionText;

    public Image image;
}
