using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    // Variables governing features of the text
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    // Lets text be accessed
    public void Show() {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    // Hides text 
    public void Hide() {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText() {
        if (!active) {
            return;
        }

        if ((Time.time - lastShown) > duration) {
            Hide();
        }

        go.transform.position += motion * Time.deltaTime;
    }
}
