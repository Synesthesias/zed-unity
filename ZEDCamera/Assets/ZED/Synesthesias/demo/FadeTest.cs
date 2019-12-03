using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<ZEDManager>().FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FindObjectOfType<ZEDManager>().FadeOut();
        }
    }
}
