using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUpdater : MonoBehaviour {
    private float _currentValue = 0;
    public float currentValue {
        get { return _currentValue; }
        set {
            _currentValue = value;
            UpdateBar();
        }
    }
    private float _maxValue = 0;
    public float maxValue {
        get { return _maxValue; }
        set {
            _maxValue = value;
            UpdateBar();
        }
    }

    public Text textComp;
    public string prefixStr = "";
    public string suffixStr = "";

    public void UpdateBar() {
        if (_maxValue > 0)
            transform.localScale = new Vector3(Mathf.Clamp01(_currentValue / _maxValue), transform.localScale.y, transform.localScale.z);
//        else
//            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        if (textComp != null)
            textComp.text = prefixStr + Mathf.Ceil(_currentValue) + suffixStr;
    }

    // Use this for initialization
    void Start () {
        UpdateBar();
	}
}
