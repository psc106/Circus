using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = default;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -20)
        {
            Destroy(gameObject);
        }

    }
}
