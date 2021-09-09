using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level2_rocks : MonoBehaviour
{
    void Update()
    {
        if (Camera.main.GetComponent<level2>().getLost())
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Camera.main.GetComponent<level2>().setLostTrue();
    }
}
