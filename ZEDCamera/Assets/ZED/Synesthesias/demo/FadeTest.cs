using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<ZEDManager>().FadeIn(2.0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FindObjectOfType<ZEDManager>().FadeOut(2.0f);
        }
    }
}
