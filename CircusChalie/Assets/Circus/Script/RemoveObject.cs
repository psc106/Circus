using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{

    public bool isActive = default;

    private void Start()
    {
        isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
