using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : Menu {
    public TextAsset creditsFile;
    public float paddingBottom = 0;
    public TMP_Text textUI;
    public RectTransform textContainer;
    
    // Function runs when object is spawned, even when it's disabled
    override protected void Awake() {
        base.Awake();
        // Load Credits text
        textUI.SetText(creditsFile.text);
        // Set container's height to text's actual height
        // Source: TMP_Example Script 01
        Vector2 textSize = textUI.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);
        textContainer.sizeDelta = new Vector2(textContainer.sizeDelta.x, textSize.y + paddingBottom);
    }
}
