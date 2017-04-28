using UnityEngine;
using UnityEngine.UI;
using TextFx;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Controller;

public class TalkDialog : MonoSingleton<TalkDialog>
{
    public GameObject DialogGroup;
    public Camera UICamera;
    public TextFxUGUI message;
    public Text msg;
    private AudioSource mClickAudio;
    //private int mInputLevel = 10;
    private List<string> mCachedTexts = new List<string>();//for too many word that cant display once all in dialog

    public enum DialogAnimStype {
        NO_ANIM, SCALE_ANIM, FADE_INOUT,
    };

    private bool isDialogOpened = false;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        mClickAudio = DialogGroup.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	//void Update () {
 //       if (InputMgr.GetInstance().GetLevel() > mInputLevel)
 //       {
 //           return;
 //       }
 //       if (Input.GetKeyUp(KeyCode.K))
 //       {
 //           isDialogOpened = !isDialogOpened;
 //           //int uimask = LayerMask.GetMask("UI_Dialog");
 //           if (isDialogOpened)
 //           {
 //               ////int mask = LayerMask.GetMask("UI_Dialog");
 //               //UICamera.cullingMask |= LayerMask.GetMask("UI_Dialog");
 //               ////Debug.Log("MenuController.update() enable dialog ==============");
 //               //message.AnimationManager.PlayAnimation();
 //               ////message.text = "aaaaaaaaaa";
 //               //mClickAudio.Play();
 //               ShowTalk();
 //           }
 //           else
 //           {
 //               //UICamera.cullingMask &= ~LayerMask.GetMask("UI_Dialog");
 //               //Debug.Log("MenuController.update() disable dialog --------------");
 //               HideTalk();
 //           }

 //       }
 //       else if (Input.GetKeyUp(KeyCode.G))
 //       {
 //           ShowText("bbbbbbbbbb你好啊bbbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
 //       }
 //   }

    //void OnDisable()
    //{

    //}

    //void OnEnable()
    //{

    //}
    public void ShowTalk(int talkeeId, int talkerId)
    {
        isDialogOpened = true;
        message.text = TalkSystem.GetInstance().GetTalk(talkeeId, talkerId);
        msg.text = message.text;
        if (message.text.Equals(""))
        {
            HideTalk();
        }
        else
        {
            InputMgr.GetInstance().SetLevel(10);
            UICamera.cullingMask |= LayerMask.GetMask("UI_Dialog");
            Debug.Log("TalkDialog.ShowTalk() enable dialog ============== " + message.text);
            //message.AnimationManager.PlayAnimation();
            mClickAudio.Play();
        }
    }

    public void HideTalk()
    {
        isDialogOpened = false;
        //message.AnimationManager.ResetAnimation();
        UICamera.cullingMask &= ~LayerMask.GetMask("UI_Dialog");
        InputMgr.GetInstance().RestoreLevel();
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
        message.AnimationManager.PlayAnimation();
    }

    public void UpdateText(string text, int updateStyle = 0)
    {
        message.text = text;
        message.AnimationManager.PlayAnimation();
    }
}
