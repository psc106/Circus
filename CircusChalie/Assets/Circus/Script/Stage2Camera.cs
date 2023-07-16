using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Camera : MonoBehaviour
{

    private bool isStop = false;
    private Vector3 stopPosition;

    void Update()
    {

        if ((GameManager.instance.isLoading || GameManager.instance.isGameover || GameManager.instance.isStageClear)
            && GameManager.instance.sceneUI.gameoverUI.activeInHierarchy)
        {
            transform.position = new Vector3(transform.parent.transform.position.x+9, 100f, -20);
        }
        else
        {
            if (transform.parent.GetComponent<PlayerSt2>().isInvicible)
            {
                if (isStop)
                {
                    transform.position = stopPosition;
                }
                else
                {
                    isStop = true;
                    stopPosition = transform.position;
                }
            }

            if (transform.parent.transform.position.x < 145)
            {
                transform.position = new Vector3(transform.position.x, 1f, -20);
            }
            else
            {
                transform.position = new Vector3(145+9, 1f, -20);
            }
        }
    }
}
