using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Souls {
    public Value totalSouls; // Holds the total souls value and its scale

    // public Value addToTotalSouls(double totalSouls, int totalSoulsScale, double value, int valueScale) {
    //     Value valueClass = new Value();

    //     if (totalSoulsScale != valueScale) {
    //         Debug.Log("diferente");
    //         if (totalSoulsScale > valueScale) {
    //             Debug.Log("Quantidade de almas totais é maior que o valor adicionado no clique");
    //             Debug.Log("totalSouls: " + totalSouls);
    //             Debug.Log("totalSoulsScale: " + totalSoulsScale);
    //             Debug.Log("value: " + value);
    //             Debug.Log("valueScale: " + valueScale);

    //             if (totalSoulsScale > 0) {
    //                 totalSoulsScale--;
    //                 totalSouls *= 1000000;
    //                 valueClass = addToTotalSouls(totalSouls, totalSoulsScale, value, valueScale);
    //             }
    //         }

    //         if (totalSoulsScale < valueScale) {
    //             Debug.Log("Valor que está sendo adicionado no click é maior que o total de almas");
    //             if (valueScale > 0) {
    //                 valueScale--;
    //                 value *= 1000000;
    //                 valueClass = addToTotalSouls(totalSouls, totalSoulsScale, value, valueScale);
    //             }
    //         }

    //         Debug.Log("Caiu aqui e não deveria");
    //         Debug.Log("valueClass.value: " + valueClass.value);
    //         Debug.Log("valueClass.scale: " + valueClass.scale);
    //         return valueClass;
    //     } else {
    //         Debug.Log("Mesmo scale");
    //         totalSouls += value;

    //         valueClass.value = double.Parse(totalSouls.ToString("N3"));
    //         valueClass.scale = totalSoulsScale;

    //         return valueClass;
    //     }
    // }

    // public Value subtract(double totalSouls, int totalSoulsScale, double value, int valueScale) {
    //     Value valueClass = new Value();

    //     if (totalSoulsScale != valueScale) {
    //         if (totalSoulsScale > valueScale) {
    //             if (totalSoulsScale > 0) {
    //                 totalSoulsScale--;
    //                 totalSouls *= 1000000;
    //                 valueClass = subtract(totalSouls, totalSoulsScale, value, valueScale);
    //             }
    //         }

    //         if (totalSoulsScale < valueScale) {
    //             Debug.Log("Valor negativo aqui (scale do valor é maior que o scale do total de almas)");
    //             return null;
    //         }

    //         return valueClass;
    //     } else {
    //         if ((totalSouls - value) < 0) {
    //             Debug.Log("Valor negativo aqui (scales iguais)");
    //             return null;
    //         } else {
    //             totalSouls -= value;
    //             valueClass.value = totalSouls;
    //         }
    //     }

    //     return valueClass;
    // }
}
