using UnityEngine;

public class MainMenu : MonoBehaviour {
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
    // Use this for initialization
    void Start () {
        mClickAudio = MenuGroup.GetComponent<AudioSource>();
        UICamera.cullingMask |= LayerMask.GetMask("UI_Menu");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.M))
        {
            //MenuGroup.SetActive(!MenuGroup.activeSelf);
            isMenuOpened = !isMenuOpened;
            //int uimask = LayerMask.GetMask("UI_Menu");
            if (isMenuOpened)
            {
                //int mask = LayerMask.GetMask("UI_Menu");
                //UICamera.cullingMask |= LayerMask.GetMask("UI_Menu");
                MenuGroup.SetActive(true);
                isMenuOpened = true;
                //Debug.Log("MenuController.update() enable menu ==============");
                mClickAudio.Play();
            }
            else
            {
                MenuGroup.SetActive(false);
                isMenuOpened = false;
                //int mask = LayerMask.GetMask("UI_Menu");
                //UICamera.cullingMask &= ~LayerMask.GetMask("UI_Menu");
                //Debug.Log("MenuController.update() disable menu --------------");
            }
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
}
