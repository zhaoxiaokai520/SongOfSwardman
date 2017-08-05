using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
// NOTE:
// only DEEPEST camera can make effect available, 
// add this script to lower depth camera will not work
//
public class BaseEffect : MonoBehaviour {
    protected bool effectSupported = true;
    //protected bool enableEffect = true;
    public Shader effectShader;
    protected Material effectMaterial;

    void Awake()
    {

    }
	// Use this for initialization
	void Start () {
        if (!SystemInfo.supportsImageEffects)
        {
            effectSupported = SystemInfo.supportsImageEffects;
            enabled = false;
            return;
        }

        if (!effectShader && !effectShader.isSupported)
        {
            effectSupported = false;
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (effectSupported)
        {

        }
    }

    void OnDisable()
    {
        if (effectMaterial)
        {
            DestroyImmediate(effectMaterial);
        }
    }

    public bool IsSupported()
    {
        return effectSupported;
    }

    protected Material materialEffect
    {
        get
        {
            if (effectMaterial == null)
            {
                effectMaterial = new Material(effectShader);
                effectMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return effectMaterial;
        }
    }
}