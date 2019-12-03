using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZEDFade : MonoBehaviour
{
    //[SerializeField] float zedPlaneAlpha = 1.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeIn());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        float a = 0.0f;
        while(a <= 1.0f)
        {
            a += Time.deltaTime;
            SetAlpha(a);
            yield return null;
        }
        SetAlpha(1.0f);
        yield break;
    }

    public IEnumerator FadeOut()
    {
        float a = 1.0f;
        while(a >= 0.0f)
        {
            a -= Time.deltaTime;
            SetAlpha(a);
            yield return null;
        }
        SetAlpha(0.0f);
        yield break;
    }

    private void SetAlpha(float alpha)
    {
        try
        {
            FindObjectOfType<ZEDMixedRealityPlugin>().leftMaterial.SetFloat("_Alpha", alpha);
            FindObjectOfType<ZEDMixedRealityPlugin>().rightMaterial.SetFloat("_Alpha", alpha);
        }
        catch
        {

        }
    }
}
