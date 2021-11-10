using System;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManageStart : MonoBehaviour
{
    //选项文本
    public Text optionText;
    //帮助文本
    public Text helpText;
    //离开文本
    public Text exitText;
    //用户名文本
    public Text usernameText;
    //保存鼠标位置（屏幕坐标）
    Vector3 mousePosition;
    //保存鼠标位置（世界坐标）
    Vector3 mouseWorldPos;
    //保存冒险模式的两个spite 正常状态和按下状态
    public Sprite[] adventrueSprite = new Sprite[2];
    //保存迷你模式的两个spite 正常状态和按下状态
    public Sprite[] survivalSprite = new Sprite[2];
    //保存益智模式的两个spite 正常状态和按下状态
    public Sprite[] challengeSprite = new Sprite[2];
    //保存更改用户名木板选项卡的两个sprite
    public Sprite[] woodSign2Sprite = new Sprite[2];

    //冒险模式物体找到后的返回值
    private GameObject adventrueScreen;
    //迷你模式找到后的返回值
    private GameObject survivalScreen;
    //益智模式找到后的返回值
    private GameObject challengeScreen;
    //改变用户名板子找到后的返回值
    private GameObject woodSign2Screen;
    //改变用户名面板的返回值
    private GameObject changeUsername = null;
    //新疆用户名面板的返回值
    private GameObject createUsername = null;
    //改变用户名的UI面板
    private GameObject changeUsernameUI;
    //新建用户名的UI面板
    private GameObject createUsernameUI;
    //改变游戏用户名的输入框
    public InputField usernameInput;
    //新建游戏用户名的文本预制体
    public Text usernamePrefab;
    //新建游戏用户名的默认y坐标
    private int usernamey = 92;
    //保存创建游戏用户名 最多10个
    private Text[] usernameTexts = new Text[10];
    //保存游戏用户名的个数
    private int usernameNum = 0;
    //保存点击游戏名后绿色背景的Panel容器
    private GameObject usernameP;
    //判断本次操作是不是重命名 因为重命名和新建名字都会调用出同一个对话框
    private bool isRename = false;
    //保存当前是哪一个用户名被点击了
    private int usernameIndex;

    //改变用户名的预制体
    public GameObject changeUsernamePrefab;
    //新建用户名的预制体
    public GameObject createUsernamePrefab;
    public GameObject optionDialogPrefab;
    private GameObject optionDialog;
    private GameObject optionDialogUI;
    public Slider volumeSlider;
    public Slider soundEffectSlider;
    private GameObject exitGame;
    private GameObject exitGameUI;

    private void Start()
    {
        //读取用户名
        usernameText.text = PlayerPrefs.GetString("username", "SmallZombieZombie");
        changeUsernameUI = GameObject.Find("ChangeUsernameUI");//找到游戏中的改变用户名的UI面板
        createUsernameUI = GameObject.Find("CreateUsernameUI");//找到游戏中新建用户名的UI面板
        optionDialogUI = GameObject.Find("OptionDialogUI");
        exitGameUI = GameObject.Find("ExitGameUI");
        usernameP = GameObject.Find("UsernameP");
        changeUsernameUI.SetActive(false);//失能UI面板 初始不显示
        createUsernameUI.SetActive(false);
        optionDialogUI.SetActive(false);
        exitGameUI.SetActive(false);

        Transform[] f = changeUsernameUI.GetComponentsInChildren<Transform>();//查找子物体
        usernameNum = PlayerPrefs.GetInt("usernameNum", 0);//读取创建的用户数量
        int y = usernamey;
        for (int i = 0; i < usernameNum; i++)
        {
            Text t = Instantiate(usernamePrefab);//实例化一个文本
            foreach (Transform child in f)
            {
                if (child.name == "Username")
                {
                    t.transform.SetParent(child);//设置父类物体
                    string s = PlayerPrefs.GetString("username" + i, "SmallZombieZombie");//获取存储的用户名
                    t.GetComponent<Text>().text = s;
                    if (usernameText.text.Equals(s))
                    {
                        usernameP.transform.localPosition = new Vector3(-18, t.transform.localPosition.y, 0);//设置默认坐标
                    }
                    t.transform.localScale = new Vector3(1, 1, 1);//恢复原来的比例大小 因为放入UI中会被缩放
                    t.transform.localPosition = new Vector3(-18, y, 0);//设置文本坐标
                    usernameTexts[i] = t;//放入用户名数组中
                }
                else if (child.name == "NameNull")
                {
                    Vector3 pos = child.transform.localPosition;
                    child.transform.localPosition = new Vector3(pos.x, pos.y - 20, 0);//将新建用户名的按钮下移
                }
            }
            y -= 20;//每次得到一个用户名后将坐标往下移动
        }
    }

    private void Update()
    {

        mousePosition = Input.mousePosition; //获取屏幕坐标
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition); //屏幕坐标转世界坐标
        //以鼠标的当前位置发射射线（2D状态下得用2D的射线）
        //方向向左 距离为1f //这个方法不好用 UI上面的碰撞器识别不出来
        //RaycastHit2D c = Physics2D.Raycast(new Vector2(mouseWorldPos.x, mouseWorldPos.y), Vector2.left, 1f);
        //这个方法可以识别UI上面的碰撞器
        Collider2D[] c = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //找到游戏里的各种物体
        adventrueScreen = GameObject.Find("SelectorScreenAdventure");
        survivalScreen = GameObject.Find("SelectorScreenSurvival");
        challengeScreen = GameObject.Find("SelectorScreenChallenges");
        woodSign2Screen = GameObject.Find("SelectorScreen_WoodSign2");

        if (c.Length > 0)
        {
            foreach (Collider2D d in c)
            {
                if (changeUsername == null&&optionDialog==null&&exitGame==null)//改变用户名面板为空的时候才可以操作主面板
                {
                    switch (d.gameObject.name)
                    {
                        case "AdventrueCollision"://冒险模式 更改sprite的显示
                            //防止多个 被点亮的情况
                            survivalScreen.GetComponent<SpriteRenderer>().sprite = survivalSprite[0];
                            challengeScreen.GetComponent<SpriteRenderer>().sprite = challengeSprite[0];
                            adventrueScreen.GetComponent<SpriteRenderer>().sprite = adventrueSprite[1];
                            if (Input.GetMouseButtonDown(0))
                            {
                                SceneManager.LoadScene(2); //加载下一个场景
                            }
                            break;
                        case "SurvivalCollision"://迷你模式
                            //防止多个 被点亮的情况
                            adventrueScreen.GetComponent<SpriteRenderer>().sprite = adventrueSprite[0];
                            challengeScreen.GetComponent<SpriteRenderer>().sprite = challengeSprite[0];
                            survivalScreen.GetComponent<SpriteRenderer>().sprite = survivalSprite[1];
                            break;
                        case "ChallengeCollision"://益智模式
                            //防止多个 被点亮的情况
                            adventrueScreen.GetComponent<SpriteRenderer>().sprite = adventrueSprite[0];
                            survivalScreen.GetComponent<SpriteRenderer>().sprite = survivalSprite[0];
                            challengeScreen.GetComponent<SpriteRenderer>().sprite = challengeSprite[1];
                            break;
                        case "SelectorScreen_WoodSign2C"://改变用户名选项卡
                            woodSign2Screen.GetComponent<SpriteRenderer>().sprite = woodSign2Sprite[1];
                            if (Input.GetMouseButtonDown(0))
                            {
                                //实例化改变用户名的面板
                                changeUsername = Instantiate(changeUsernamePrefab, new Vector3(148, 69, 0), Quaternion.identity);
                                changeUsernameUI.SetActive(true);//激活UI面板
                                woodSign2Screen.GetComponent<SpriteRenderer>().sprite = woodSign2Sprite[0];
                            }
                            break;
                        case "Option"://选项
                            optionText.color = new UnityEngine.Color(80 / 255f, 231 / 255f, 35 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //实例化改变用户名的面板
                                optionDialog = Instantiate(optionDialogPrefab, new Vector3(-14, 0, 0), Quaternion.identity);
                                optionDialogUI.SetActive(true);//激活UI面板
                                optionText.color = new UnityEngine.Color(50 / 255f, 50 / 255f, 50 / 255f);
                                //读取上一次保存的值
                                volumeSlider.value = PlayerPrefs.GetFloat("volume");
                                soundEffectSlider.value = PlayerPrefs.GetFloat("soundEffect");
                            }
                            break;
                        case "Help"://帮助
                            helpText.color = new UnityEngine.Color(80 / 255f, 231 / 255f, 35 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                SceneManager.LoadScene(3); //加载下一个场景
                            }
                            break;
                        case "Exit"://离开
                            exitText.color = new UnityEngine.Color(80 / 255f, 231 / 255f, 35 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //实例化改变用户名的面板
                                exitGame = Instantiate(createUsernamePrefab, new Vector3(-44, -23, 0), Quaternion.identity);
                                exitGameUI.SetActive(true);//激活UI面板
                                exitText.color = new UnityEngine.Color(50 / 255f, 50 / 255f, 50 / 255f);
                            }
                            break;
                    } 
                }
                else if (changeUsername != null&&createUsername==null)
                {
                    switch (d.name)
                    {
                        case "NameNull"://空用户 新建用户
                            //更改文本颜色
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(255 / 255f, 255 / 255f, 255 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //实例化新建用户名面板
                                createUsername = Instantiate(createUsernamePrefab, new Vector3(-44, -23, 0), Quaternion.identity);
                                createUsernameUI.SetActive(true);//激活新建用户名UI面板
                                d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(219 / 255f, 210 / 255f, 169 / 255f);
                            }
                            isRename = false;//不是重命名
                            break;
                        case "Confirm"://确认
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                for (int i = 0; i < usernameNum; i++)
                                {
                                    if (usernameTexts[i].transform.localPosition.y == usernameP.gameObject.transform.localPosition.y)
                                    {
                                        usernameText.text = usernameTexts[i].text;//更改界面显示的用户名
                                        PlayerPrefs.SetString("username", usernameTexts[i].text);//存储当前的用户名
                                    }
                                }
                                Destroy(changeUsername);//销毁改变用户名面板
                                changeUsernameUI.SetActive(false);
                            }
                            break;
                        case "Delete"://删除用户名
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            break;
                        case "Cancel"://取消操作
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                Destroy(changeUsername);//销毁更改用户名面板
                                changeUsernameUI.SetActive(false);//失能更改用户名UI面板
                            }
                            break;
                        case "Rename"://重命名
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //实例化新建用户名面板
                                createUsername = Instantiate(createUsernamePrefab, new Vector3(-44, -23, 0), Quaternion.identity);
                                createUsernameUI.SetActive(true);//激活新建用户名UI面板
                                d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);

                                for (int i = 0; i < usernameNum; i++)
                                {
                                    //因为Panel的坐标和某一个text的坐标一定是对应的 所以可以这么操作
                                    if (usernameTexts[i].transform.localPosition.y == usernameP.gameObject.transform.localPosition.y)
                                    {
                                        //输入框内的文等于选中的文本
                                        usernameInput.text = usernameTexts[i].text;
                                        usernameIndex = i;//保存此次选中的文本索引
                                    }
                                }
                                isRename = true;//是重命名
                            }
                            break;
                        case "UsernameT(Clone)"://文本
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(255 / 255f, 255 / 255f, 255 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //点击文本后将背景移动到文本的位置
                                usernameP.transform.localPosition = d.gameObject.transform.localPosition;
                            }
                            break;
                        default:
                            //恢复默认颜色
                            Transform[] father = changeUsernameUI.GetComponentsInChildren<Transform>();
                            foreach (Transform child in father)
                            {
                                if (child.name == "NameNull")
                                {
                                    child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(219 / 255f, 210 / 255f, 169 / 255f);
                                }
                                else if (child.name == "Delete" || child.name == "Confirm" || child.name == "Cancel" || child.name == "Rename")
                                {
                                    child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                                }
                            }
                            for (int i = 0; i < usernameNum; i++)
                            {
                                usernameTexts[i].gameObject.GetComponent<Text>().color = new UnityEngine.Color(219 / 255f, 210 / 255f, 169 / 255f);
                            }
                            break;
                    }
                }
                else if (createUsername != null)
                {
                    switch (d.name)
                    {
                        case "Confirm"://确认
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                if (isRename)//重命名
                                {
                                    //将数组内的文本更改
                                    usernameTexts[usernameIndex].text = usernameInput.text;
                                    //重新存储文本的值
                                    PlayerPrefs.SetString("username" + usernameIndex, usernameInput.text);
                                    Destroy(createUsername);//销毁创建用户名对话框
                                    createUsernameUI.SetActive(false);
                                }
                                else
                                {
                                    //存储用户名数组
                                    PlayerPrefs.SetString("username" + usernameNum, usernameInput.text);
                                    //存储界面显示的用户名
                                    PlayerPrefs.SetString("username", usernameInput.text);
                                    //直接更改当前显示的用户名
                                    usernameText.text = usernameInput.text;
                                    //获取子物体
                                    Transform[] f = changeUsernameUI.GetComponentsInChildren<Transform>();
                                    Text t = Instantiate(usernamePrefab);//实例化文本
                                    foreach (Transform child in f)
                                    {
                                        if (child.name == "Username")//找到Username 作为用户名数组的父物体
                                        {
                                            t.transform.SetParent(child);//设置父类物体
                                        }
                                        else if (child.name == "NameNull")
                                        {
                                            Vector3 pos = child.transform.localPosition;
                                            //新建用户名按钮下移
                                            child.transform.localPosition = new Vector3(pos.x, pos.y - 20, 0);
                                        }
                                    }

                                    t.text = usernameInput.text;//设置文本为输入框的文本
                                    t.transform.localScale = new Vector3(1, 1, 1);//恢复原来的缩放
                                    t.transform.localPosition = new Vector3(-18, usernamey, 0);//设置位置
                                    usernameTexts[usernameNum] = t;//将该用户名放入数组
                                    for (int i = 0; i < usernameNum; i++)
                                    {
                                        //每次新建一个用户名就将原来的用户名下移
                                        usernameTexts[i].transform.localPosition = new Vector3(usernameTexts[i].transform.localPosition.x, usernameTexts[i].transform.localPosition.y - 20, 0);
                                    }
                                    usernameNum += 1;//用户名的数量增加
                                    PlayerPrefs.SetInt("usernameNum", usernameNum);//保存用户名的数量

                                    Destroy(createUsername);
                                    createUsernameUI.SetActive(false);
                                    Destroy(changeUsername);
                                    changeUsernameUI.SetActive(false);
                                }
                            }
                            break;
                        case "Cancel"://取消
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                Destroy(createUsername);//销毁更改用户名面板
                                createUsernameUI.SetActive(false);//失能更改用户名UI面板
                            }
                            break;
                        default:
                            //恢复原来的颜色
                            Transform[] father = createUsernameUI.GetComponentsInChildren<Transform>();
                            foreach (Transform child in father)
                            {

                                if (child.name == "Confirm" || child.name == "Cancel")
                                {
                                    child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                                }
                            }
                            break;
                    }
                }
                else if(optionDialog!=null)
                {
                    switch (d.name)
                    {
                        case "Confirm":
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                Destroy(optionDialog);//销毁更改用户名面板
                                optionDialogUI.SetActive(false);//失能更改用户名UI面板
                            }
                            break;
                        default:
                            //恢复原来的颜色
                            Transform[] father = optionDialogUI.GetComponentsInChildren<Transform>();
                            foreach (Transform child in father)
                            {
                                if (child.name == "Confirm")
                                {
                                    child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                                }
                            }
                            break;
                    }
                }
                else if (exitGame != null)
                {
                    switch (d.name)
                    {
                        case "Confirm":
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButton(0))
                            {
                                ExitGame();
                            }
                            break;
                        case "Cancel":
                            d.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 255 / 255f, 0 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                Destroy(exitGame);//销毁更改用户名面板
                                exitGameUI.SetActive(false);//失能更改用户名UI面板
                            }
                            break;
                        default:
                            //恢复原来的颜色
                            Transform[] father = exitGameUI.GetComponentsInChildren<Transform>();
                            foreach (Transform child in father)
                            {
                                if (child.name == "Confirm" || child.name == "Cancel")
                                {
                                    child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                                }
                            }
                            break;
                    }
                }
            }
        }
        else
        {
            if (changeUsername == null&&optionDialog==null)
            {
                //没有检测到物体的时候就将显示归为默认状态
                adventrueScreen.GetComponent<SpriteRenderer>().sprite = adventrueSprite[0];
                survivalScreen.GetComponent<SpriteRenderer>().sprite = survivalSprite[0];
                challengeScreen.GetComponent<SpriteRenderer>().sprite = challengeSprite[0];
                woodSign2Screen.GetComponent<SpriteRenderer>().sprite = woodSign2Sprite[0];
                optionText.color = new UnityEngine.Color(50 / 255f, 50 / 255f, 50 / 255f);
                helpText.color = new UnityEngine.Color(50 / 255f, 50 / 255f, 50 / 255f);
                exitText.color = new UnityEngine.Color(50 / 255f, 50 / 255f, 50 / 255f);
            }
            else if (changeUsername != null&&createUsername==null)
            {
                //恢复原来的颜色
                Transform[] father = changeUsernameUI.GetComponentsInChildren<Transform>();
                foreach (Transform child in father)
                {
                    if (child.name == "NameNull")
                    {
                        child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(219 / 255f, 210 / 255f, 169 / 255f);
                    }
                    else if (child.name == "Delete" || child.name == "Confirm" || child.name == "Cancel" || child.name == "Rename")
                    {
                        child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                    }
                }
            }
            else if (createUsername != null)
            {
                //恢复原来的颜色
                Transform[] f = createUsernameUI.GetComponentsInChildren<Transform>();
                foreach (Transform child in f)
                {

                    if (child.name == "Confirm" || child.name == "Cancel")
                    {
                        child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                    }
                }
            }
            else if (optionDialog != null)
            {
                //恢复原来的颜色
                Transform[] father = optionDialogUI.GetComponentsInChildren<Transform>();
                foreach (Transform child in father)
                {

                    if (child.name == "Confirm")
                    {
                        child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                    }
                }
            }
            else if (exitGame != null)
            {
                //恢复原来的颜色
                Transform[] father = exitGameUI.GetComponentsInChildren<Transform>();
                foreach (Transform child in father)
                {
                    if (child.name == "Confirm" || child.name == "Cancel")
                    {
                        child.gameObject.GetComponent<Text>().color = new UnityEngine.Color(0 / 255f, 200 / 255f, 0 / 255f);
                    }
                }
            }
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR    //在编辑器模式下
            EditorApplication.isPlaying = false;
        #else
                        Application.Quit();
        #endif
    }

    //音效值
    public void SoundEffectSlideChanged()
    {
        //保存音量的值
        PlayerPrefs.SetFloat("soundEffect", soundEffectSlider.value);
    }

    //音量值
    public void VolumeSlideChanged()
    {
        //保存音量的值
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}
