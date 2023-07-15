using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target = default;
    public float distanceX = default;
    public float distanceY = default;
    public bool isAlways = default;


    // Update is called once per frame
    void Update()
    {
        if (isAlways)
        {
            Vector3 tmp = target.transform.position;
            transform.position = new Vector3(tmp.x+ distanceX, distanceY, 0);
        }
        else if( target.transform.position.x<150 )
        {
            Vector3 tmp = target.transform.position;
            transform.position = new Vector3(tmp.x + distanceX, distanceY, 0);
        }
    }

}
