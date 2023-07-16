using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text recordText;
    public TMP_Text stageText;

    public TMP_Text bonusText;
    
    public GameObject gameoverUI;
    public TMP_Text endText;

    public GameObject inputUI;

    private float bonusDelay = default;

    public List<Image> Lifes;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        bonusDelay = 0;

        WriteBonus();
        WritScore();
        WriteStage();
        WriteRecord();
        //StartCoroutine(Bonus());
    }
    private void Update()
    {
        GameManager manager = GameManager.instance;
        if (!manager.isGameover && !manager.isStageClear && !manager.isLoading)
        {
            if (manager.bonus > 0)
            {
                bonusDelay += Time.deltaTime;
                if (bonusDelay > .2f)
                {
                    bonusDelay = 0;
                    manager.bonus -= 10;
                    WriteBonus();

                    if (manager.bonus == 0)
                    {
                        manager.OnPlayerDead();
                    }
                }
            }
        }

    }
    IEnumerator Bonus()
    {
        yield return new WaitForSeconds(.1f);
        GameManager.instance.bonus -= 10;
//        bonusText.text = $"BONUS-{bonus:D6}";
    }

    public void WriteBonus()
    {
        bonusText.text = $"BONUS-{GameManager.instance.bonus:D4}";
    }
    public void WritScore()
    {
        scoreText.text = $"SCORE-{GameManager.instance.score:D6}";
    }
    public void WriteStage()
    {
        stageText.text = $"STAGE-{GameManager.instance.stage:D2}";
    }
    public void WriteRecord()
    {
        recordText.text = $"BEST-{GameManager.instance.record:D6}";
    }


    public void WriteFail()
    {
        endText.text = $"STAGE {GameManager.instance.stage:D2}";
    }
    public void WriteClear()
    {
        endText.text = "Stage Clear";
    }
    public void WriteGameover()
    {
        endText.text = "Game Over";
    }


    public void WriteBonus(int num)
    {
        bonusText.text = $"BONUS-{num:D4}";
    }
    public void WritScore(int num)
    {
        scoreText.text = $"SCORE-{num:D6}";
    }
    public void WriteStage(int num)
    {
        stageText.text = $"STAGE-{num:D2}";
    }
    public void WriteRecord(int num)
    {
        recordText.text = $"BEST-{num:D6}";
    }
}
