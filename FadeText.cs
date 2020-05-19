using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour {
    void Update() {
        StartCoroutine(FadeTextToZeroAlpha(1.5f, GetComponent<Text>()));
        StartCoroutine(waitAndDestroyText());
    }

    public IEnumerator FadeTextToZeroAlpha(float time, Text text) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));

            yield return null;
        }
    }

    public IEnumerator waitAndDestroyText() {
        yield return new WaitForSeconds(1.4f);
        Destroy(this.gameObject);
    }
}
