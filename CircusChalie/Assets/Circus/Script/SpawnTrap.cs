using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrap : MonoBehaviour
{
    public List<GameObject> traps = default;
    private GameObject recentPotTrap = default;
    private GameObject recentTrap = default;
    private int recentTrapType = 1;

    private float spawnRingDistance = 16f;
    private float spawnPotDistance = 22f;
    private float spawnMonkeyDistance = 12f;
    private GameObject player;

    public GameObject trapPool;

    private void Awake()
    {
        player = gameObject.GetComponent<Follow>().target;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.stage == 1)
        {
            if (recentTrap == null || (transform.position.x - recentTrap.transform.position.x) > spawnRingDistance)
            {
                int trapNum = Random.Range(0, traps.Count - 1);
                //int trapNum = 2;
                recentTrap = Instantiate(traps[trapNum], new Vector3(transform.position.x, traps[trapNum].transform.position.y), Quaternion.identity, trapPool.transform);
                if (trapNum == 2 && Random.Range(0, 10) > 0)
                {
                    if (recentTrap != null && recentTrap.GetComponentInChildren<Gold>() != null)
                    {
                        recentTrap.GetComponentInChildren<Gold>().gameObject.SetActive(false);
                    }
                }
            }

            if (player.transform.position.x <= 130)
            {
                if (recentPotTrap == null || Mathf.Abs(transform.position.x - recentPotTrap.transform.position.x) > spawnPotDistance)
                {
                    if (recentPotTrap != null && transform.position.x - recentPotTrap.transform.position.x < 0)
                    {
                        Destroy(recentPotTrap);
                    }
                    int trapNum = 3;
                    recentPotTrap = Instantiate(traps[trapNum], new Vector3(transform.position.x, traps[trapNum].transform.position.y), Quaternion.identity, trapPool.transform);
                }
            }
        }

        if (GameManager.instance.stage == 2)
        {
            if (recentTrap == null || (transform.position.x - recentTrap.transform.position.x) > spawnMonkeyDistance)
            {
                if (Random.Range(0,10)<=1 && recentTrapType==0)
                {
                    recentTrapType = 1;
                    GameObject tmp = Instantiate(traps[1], new Vector3(transform.position.x, traps[1].transform.position.y), Quaternion.identity, trapPool.transform);
                    tmp.GetComponent<ScrollAndJump>().frontMonkey = recentTrap;
                    recentTrap = tmp;
                }
                else
                {
                    recentTrapType = 0;
                    recentTrap = Instantiate(traps[0], new Vector3(transform.position.x+Random.Range(-4, 4), traps[0].transform.position.y), Quaternion.identity, trapPool.transform);

                }
            }

        }
    }
}
