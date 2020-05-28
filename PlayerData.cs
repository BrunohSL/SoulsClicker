using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public double totalSoulsValue;
    public int totalSoulsScale;

    public string lastTimeOnline;

    public int click_level;
    public double click_initialProduction;
    public double click_initialCost;
    public double click_growthRate;
    public double click_actualProductionValue;
    public int click_actualProductionScale;
    public double click_nextProductionValue;
    public int click_nextProductionScale;
    public double click_nextCostValue;
    public int click_nextCostScale;

    public int[] employee_level = new int[5];
    public double[] employee_initialProduction = new double[5];
    public double[] employee_initialCost = new double[5];
    public double[] employee_growthRate = new double[5];
    public double[] employee_actualProductionValue = new double[5];
    public int[] employee_actualProductionScale = new int[5];
    public double[] employee_nextProductionValue = new double[5];
    public int[] employee_nextProductionScale = new int[5];
    public double[] employee_nextCostValue = new double[5];
    public int[] employee_nextCostScale = new int[5];

    public PlayerData(GameController gameController) {
        totalSoulsValue = gameController.souls.totalSouls.value;
        totalSoulsScale = gameController.souls.totalSouls.scale;

        click_level = gameController.click.level;
        click_initialProduction = gameController.click.initialProduction;
        click_initialCost = gameController.click.initialCost;
        click_growthRate = gameController.click.growthRate;
        click_actualProductionValue = gameController.click.actualProduction.value;
        click_actualProductionScale = gameController.click.actualProduction.scale;
        click_nextProductionValue = gameController.click.nextProduction.value;
        click_nextProductionScale = gameController.click.nextProduction.scale;
        click_nextCostValue = gameController.click.nextCost.value;
        click_nextCostScale = gameController.click.nextCost.scale;

        lastTimeOnline = System.DateTime.Now.ToString();

        int counter = 0;

        foreach (Employees employee in gameController.employees) {
            employee_level[counter] = employee.level;
            employee_initialProduction[counter] = employee.initialProduction;
            employee_initialCost[counter] = employee.initialCost;
            employee_growthRate[counter] = employee.growthRate;
            employee_actualProductionValue[counter] = employee.actualProduction.value;
            employee_actualProductionScale[counter] = employee.actualProduction.scale;
            employee_nextProductionValue[counter] = employee.nextProduction.value;
            employee_nextProductionScale[counter] = employee.nextProduction.scale;
            employee_nextCostValue[counter] = employee.nextCost.value;
            employee_nextCostScale[counter] = employee.nextCost.scale;
            counter++;
        }
    }
}
