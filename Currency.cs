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

    /**
     * Handles the sum of two values, uses the scale to make the sum
     *
     * @param double totalSouls    First value, this will receive the second parameter value
     * @param int totalSoulsScale  First value scale, used to make the sum
     * @param double value         Second value, this will be added to the first value
     * @param int valueScale       Second value scale, used to make the sum
     *
     * @return Value valueClass    Return a instance of valueClass with the sum result of the two values
     */
    public Value add(double totalSouls, int totalSoulsScale, double value, int valueScale) {
        Value valueClass = new Value();

        if (totalSoulsScale != valueScale) {
            int scaleDiff = totalSoulsScale - valueScale;

            if (scaleDiff < -2 || scaleDiff > 2) {
                Debug.Log("Diferença de scale é maior que 2");

                if (scaleDiff > 0) {
                    Debug.Log("Faz de conta que somou");
                    valueClass.value = double.Parse(totalSouls.ToString("N3"));
                    valueClass.scale = totalSoulsScale;

                    return valueClass;
                } else {
                    Debug.Log("Considerando o adicionado como total");
                    valueClass.value = value;
                    valueClass.scale = valueScale;

                    return valueClass;
                }
            }

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

    /**
     * Handles the subtraction of two values, uses the scale to make the subtraction
     *
     * @param double totalSouls    First value, this will receive the second parameter value
     * @param int totalSoulsScale  First value scale, used to make the subtraction
     * @param double value         Second value, this will be added to the first value
     * @param int valueScale       Second value scale, used to make the subtraction
     *
     * @return Value valueClass    Return a instance of valueClass with the subtraction result of the two values
     * @return Value null          Return null if the subtraction result is lower than 0
     */
    public Value subtract(double totalSouls, int totalSoulsScale, double value, int valueScale) {
        Value valueClass = new Value();

        if (totalSoulsScale != valueScale) {
            Debug.Log("Passou aqui");
            int scaleDiff = totalSoulsScale - valueScale;

            if (scaleDiff < -2 || scaleDiff > 2) {
                Debug.Log("Diferença de scale é maior que 2");

                if (scaleDiff > 0) {
                    Debug.Log("Faz de conta que subtraiu");
                    valueClass.value = double.Parse(totalSouls.ToString("N3"));
                    valueClass.scale = totalSoulsScale;

                    return valueClass;
                }
            }

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
            Debug.Log("Scales iguais");
            if ((totalSouls - value) < 0) {
                Debug.Log("Valor negativo aqui (scales iguais)");
                return null;
            } else {
                Debug.Log("Entrou aqui");
                totalSouls -= value;
                valueClass.value = totalSouls;
                valueClass.scale = totalSoulsScale;
            }

            return valueClass;
        }
    }

    public Value multiply(double value, int scale, double multiplier) {
        Value valueClass = new Value();

        value *= multiplier;

        while (value > 1000000) {
            value /= 1000000;
            scale++;
        }

        valueClass.value = value;
        valueClass.scale = scale;

        return valueClass;
    }
}
