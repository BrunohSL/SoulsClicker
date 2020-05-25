using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private double multipliers;
    private float time = 1f;
    // public Value totalProduction;

    public Currency currency;

    public Souls souls;
    public Click click;
    public Employees[] employees;
    public Text soulsText;
    public GameObject clickTextPrefab;
    public Canvas canvas;

    void Start() {
        PlayerData data = SaveController.loadGame();

        if (data == null) {
            foreach (Employees employee in employees) {
                employee.actualProduction.scale = 0;
                employee.nextCost.scale = 0;
                employee.nextProduction.scale = 0;

                employee.actualProduction.value = employee.initialProduction;
                employee.nextCost.value = double.Parse(getNextUpgradeCost(employee.initialCost, employee.growthRate, employee.level));
            }

            click.actualProduction.value = click.initialProduction;
            click.actualProduction.scale = 0;
            click.nextCost.value = double.Parse(getNextUpgradeCost(click.initialCost, click.growthRate, click.level));
            if (click.level == 0) {
                click.nextProduction.value = click.initialProduction;
            } else {
                click.nextProduction.value = double.Parse(getNextProductionRate(click.initialProduction, click.level));
            }
        } else {
            loadGame(data);
        }

        foreach (Employees employee in employees) {
            if (employee.level == 0) {
                employee.nextProduction.value = employee.initialProduction;
                employee.image.enabled = false;
            } else {
                employee.nextProduction.value = double.Parse(getNextProductionRate(employee.initialProduction, employee.level));
                employee.image.enabled = true;
            }
        }
    }

    void Update() {
        soulsText.text = "Souls: " + souls.totalSouls.value.ToString("N2") + currency.suifx[souls.totalSouls.scale];

        checkForScaleChange();

        click.actualProductionText.text = click.actualProduction.value.ToString("N2");
        click.nextCostText.text = click.nextCost.value.ToString("N2") + currency.suifx[click.nextCost.scale];
        click.nextProductionText.text = "+ " + getNextProductionDiff(click.nextProduction.value, click.actualProduction.value).ToString("N2") + currency.suifx[click.nextProduction.scale];;
        click.playerNameText.text = click.playerName;
        click.levelText.text = "Lv " + click.level.ToString();
        click.descriptionText.text = click.description;

        foreach (Employees employee in employees) {
            employee.actualProductionText.text = employee.actualProduction.value.ToString("N2") + currency.suifx[employee.actualProduction.scale];
            employee.nextCostText.text = employee.nextCost.value.ToString("N2") + currency.suifx[employee.nextCost.scale];
            employee.nextProductionText.text = "+ " + getNextProductionDiff(employee.nextProduction.value, employee.actualProduction.value).ToString("N2") + currency.suifx[employee.nextProduction.scale];
            employee.employeeNameText.text = employee.employeeName;
            employee.employeeLevelText.text = "Lv " + employee.level.ToString();
            employee.employeeDescriptionText.text = employee.description;

            if (employee.nextCost.value > 1000000) {
                Debug.Log("employee.nextCost Scale up");
                employee.nextCost.value /= 1000000;
                employee.nextCost.scale++;
            }

            if (employee.actualProduction.value > 1000000) {
                Debug.Log("employee.actualProduction Scale up");
                employee.actualProduction.value /= 1000000;
                employee.actualProduction.scale++;
            }

            if (employee.nextProduction.value > 1000000) {
                Debug.Log("employee.nextProduction Scale up");
                employee.nextProduction.value /= 1000000;
                employee.nextProduction.scale++;
            }

            if (employee.level > 0) {
                employee.image.enabled = true;
            }
        }

        // time -= Time.deltaTime;
        if (time <= 0) {
            Debug.Log("Gerou alma pelo funcionário");
            Value totalProduction = getEmployeeTotalProduction();
            // Debug.Log(totalProduction.value);
            // Debug.Log(totalProduction.scale);
            Value valueClass = currency.add(souls.totalSouls.value, souls.totalSouls.scale, totalProduction.value, totalProduction.scale);

            if (totalProduction.value > 0) {
                GameObject clickTextPrefabObj = Instantiate(clickTextPrefab, new Vector3(Random.Range(-20f, 100f), Random.Range(0f, 70f), 0f), Quaternion.identity);
                clickTextPrefabObj.transform.SetParent(canvas.transform, false);
                clickTextPrefabObj.GetComponent<Text>().text = "+ " + totalProduction.value.ToString("N2") + currency.suifx[souls.totalSouls.scale];
            }

            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;
            time = 1f;
        }

        // if (Input.GetKeyDown(KeyCode.T)) {
        //     time = 0;
        // }
    }

    void FixedUpdate() {
        saveGame();
    }

    public Value getEmployeeTotalProduction() {
        Value valueClass = new Value();

        // totalProduction.value = 0;
        // totalProduction.scale = 0;

        foreach (Employees employee in employees) {
            if (employee.level > 0) {
                valueClass.value += employee.actualProduction.value;
                valueClass.scale = 0;
            }
        }

        return valueClass;
    }

    public void saveGame() {
        SaveController.saveGame(this);
    }

    public void loadGame(PlayerData data) {
        souls.totalSouls.value = data.totalSoulsValue;
        souls.totalSouls.scale = data.totalSoulsScale;

        click.level = data.click_level;
        click.initialProduction = data.click_initialProduction;
        click.initialCost = data.click_initialCost;
        click.growthRate = data.click_growthRate;
        click.actualProduction.value = data.click_actualProductionValue;
        click.actualProduction.scale = data.click_actualProductionScale;
        click.nextProduction.value = data.click_nextProductionValue;
        click.nextProduction.scale = data.click_nextProductionScale;
        click.nextCost.value = data.click_nextCostValue;
        click.nextCost.scale = data.click_nextCostScale;

        int counter = 0;
        foreach (var employee in employees) {
            employee.level = data.employee_level[counter];
            employee.initialProduction = data.employee_initialProduction[counter];
            employee.initialCost = data.employee_initialCost[counter];
            employee.growthRate = data.employee_growthRate[counter];
            employee.actualProduction.value = data.employee_actualProductionValue[counter];
            employee.actualProduction.scale = data.employee_actualProductionScale[counter];
            employee.nextProduction.value = data.employee_nextProductionValue[counter];
            employee.nextProduction.scale = data.employee_nextProductionScale[counter];
            employee.nextCost.value = data.employee_nextCostValue[counter];
            employee.nextCost.scale = data.employee_nextCostScale[counter];
            counter++;
        }
    }

    /**
     * Gets the difference of nextProduction and actualProduction to show how much it will increase after the upgrade
     *
     * @param double nextProduction
     * @param double actualProduction
     *
     * @return double diff
     */
    public double getNextProductionDiff(double nextProduction, double actualProduction) {
        double diff = nextProduction - actualProduction;

        return diff;
    }

    /**
     * Generate the next upgrade cost of click or employee
     *
     * @param double initialCost original cost of click or employee, the initial value
     * @param double growthRate  the coefficient used to increase the next upgrade value for each level up
     * @param double level       click level or employee level
     *
     * @return string nextCost
     */
    private string getNextUpgradeCost(double initialCost, double growthRate, int level) {
        double nextCost = initialCost * (Mathf.Pow((float)growthRate, level));

        return nextCost.ToString("N2");
    }

    /**
     * Generate the next upgrade cost of click or employee
     *
     * @param double initialProduction production original value, the initial value
     * @param double level             click level or employee level
     * @param double multiplier        value of all bonuses that increases soul generation
     *
     * @return string nextProductionRate
     */
    private string getNextProductionRate(double initialProduction, int level, double multiplier = 1) {
        double nextProductionRate = (initialProduction * level) * multiplier;

        return nextProductionRate.ToString("N2");
    }

    /**
     * Handles the click to generate more souls
     */
    public void buttonClick() {
        Value valueClass = currency.add(souls.totalSouls.value, souls.totalSouls.scale, click.actualProduction.value, click.actualProduction.scale);

        souls.totalSouls.value = valueClass.value;
        souls.totalSouls.scale = valueClass.scale;

        GameObject clickTextPrefabObj = Instantiate(clickTextPrefab, new Vector3(Random.Range(-20f, 100f), Random.Range(0f, 70f), 0f), Quaternion.identity);
        clickTextPrefabObj.transform.SetParent(canvas.transform, false);
        clickTextPrefabObj.GetComponent<Text>().text = "+ " + click.actualProduction.value.ToString("N2") + currency.suifx[click.actualProduction.scale];
    }

    /**
     * Upgrade employee button behavior
     *
     * @param string employeeName Name of the employee that will level up
     *
     * First check if the player can buy the next upgrade,
     * Then reduces the cost of the upgrade from your number of souls,
     * Then set the actual production as the next production value
     * Then increase the player level by one
     * Then generates the new next production value
     * Then generates the new next cost for the upgrade
     */
    public void levelUpEmplyee(string employeeName) {
        Employees employee = getEmployeeByName(employeeName);
        Value valueClass = new Value();

        valueClass = currency.subtract(souls.totalSouls.value, souls.totalSouls.scale, employee.nextCost.value, employee.nextCost.scale);

        if (valueClass == null) {
            Debug.Log("Valor negativo aqui (valor de custo do próximo upgrade é muito caro)");
        } else {
            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;

            employee.actualProduction.value = employee.nextProduction.value;
            employee.level++;

            employee.nextProduction.value = double.Parse(getNextProductionRate(employee.initialProduction, employee.level));
            employee.nextCost.value = double.Parse(getNextUpgradeCost(employee.initialCost, employee.growthRate, employee.level));
        }
    }

    /**
     * Upgrade click button behavior
     *
     * First check if the player can buy the next upgrade,
     * Then reduces the cost of the upgrade from your number of souls,
     * Then set the actual production as the next production value
     * Then increase the player level by one
     * Then generates the new next production value
     * Then generates the new next cost for the upgrade
     */
    public void levelUpClick() {
        Value valueClass = new Value();

        valueClass = currency.subtract(souls.totalSouls.value, souls.totalSouls.scale, click.nextCost.value, click.nextCost.scale);

        if (valueClass == null) {
            Debug.Log("Valor negativo aqui (valor de custo do próximo upgrade é muito caro)");
        } else {
            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;

            click.actualProduction.value = click.nextProduction.value;
            click.level++;

            click.nextProduction.value = double.Parse(getNextProductionRate(click.initialProduction, click.level));
            click.nextCost.value = double.Parse(getNextUpgradeCost(click.initialCost, click.growthRate, click.level));
        }

        Debug.Log("souls.totalSouls.value: " + souls.totalSouls.value);
        Debug.Log("souls.totalSouls.scale: " + souls.totalSouls.scale);
    }

    /**
     * Search on the employees vector for a match with the name passed
     *
     * @param strint employeeName Name to look for
     *
     * @return Employee employee or null if nothing found
     */
    private Employees getEmployeeByName(string employeeName) {
        foreach (Employees employee in employees) {
            if (employee.employeeName == employeeName) {
                return employee;
            }
        }

        return null;
    }

    public void checkForScaleChange() {
        if (souls.totalSouls.value > 1000000) {
            Debug.Log("souls.totalSouls Scale up");
            souls.totalSouls.value /= 1000000;
            souls.totalSouls.scale++;
        }

        if (click.actualProduction.value > 1000000) {
            Debug.Log("click.actualProduction Scale up");
            click.actualProduction.value /= 1000000;
            click.actualProduction.scale++;
        }

        if (click.nextProduction.value > 1000000) {
            Debug.Log("click.nextCost Scale up");
            click.nextProduction.value /= 1000000;
            click.nextProduction.scale++;
        }

        if (click.nextCost.value > 1000000) {
            Debug.Log("click.nextCost Scale up");
            click.nextCost.value /= 1000000;
            click.nextCost.scale++;
        }

        if (click.nextCost.value > 1000000) {
            Debug.Log("click.nextCost Scale up");
            click.nextCost.value /= 1000000;
            click.nextCost.scale++;
        }
    }
}
