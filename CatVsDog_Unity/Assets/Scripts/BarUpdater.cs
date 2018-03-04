using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUpdater : MonoBehaviour {
    private float _currentValue = 0;
    public float currentValue {
        get { return _currentValue; }
        set {
            // Keep currentValue >= 0 and <= maxValue
            _currentValue = Mathf.Max(Mathf.Min(value, _maxValue), 0);
            UpdateBar();
        }
    }
    private float _maxValue = 0;
    public float maxValue {
        get { return _maxValue; }
        set {
            _maxValue = value;
            _currentValue = Mathf.Min(_currentValue, _maxValue); // Keep currentValue within maxValue bounds
        }
    }

    public Text textComp;
    public string prefixStr = "";
    public string suffixStr = "";

    public void UpdateBar() {
        if (_maxValue > 0)
            transform.localScale = new Vector3(_currentValue / _maxValue, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        textComp.text = prefixStr + Mathf.Ceil(_currentValue) + suffixStr;
    }

    // Use this for initialization
    void Start () {
        UpdateBar();
	}
}
