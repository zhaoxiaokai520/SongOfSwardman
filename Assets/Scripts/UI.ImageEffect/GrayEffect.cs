using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
// NOTE:
// only DEEPEST camera can make effect available, 
// add this script to lower depth camera will not work
//
public class GrayEffect : BaseEffect
{
    //[HideInInspector()]
    //gray property
    //[CondHideAttr("effectSupported", false)]
    //[CondHideAttr("enableGray", false)]
    //[Header("Gray Attr")]
    [Range(0.0f, 1.0f)]
    public float grayScaleAmout = 1.0f;

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
            RenderGray(sourceTexture, destTexture);
        }
        else
        {
            //Debug.Log("GrayEffect.OnRenderImage normal=====================");
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    private void RenderGray(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        Debug.Log("GrayEffect.RenderGray grayShader" + grayScaleAmout);
        materialEffect.SetFloat("_LuminosityAmount", grayScaleAmout);
        Graphics.Blit(sourceTexture, destTexture, materialEffect);
    }
}