using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestText : MonoBehaviour
{
    public static TMPro.TMP_Text textTMP;
    public static string text = "null";

    private void Start()
    {
        textTMP = GetComponent<TMPro.TMP_Text>();
    }
    private void Update()
    {
        textTMP.text = text;
    }
}
