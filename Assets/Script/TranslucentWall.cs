using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Presets;
using UnityEngine;

public class TranslucentWall : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private bool isTranslucent;
    private Coroutine translucentCoroutine;
    private Coroutine resetCoroutine;
    private Coroutine timeCheckCoroutine;
    private float timer = 0.0f;
    private const float resetTime = 0.5f;
    private bool isReset =false;
    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }


    public void Translucent()
    {
        if(isTranslucent)
        {
            timer = 0;
            return;
        }

        if(resetCoroutine != null && isReset)
        {
            timer = 0;
            isTranslucent = false;
            isReset = false;
            StopCoroutine(resetCoroutine);
        }

        SetMaterialAlpha(3.0f,3000);
        isTranslucent = true;
        translucentCoroutine = StartCoroutine(StartTranslucent());

    }
    private void ResetTranslucent()
    {
        SetMaterialAlpha(0f, -1);
        resetCoroutine = StartCoroutine(ResetTranslucentCoroutine());
    }
    private void SetMaterialAlpha(float mode,int renderQ)
    {
        foreach (var renderer in meshRenderers)
        {

            foreach (Material mat in renderer.materials)
            {
                ChangeMaterialAlphaMode(mat, mode);
            }

        }

    }
    private void ChangeMaterialAlphaMode(Material mat, float mode)
    {
        switch (mode)
        {
            case 0.0f:
                mat.SetFloat("_Mode", 0);
                mat.SetOverrideTag("RenderType", "Opaque");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                break;

            case 3.0f:
                mat.SetFloat("_Mode", 3);
                mat.SetOverrideTag("RenderType", "Transparent");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;


        }
    }

    IEnumerator StartTranslucent()
    {
        while (true)
        {
            bool complete = true;
            foreach (var renderer in meshRenderers)
            {
                if (renderer.material.color.a > 0.5f)
                    complete = false;

                var col = renderer.material.color;
                col.a -= Time.deltaTime;
                renderer.material.color = col;
            }

            if(complete)
            {
                if (timeCheckCoroutine != null)
                    StopCoroutine(timeCheckCoroutine);
                timeCheckCoroutine = StartCoroutine(ResetTimer());
                break;
            }

            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator ResetTranslucentCoroutine()
    {
        isTranslucent = false;

        while (true)
        {
            bool complete = true;
            foreach (var renderer in meshRenderers)
            {
                if (renderer.material.color.a < 1f)
                    complete = false;

                var col = renderer.material.color;
                col.a += Time.deltaTime;
                renderer.material.color = col;
            }

            if (complete)
            {
                isReset = false;
                break;
            }

            yield return new WaitForSeconds(0.001f);
        }
    }


    IEnumerator ResetTimer()
    {
        timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= resetTime)
            {
                isReset = true;
                ResetTranslucent();

            }


            yield return null;
        }

    }
}
