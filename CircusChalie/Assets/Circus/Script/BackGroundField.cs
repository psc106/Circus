using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackGroundField : MonoBehaviour
{
    private TMP_Text tmpText;

    public int currDistance = default;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void ChangeText()
    {
        if (tmpText != null) 
        {
            tmpText.text = $"{currDistance * 10, 3}";
        }
    } 
}
