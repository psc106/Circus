using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGround : MonoBehaviour
{
    public Transform player;

    public BackGroundField left;
    public BackGroundField center;
    public BackGroundField right;

    void Update()
    {

        if (!GameManager.instance.isLoading)
        {
            float playerX = player.transform.position.x;
            float centerX = center.transform.position.x;
            float leftX = left.transform.position.x;

            if (player.transform.position.x < 150)
            {
                if ((centerX - playerX) < 2.1f && (centerX - playerX) > 1.9f)
                {

                    if (player.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        SwapLeft();
                    }
                }


                else if ((leftX - playerX) < 2.1f && (leftX - playerX) > 1.9f)
                {


                    if (player.GetComponent<Rigidbody2D>().velocity.x < 0)
                    {
                        SwapRight();
                    }
                }
            }
        }
        else
        {
            Init();
        }
    }

    void Init()
    {
        while(left.currDistance != 10-GameManager.instance.savePoint)
        {
            SwapRight();
        }
    }

    void SwapLeft()
    {
        GameManager.instance.savePoint += 1;
        left.transform.position = new Vector3(left.transform.position.x + 48, left.transform.position.y, left.transform.position.z);
        BackGroundField tmp = left;
        left = center;
        center = right;
        right = tmp;
        right.currDistance -= 3;
        right.ChangeText();
    }
    void SwapRight()
    {
        GameManager.instance.savePoint -= 1;
        right.transform.position = new Vector3(right.transform.position.x - 48, right.transform.position.y, right.transform.position.z);
        BackGroundField tmp = right;
        right = center;
        center = left;
        left = tmp;
        left.currDistance += 3;
        left.ChangeText();
    }
}
