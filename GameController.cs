using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private float time = 1f;
    public string lastTimeOnline;

    public Value offlineEarnings;
    public GameObject offlineEarningText;
    public GameObject offlineEarningPanel;

    public Currency currency;
    public Souls souls;
    public Click click;
    public Employees[] employees;
    public Text soulsText;
    public GameObject clickTextPrefab;
    public Canvas canvas;

    public bool isShopOpen = false;

    void Start() {
        PlayerData data = SaveController.loadGame();

        offlineEarningText = GameObject.FindGameObjectWithTag("offlineEarningText");
        offlineEarningPanel = GameObject.FindGameObjectWithTag("offlineEarningPanel");
        offlineEarningPanel.SetActive(false);

        if (data == null) {
            foreach (Employees employee in employees) {
                employee.actualProduction.scale = 0;
                employee.nextCost.scale = 0;
                employee.nextProduction.scale = 0;

                employee.actualProduction.value = employee.initialProduction;

                employee.nextCost.value = employee.initialCost;
                employee.nextCost.scale = 0;
            }

            click.actualProduction.value = click.initialProduction;
            click.actualProduction.scale = 0;

            click.nextCost.value = click.initialCost;
            click.nextCost.scale = 0;

            if (click.level == 0) {
                click.nextProduction.value = click.initialProduction;
            } else {
                click.nextProduction.value = double.Parse(getNextProductionRate(click.initialProduction, click.level));
            }
        } else {
            loadGame(data);

            string currentTime = System.DateTime.Now.ToString();
            string diffInSeconds = (System.DateTime.Parse(currentTime) - System.DateTime.Parse(lastTimeOnline)).TotalSeconds.ToString();
            Value actualProduction = getEmployeeTotalProduction();
            offlineEarnings.value = double.Parse(diffInSeconds) * actualProduction.value;
            offlineEarnings.scale = actualProduction.scale;
            offlineEarnings.multiplier = 2;

            while (offlineEarnings.value > 1000000) {
                offlineEarnings.value /= 1000000;
                offlineEarnings.scale++;
            }

            if (offlineEarnings.value > 0) {
                offlineEarningPanel.SetActive(true);
                offlineEarningText.GetComponent<Text>().text = "You earned " + offlineEarnings.value.ToString("N2") + currency.suifx[offlineEarnings.scale] + " souls while offline";
            }
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
        checkForScaleChange();

        click.actualProductionText.text = click.actualProduction.value.ToString("N2");
        click.nextCostText.text = click.nextCost.value.ToString("N2") + currency.suifx[click.nextCost.scale];
        click.nextProductionText.text = "+ " + getNextProductionDiff(click.nextProduction.value, click.actualProduction.value).ToString("N2") + currency.suifx[click.nextProduction.scale];;
        click.playerNameText.text = click.playerName;
        click.levelText.text = "Lv " + click.level.ToString();
        click.descriptionText.text = click.description;

        if (currency.subtract(souls.totalSouls.value, souls.totalSouls.scale, click.nextCost.value, click.nextCost.scale) == null) {
            click.upgradeButton.interactable = false;
        } else {
            click.upgradeButton.interactable = true;
        }

        foreach (Employees employee in employees) {
            employee.actualProductionText.text = employee.actualProduction.value.ToString("N2") + currency.suifx[employee.actualProduction.scale];
            employee.nextCostText.text = employee.nextCost.value.ToString("N2") + currency.suifx[employee.nextCost.scale];
            employee.nextProductionText.text = "+ " + getNextProductionDiff(employee.nextProduction.value, employee.actualProduction.value).ToString("N2") + currency.suifx[employee.nextProduction.scale];
            employee.employeeNameText.text = employee.employeeName;
            employee.employeeLevelText.text = "Lv " + employee.level.ToString();
            employee.employeeDescriptionText.text = employee.description;

            if (currency.subtract(souls.totalSouls.value, souls.totalSouls.scale, employee.nextCost.value, employee.nextCost.scale) == null) {
                employee.upgradeButton.interactable = false;
            } else {
                employee.upgradeButton.interactable = true;
            }

            if (employee.actualProduction.value > 1000000) {
                employee.actualProduction.value /= 1000000;
                employee.actualProduction.scale++;
            }

            if (employee.nextProduction.value > 1000000) {
                employee.nextProduction.value /= 1000000;
                employee.nextProduction.scale++;
            }

            if (employee.level > 0) {
                employee.image.enabled = true;
            }
        }

        time -= Time.deltaTime;
        if (time <= 0) {
            Value totalProduction = getEmployeeTotalProduction();
            Value valueClass = currency.add(souls.totalSouls.value, souls.totalSouls.scale, totalProduction.value, totalProduction.scale);

            if (totalProduction.value > 0 && !isShopOpen) {
                GameObject clickTextPrefabObj = Instantiate(clickTextPrefab, new Vector3(Random.Range(-20f, 100f), Random.Range(0f, 70f), 0f), Quaternion.identity);
                clickTextPrefabObj.transform.SetParent(canvas.transform, false);
                clickTextPrefabObj.GetComponent<Text>().text = "+ " + totalProduction.value.ToString("N2") + currency.suifx[souls.totalSouls.scale];
            }

            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;
            time = 1f;
        }
    }

    void FixedUpdate() {
        saveGame();
    }

    public void setIsShopOpen(bool isOpen) {
        this.isShopOpen = isOpen;
    }

    void debug() {
        if (Input.GetKeyDown(KeyCode.T)) {
            string today = System.DateTime.Now.ToString();
            string tomorrow = System.DateTime.Parse(today).AddDays(1).ToString();

            var diffInSeconds = (System.DateTime.Parse(tomorrow) - System.DateTime.Parse(today)).TotalSeconds;
        }
    }

    // /**
    //  * Handles the button to close the pop up of offline earnings
    //  * If closed, gives the player the offline amount
    //  * If the player watch the ads give the value * 2 (NOT IMPLEMENTED YET)
    //  */
    // public void closeOfflineEarnings() {
    //     Value valueClass = new Value();

    //     valueClass = currency.add(souls.totalSouls.value, souls.totalSouls.scale, offlineEarnings.value, offlineEarnings.scale);

    //     souls.totalSouls.value = valueClass.value;
    //     souls.totalSouls.scale = valueClass.scale;

    //     offlineEarningPanel.SetActive(false);
    // }

    public void addOfflineEarnings() {
        Value valueClass = new Value();

        valueClass = currency.add(souls.totalSouls.value, souls.totalSouls.scale, offlineEarnings.value, offlineEarnings.scale);

        souls.totalSouls.value = valueClass.value;
        souls.totalSouls.scale = valueClass.scale;

        offlineEarningPanel.SetActive(false);
    }

    public void doubleOfflineEarnings() {
        Value valueClass = new Value();

        valueClass = currency.multiply(offlineEarnings.value, offlineEarnings.scale, 2);

        offlineEarnings.value = valueClass.value;
        offlineEarnings.scale = valueClass.scale;
    }

    public void offlineEarningButton(bool reward = false) {
        if (reward) {
            doubleOfflineEarnings();
        }

        addOfflineEarnings();
    }

    /**
     * Run through the employees array and return the sum of all employees actual production values
     */
    public Value getEmployeeTotalProduction() {
        Value valueClass = new Value();

        foreach (Employees employee in employees) {
            if (employee.level > 0) {
                valueClass.value += employee.actualProduction.value;
                valueClass.scale = 0;
            }
        }

        return valueClass;
    }

    /**
     * Send the game data to be saved in a binary file
     */
    public void saveGame() {
        SaveController.saveGame(this);
    }

    /**
     * Assign the data from the last save file to the game
     */
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

        lastTimeOnline = data.lastTimeOnline;

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
     * @return Value nextCost
     */
    private Value getNextUpgradeCost(double initialCost, double growthRate, int level, int scale) {
        Value nextCost = new Value();
        nextCost.scale = 0;

        double nextCostValue = initialCost * (Mathf.Pow((float)growthRate, level));
        nextCost.value = nextCostValue;

        if (nextCost.value > 1000000) {
            Debug.Log("Subiu scale do próximo custo");
            nextCost.value /= 1000000;
            nextCost.scale++;
        }

        return nextCost;
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
            // Debug.Log("Valor negativo aqui (valor de custo do próximo upgrade é muito caro)");
        } else {
            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;

            employee.actualProduction.value = employee.nextProduction.value;
            employee.level++;

            employee.nextProduction.value = double.Parse(getNextProductionRate(employee.initialProduction, employee.level));

            Value employeeNextCost = new Value();
            employeeNextCost = getNextUpgradeCost(employee.initialCost, employee.growthRate, employee.level, employee.nextCost.scale);

            employee.nextCost.value = employeeNextCost.value;
            employee.nextCost.scale = employeeNextCost.scale;
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
            // Debug.Log("Valor negativo aqui (valor de custo do próximo upgrade é muito caro)");
        } else {
            souls.totalSouls.value = valueClass.value;
            souls.totalSouls.scale = valueClass.scale;

            click.actualProduction.value = click.nextProduction.value;
            click.level++;

            click.nextProduction.value = double.Parse(getNextProductionRate(click.initialProduction, click.level));

            Value clickNextCost = new Value();
            clickNextCost = getNextUpgradeCost(click.initialCost, click.growthRate, click.level, click.nextCost.scale);

            click.nextCost.value = clickNextCost.value;
            click.nextCost.scale = clickNextCost.scale;

            Debug.Log("clickNextCost.value: " + clickNextCost.value);
            Debug.Log("clickNextCost.scale: " + clickNextCost.scale);
        }
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

    /**
     * Handles the transition in scale of values
     */
    public void checkForScaleChange() {
        if (souls.totalSouls.value > 1000000) {
            souls.totalSouls.value /= 1000000;
            souls.totalSouls.scale++;
        }

        if (click.actualProduction.value > 1000000) {
            click.actualProduction.value /= 1000000;
            click.actualProduction.scale++;
        }

        if (click.nextProduction.value > 1000000) {
            click.nextProduction.value /= 1000000;
            click.nextProduction.scale++;
        }

        soulsText.text = "Souls: " + souls.totalSouls.value.ToString("N2") + currency.suifx[souls.totalSouls.scale];
    }
}
