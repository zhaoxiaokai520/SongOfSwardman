using UnityEngine;
using UnityEngine.UI;
//using TextFx;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Mgr;

public class TalkDialog : MonoSingleton<TalkDialog>
{
    public GameObject DialogGroup;
    public Camera UICamera;
    public Text message;
    public Text msg;
    public AudioClip clickAudio;
    public AudioClip talkEndAudio;
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
        //mClickAudio = DialogGroup.GetComponent<AudioSource>();

        //mTalkEndAudio = (AudioClip)Resources.Load(Application.dataPath + "/RawRes/Audio/Sound/Slide_Sharp_01.mp3", typeof(AudioClip));

        //string url = "file://" + Application.streamingAssetsPath + "/Audio/Sounds/Slide_Sharp_01.mp3";
        //DebugHelper.Log(url);
        //WWW www = WWW.LoadFromCacheOrDownload(url, 3);
        //mTalkEndAudio = www.audioClip;
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
		if (msg.text.Equals(""))
        {
            HideTalk();
            //talkEndAudio.Play();
            AudioMgr.instance.PlayAudioClip(talkEndAudio, gameObject);
        }
        else
        {
            InputMgr.GetInstance().SetLevel(10);
            UICamera.cullingMask |= LayerMask.GetMask("UI_Dialog");
            Debug.Log("TalkDialog.ShowTalk() enable dialog ============== " + message.text);
            //message.AnimationManager.PlayAnimation();
            AudioMgr.instance.PlayAudioClip(clickAudio, gameObject);
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
		msg.font.RequestCharactersInTexture(text);
        CharacterInfo characterInfo;
        float width = 0f;
        for (int i = 0; i < text.Length; i++)
        {
			msg.font.GetCharacterInfo(text[i], out characterInfo);
            width += characterInfo.advance;
        }

		msg.text = text;
        //message.AnimationManager.PlayAnimation();
    }

    public void UpdateText(string text, int updateStyle = 0)
    {
		msg.text = text;
        //message.AnimationManager.PlayAnimation();
    }
}
