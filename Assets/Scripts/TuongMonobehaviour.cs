using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TuongMonobehaviour : MonoBehaviour
{
    private void Start()
    {
        this.LoadComponents();
    }
    private void Reset()
    {
        this.LoadComponents();
    }
    protected virtual void LoadComponents()
    {

    }
    protected virtual TextMeshProUGUI LoadTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI, string name)
    {
        if (textMeshProUGUI != null) return textMeshProUGUI;
        return textMeshProUGUI = GameObject.Find(name).GetComponent<TextMeshProUGUI>();
    }
}
