using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Camera : MonoBehaviour
{

    void Update()
    {

        if ((GameManager.instance.isLoading || GameManager.instance.isGameover || GameManager.instance.isStageClear)
            && GameManager.instance.sceneUI.gameoverUI.activeInHierarchy)
        {
            transform.position = new Vector3(transform.parent.transform.position.x+10, 100f, -20);
        }
        else
        {
            if (transform.parent.transform.position.x < 150)
            {
                transform.position = new Vector3(transform.position.x, 0f, -20);
            }
            else
            {
                transform.position = new Vector3(160, 0f, -20);
            }
        }
    }
}
