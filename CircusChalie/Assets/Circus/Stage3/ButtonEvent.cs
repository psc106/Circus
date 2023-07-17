using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public TMP_Text scroe;
    public TMP_Text record;
    public TMP_Text newBest;

    private void Start()
    {
        int bestRecord = PlayerPrefs.GetInt("BestScore");
        if (GameManager.instance.score > bestRecord)
        {
            bestRecord = GameManager.instance.score;
        }

        PlayerPrefs.SetInt("BestScore", bestRecord);
        if (bestRecord == GameManager.instance.score)
        {
            newBest.enabled = true;
        }   
        
        record.text = $"Best score  : {bestRecord:D6}";
        
        scroe.text = $"Final score : {GameManager.instance.score:D6}";
    }


    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.instance.ReturnTitle();
        }
    }
}
