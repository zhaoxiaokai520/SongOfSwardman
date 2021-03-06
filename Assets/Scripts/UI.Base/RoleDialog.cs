﻿using UnityEngine;
//using TextFx;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.UI.Mgr;

public class RoleDialog : MonoBehaviour, IUpdateSub {
    public GameObject DialogGroup;
    public Camera UICamera;
    public Text message;
    private AudioSource mClickAudio;
    private List<string> mCachedTexts = new List<string>();//for too many word that cant display once all in dialog

    public enum DialogAnimStype {
        NO_ANIM, SCALE_ANIM, FADE_INOUT,
    };

    private bool isDialogOpened = false;
    // Use this for initialization
    void Start () {
        mClickAudio = DialogGroup.GetComponent<AudioSource>();

		GameUpdateMgr.GetInstance().Register(this);
    }

	void OnDestory()
	{
		GameUpdateMgr.instance.Unregister(this);
	}
	
	// Update is called once per frame
	public void UpdateSub (float delta) {
        if (Input.GetKeyUp(KeyCode.K))
        {
            isDialogOpened = !isDialogOpened;
            //int uimask = LayerMask.GetMask("UI_Dialog");
            if (isDialogOpened)
            {
                //int mask = LayerMask.GetMask("UI_Dialog");
                UICamera.cullingMask |= LayerMask.GetMask("UI_Dialog");
                //Debug.Log("MenuController.update() enable dialog ==============");
                //message.AnimationManager.PlayAnimation();
                //message.text = "aaaaaaaaaa";
                mClickAudio.Play();
            }
            else
            {
                UICamera.cullingMask &= ~LayerMask.GetMask("UI_Dialog");
                //Debug.Log("MenuController.update() disable dialog --------------");
            }
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            ShowText("bbbbbbbbbb你好啊bbbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }

    void OnDisable()
    {

    }

    void OnEnable()
    {

    }

    public void ShowText(string text, int showStyle = 0)
    {
        //seperate text for long text
        message.font.RequestCharactersInTexture(text);
        CharacterInfo characterInfo;
        float width = 0f;
        for (int i = 0; i < text.Length; i++)
        {
            message.font.GetCharacterInfo(text[i], out characterInfo);
            width += characterInfo.advance;
        }

        message.text = text;
        //message.AnimationManager.PlayAnimation();
    }

    public void UpdateText(string text, int updateStyle = 0)
    {
        message.text = text;
        //message.AnimationManager.PlayAnimation();
    }
}
