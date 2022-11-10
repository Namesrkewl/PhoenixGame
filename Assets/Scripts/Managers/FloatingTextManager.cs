using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        foreach (FloatingText txt in floatingTexts) {
            txt.UpdateFloatingText();
        }
    }

    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    // Changes the features of the text
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); // Transfer world space to screen space for use in the UI
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }

    // Returns floating text if available, makes a new one if unavailable
    private FloatingText GetFloatingText() {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null) {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}