using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    public int score = default;
    public int record = default;
    public int bonus = default;
    public int stage = default;

    public float savePoint = 0;

    public static GameManager instance;

    public bool isGameover = default;
    public bool isStageClear = default;
    public bool isLoading = default;
    public bool isJoystickActivate = default;

    public UI sceneUI = default;
    public int life;

    AudioSource audioSource;
    public AudioClip complete;
    public AudioClip mistake;
    public AudioClip newRecord;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance.isValid())
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            isJoystickActivate = false;
            score = 0;
            record = 20000;
            bonus = 5000;

            int bestRecord = PlayerPrefs.GetInt("BestScore");
            if (record < bestRecord)
            {
                record = bestRecord;
            }
        }
        else
        {
            GlobalFunction.Log("게임 매니저 2개 생겨서 종료");
            Destroy(gameObject);
        }


    }
    private void Update()
    {
        GlobalFunction.Log(life);

        if (sceneUI == null)
        {
            sceneUI = FindAnyObjectByType<UI>();
        }

        if ((stage == 1 || stage == 2) && sceneUI != null)
        {
            if (life == 3)
            {
                sceneUI.Lifes[3].enabled = false;
            }
            else if (life == 2)
            {
                sceneUI.Lifes[3].enabled = false;
                sceneUI.Lifes[2].enabled = false;
            }
            else if (life == 1)
            {
                sceneUI.Lifes[3].enabled = false;
                sceneUI.Lifes[2].enabled = false;
                sceneUI.Lifes[1].enabled = false;
            }
            else if (life == 0)
            {
                sceneUI.Lifes[3].enabled = false;
                sceneUI.Lifes[2].enabled = false;
                sceneUI.Lifes[1].enabled = false;
                sceneUI.Lifes[0].enabled = false;
            }
        }
    }

    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            if (record <= score)
            {
                score += newScore;
                record = score;
                sceneUI.WritScore(score);
                sceneUI.WriteRecord(record);
            }
            else
            {
                score += newScore;
                sceneUI.WritScore(score);
                if (record < score)
                {
                    audioSource.PlayOneShot(newRecord);
                    record = score;
                    sceneUI.WriteRecord(record);
                }
            }
        }
    }

    public void OnPlayerDead()
    {
        sceneUI.inputUI.SetActive(false);

        sceneUI.audioSource.Stop();
        audioSource.PlayOneShot(mistake);

        isGameover = true;
        StartCoroutine(PlayGameover());
    }

    public void OnStagedClear()
    {
        sceneUI.inputUI.SetActive(false);

        sceneUI.audioSource.Stop();
        audioSource.PlayOneShot(complete);

        AddScore(bonus);

        bonus = 0;
        sceneUI.WriteBonus();


        isStageClear = true;
        StartCoroutine(PlayClear());
    }

    public void OnStagedFail(Player player)
    {
        sceneUI.inputUI.SetActive(false);

        sceneUI.audioSource.Stop();
        audioSource.PlayOneShot(mistake);

        StartCoroutine(PlayStage1Restart(player));
    }

    public void OnStaged2Fail(PlayerSt2 playerSt2)
    {
        sceneUI.inputUI.SetActive(false);

        sceneUI.audioSource.Stop();
        audioSource.PlayOneShot(mistake);

        StartCoroutine(PlayStage2Restart(playerSt2));
    }


    public void ReturnTitle()
    {
        Destroy(instance.gameObject);
        SceneManager.LoadScene("Title");
    }

    public void PlayLoading(int type)
    {
        switch (type)
        {
            case 0:
                sceneUI.WriteGameover();
                break;
            case 1:
                sceneUI.WriteClear();
                break;
            case 2:
                sceneUI.WriteFail();
                break;
            default:
                break;
        }
        sceneUI.gameoverUI.SetActive(true);
    }

    IEnumerator PlayGameover()
    {
        yield return new WaitForSeconds(1.5f);

        PlayLoading(0);

        yield return new WaitForSeconds(1.5f);
        isGameover = false;
        ReturnTitle();
    }

    IEnumerator PlayClear()
    {
        yield return new WaitForSeconds(1.5f);

        PlayLoading(1);

        yield return new WaitForSeconds(1.5f);

        bonus = 5000;
        score += 10000;
        stage += 1;
        savePoint = 0;
        sceneUI = null;
        isStageClear = false;
        isJoystickActivate = false;
        SceneManager.LoadScene($"Stage{stage:D2}");
    }

    IEnumerator PlayStage1Restart(Player player)
    {
        isLoading = true;
        yield return new WaitForSeconds(1.5f);
        PlayLoading(2);
        yield return new WaitForSeconds(.5f);
        life -= 1;
        sceneUI.Lifes[life].enabled = false;

        Animator[] tmp = player.GetComponentsInChildren<Animator>();
        tmp[0].SetTrigger("Start");
        tmp[1].SetTrigger("Start");

        player.transform.position = new Vector2(7 + 16 * savePoint, -5.28f);
        player.GetComponent<Rigidbody2D>().gravityScale = 2;

        yield return new WaitForSeconds(1f);

        player.isLive = true;

        bonus = 5000;
        isLoading = false;
        sceneUI.gameoverUI.SetActive(false);
        sceneUI.audioSource.Play();
        sceneUI.inputUI.SetActive(true);
    }

    IEnumerator PlayStage2Restart(PlayerSt2 player)
    {
        isLoading = true;
        yield return new WaitForSeconds(1.5f);

        PlayLoading(2);
        yield return new WaitForSeconds(.5f);

        life -= 1;
        sceneUI.Lifes[life].enabled = false;

        Animator tmp = player.GetComponent<Animator>();
        tmp.SetTrigger("Start");

        player.transform.position = new Vector2(1 + 16 * savePoint, 1f);
        player.GetComponent<Rigidbody2D>().gravityScale = 2;

        yield return new WaitForSeconds(1f);

        player.isLive = true;

        bonus = 5000;
        isLoading = false;
        sceneUI.gameoverUI.SetActive(false);
        sceneUI.audioSource.Play();
        sceneUI.inputUI.SetActive(true);
    }
}