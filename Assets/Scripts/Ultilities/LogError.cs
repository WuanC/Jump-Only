using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogError : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public void settext(string text)
    {
        this.text.text = text;
    }
}
