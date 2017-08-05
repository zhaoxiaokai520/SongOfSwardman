using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
// NOTE:
// only DEEPEST camera can make effect available, 
// add this script to lower depth camera will not work
//
public class BloomEffect : BaseEffect
{
    //bloom property
    //[CondHideAttr("enableBloom", false)]
    //[Header("Bloom Attr")]
    //[CondHideAttr("enableBloom", false)]
    public Color colorMix = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    [Range(0.001f, 1.0f)]
    public float threshold = 0.25f;
    [Range(0.001f, 2.5f)]
    public float intensity = 0.75f;
    [Range(0.2f, 1.0f)]
    public float BlurSize = 1.0f;

    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

    void Awake()
    {

    }
	// Use this for initialization
	void Start () {

    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (effectSupported && null != effectShader)
        {
            RenderBloom(sourceTexture, destTexture);
        }
        else
        {
            Debug.Log("BloomEffect.OnRenderImage normal=====================");
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    private void RenderBloom(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        Debug.Log("BloomEffect.RenderBloom");

        if (threshold != 0 && intensity != 0)
        {
            int rtW = sourceTexture.width / 4;
            int rtH = sourceTexture.height / 4;

            materialEffect.SetColor("_ColorMix", colorMix);
            materialEffect.SetVector("_Parameter", new Vector4(BlurSize * 1.5f, 0.0f, intensity, 0.8f - threshold));
            // material.SetFloat("_blurSize",BlurSize);

            RenderTexture rtTempA = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
            rtTempA.filterMode = FilterMode.Bilinear;

            RenderTexture rtTempB = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
            rtTempA.filterMode = FilterMode.Bilinear;

            Graphics.Blit(sourceTexture, rtTempA, materialEffect, 0);

            Graphics.Blit(rtTempA, rtTempB, materialEffect, 1);
            RenderTexture.ReleaseTemporary(rtTempA);

            rtTempA = RenderTexture.GetTemporary(rtW, rtH, 0, rtFormat);
            rtTempB.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rtTempB, rtTempA, materialEffect, 2);

            materialEffect.SetTexture("_Bloom", rtTempA);
            Graphics.Blit(sourceTexture, destTexture, materialEffect, 3);

            RenderTexture.ReleaseTemporary(rtTempA);
            RenderTexture.ReleaseTemporary(rtTempB);
        }
    }
}