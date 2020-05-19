using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Currency {
    public string[] suifx = new string[] {
        "",
        "k",
        "kk",
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
    };

    public Value add(double totalSouls, int totalSoulsScale, double value, int valueScale) {
        Value valueClass = new Value();

        if (totalSoulsScale != valueScale) {
            Debug.Log("diferente");
            if (totalSoulsScale > valueScale) {
                if (totalSoulsScale > 0) {
                    totalSoulsScale--;
                    totalSouls *= 1000000;
                    valueClass = add(totalSouls, totalSoulsScale, value, valueScale);
                }
            }

            if (totalSoulsScale < valueScale) {
                Debug.Log("Valor que está sendo adicionado no click é maior que o total de almas");
                if (valueScale > 0) {
                    valueScale--;
                    value *= 1000000;
                    valueClass = add(totalSouls, totalSoulsScale, value, valueScale);
                }
            }

            return valueClass;
        } else {
            Debug.Log("Mesmo scale, somando valores aqui");
            totalSouls += value;

            valueClass.value = double.Parse(totalSouls.ToString("N3"));
            valueClass.scale = totalSoulsScale;

            return valueClass;
        }
    }

    public Value subtract(double totalSouls, int totalSoulsScale, double value, int valueScale) {
        Value valueClass = new Value();

        if (totalSoulsScale != valueScale) {
            if (totalSoulsScale > valueScale) {
                if (totalSoulsScale > 0) {
                    totalSoulsScale--;
                    totalSouls *= 1000000;
                    valueClass = subtract(totalSouls, totalSoulsScale, value, valueScale);
                }
            }

            if (totalSoulsScale < valueScale) {
                Debug.Log("Valor negativo aqui (scale do valor é maior que o scale do total de almas)");
                return null;
            }

            return valueClass;
        } else {
            if ((totalSouls - value) < 0) {
                Debug.Log("Valor negativo aqui (scales iguais)");
                return null;
            } else {
                totalSouls -= value;
                valueClass.value = totalSouls;
            }
        }

        return valueClass;
    }
}
