using Assets.Scripts.Core.Event;
using Assets.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Mgr;

public class MainMenu : MonoBehaviour, IUpdateSub {
    public GameObject MenuGroup;
    public GameObject ScrollGroup;
    public GameObject StatusBtn;
    public GameObject ItemBtn;
    public GameObject EquipBtn;
    public GameObject SkillBtn;
    public GameObject AIBtn;
    public GameObject SettingBtn;
    public GameObject SaveLoadBtn;
    public Camera UICamera;

    private AudioSource mClickAudio;


    private bool isMenuOpened = false;
    private void Awake()
    {
        addListener();
    }
    // Use this for initialization
    void Start () {
        //default show scroll hide menu
        mClickAudio = GetComponent<AudioSource>();
        UICamera.cullingMask |= LayerMask.GetMask("UI_Menu");
		UpdateGameMgr.instance.Register(this);
    }

    private void OnDestroy()
    {
        rmvListener();
		UpdateGameMgr.instance.Unregister(this);
    }

    // Update is called once per frame
	public void UpdateSub (float delta) {
        if (Input.GetKeyUp(KeyCode.M))
        {
            //MenuGroup.SetActive(!MenuGroup.activeSelf);
            isMenuOpened = !isMenuOpened;
            //int uimask = LayerMask.GetMask("UI_Menu");
            _setMenuOpen(isMenuOpened);
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            CameraCtrl.AddTrailShake(0f, 30, .2f, false, 0f);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            CameraCtrl.AddTrailMove(0f, new Vector3(0, 100, 0), 0f);
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            CameraCtrl.AddTrailRotate(0f, Quaternion.Euler(0, 60, 0), 0f);
        }
    }

    void OnDisable()
    {

    }

    void OnEnable()
    {

    }

    void addListener()
    {
        GameObject btnObj = ScrollGroup.transform.Find("Scroll").gameObject;
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(OnScrollClicked);

        SosEventMgr.instance.Subscribe(MapEventId.cancel, OnCancelClicked);
    }

    void rmvListener()
    {
        GameObject btnObj = ScrollGroup.transform.Find("Scroll").gameObject;
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.RemoveListener(OnScrollClicked);

        SosEventMgr.instance.Unsubscribe(MapEventId.cancel, OnCancelClicked);
    }

    void OnScrollClicked()
    {
        _setMenuOpen(true);
        isMenuOpened = true;
    }

    bool OnCancelClicked(SosObject sender, SosEventArgs args)
    {
        _setMenuOpen(false);
        isMenuOpened = false;
        return true;
    }

    void _setMenuOpen(bool open)
    {
        if (open)
        {
            //int mask = LayerMask.GetMask("UI_Menu");
            //UICamera.cullingMask |= LayerMask.GetMask("UI_Menu");
            MenuGroup.SetActive(true);
            ScrollGroup.SetActive(false);
            //Debug.Log("MenuController.update() enable menu ==============");
            mClickAudio.Play();
        }
        else
        {
            MenuGroup.SetActive(false);
            ScrollGroup.SetActive(true);
            //int mask = LayerMask.GetMask("UI_Menu");
            //UICamera.cullingMask &= ~LayerMask.GetMask("UI_Menu");
            //Debug.Log("MenuController.update() disable menu --------------");
        }
    }
}
