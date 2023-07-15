using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPool : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isLoading)
        {
            RemoveObject[] child = GetComponentsInChildren<RemoveObject>();

            foreach (RemoveObject child2 in child)
            {
                Destroy(child2.gameObject);
            }
        }
        
    }
}
