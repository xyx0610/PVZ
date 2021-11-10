using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 游戏控制脚本 挂载在空物体上
 */
public class GameManage : MonoBehaviour
{
    //跟随鼠标移动物体实例化返回的对象
    private GameObject withMousePlant;

    //将物体从世界坐标转换为屏幕坐标
    Vector3 screenPosition;
    //获取到点击屏幕的屏幕坐标
    Vector3 mousePositionOnScreen;
    //将点击屏幕的屏幕坐标转换为世界坐标
    Vector3 mousePositionInWorld;

    //阳光的预制体
    public GameObject sunPrefab;
    //阳光数量的UI
    public Text sunText;
    //阳光数量变量
    public static int sunNum = 100;

    //保存被点击的卡片
    private GameObject clickedCard;
    //是否有卡片被点击的标志
    private bool isClickedCard = false;
    //植物是否已经种植的标志
    public static bool isHadPlanting = false;

    //僵尸的预制体
    public GameObject zombiePrefab;
    //僵尸出生的y坐标
    int[] spawnY = { 196, 96, -4, -97, -195 };

    //提示文本
    public Text hintText;
    //卡片选择栏
    public GameObject chooseBk;
    //相机物体 这里有一个警告 说可以用Component下的camera 但是试过了不可以
    public GameObject camera;
    //场景移动的速度
    public float moveSpeed;
    //场景移动的状态 分为两个状态
    private int moveState = 0;
    //保存场景移动到最右边生成的僵尸数组
    private GameObject[] zombie = new GameObject[10];
    //泥土的预制体
    public GameObject soilPrefab;

    //种植植物的音效
    public AudioClip plantClip;
    //阳光点击的音效
    public AudioClip sunClickClip;
    //僵尸快要来时的音效
    public AudioClip zombieComing;
    //生成阳光的速度控制
    public int createSunSpeed = 3;
    //进度条的预制体
    public GameObject progressBarPrefab;
    //进度条上的僵尸头
    private GameObject flagMeterHead;
    //进度条的小红旗
    private GameObject flagMeterFull;
    //僵尸的总波叔
    public int zombieWave = 10;
    //下一波僵尸一定到来的时间
    private int nextZombieWaveTime;
    //统计当前的存在的僵尸个数
    public int totalZombie = 0; 
    //进度条移动的速度
    private float progressBarSpeed = 0.01f;
    //小推车的预制体
    public GameObject lawnCleanerPrefab;
    //保存是实例化小推车后生成的物体
    private GameObject[] lawnCleaner = new GameObject[5];
    //小推车初始时往前推一下的速度
    public float lawnCleanerSpeed;
    //小推车初始化的y坐标
    private int[] lawnCleanery = { 172, 87, -15, -112, -211 };
    public GameObject shovelSlotPrefab;
    public GameObject shovelPrefab;
    private GameObject shovel;
    private bool isShovelClicked = false;

    private void Start()
    {
        //使用携程
        StartCoroutine(ShowText(1));
    }

    /*
     * 鼠标的点击时间放在一起比较好控制
     */
    private void Update()
    {
        //获取所有标签为zombie的物体
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("zombie");
        totalZombie = zombies.Length;//然后总个数就是上面数组的长度

        if (moveState == 1)
        {
            //相机移动 同时卡片选择槽也要同时移动 不然不同步
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(300, 0, -10), moveSpeed * Time.deltaTime);
            chooseBk.transform.position = Vector3.MoveTowards(chooseBk.transform.position, new Vector3(-232 + 300, 256, 0), moveSpeed * Time.deltaTime);
            if (camera.transform.position.x == 300)//移动到最右边了
            {
                moveState = 0;//及时赋值为0 不然后面出问题 因为上面那个程序被调用了多次
                for(int i = 0; i < 10; i++)
                {
                    int x = Random.Range(609, 757);//随机生成x坐标
                    int y = Random.Range(-128, 174);//随机生成y坐标
                    //实例化僵尸数组
                    zombie[i] = Instantiate(zombiePrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                Destroy(hintText);//删除提示文本
            }
        }
        else if (moveState == 2)
        {
            //相机和卡片选择栏的 移动回去
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(0, 0, -10), moveSpeed * Time.deltaTime);
            chooseBk.transform.position = Vector3.MoveTowards(chooseBk.transform.position, new Vector3(-232, 256, 0), moveSpeed * Time.deltaTime);
            if (camera.transform.position.x == 0)//移动回去了 开始游戏
            {
                moveState = 0;
                InvokeRepeating("UpdateUI", 0, 0.1f);//更新UI的显示
                InvokeRepeating("SpawnSun", createSunSpeed, createSunSpeed);//持续产生阳光
                StartCoroutine(SpawnZombieFirst(6));//第一波僵尸6秒生成

                for (int i = 0; i < 10; i++)
                {
                    Destroy(zombie[i]);//移动完成后僵尸销毁
                }
                
                for(int i = 0; i < 5; i++)
                {
                    lawnCleaner[i] = Instantiate(lawnCleanerPrefab, new Vector3(-393, lawnCleanery[i], 0), Quaternion.identity);
                }

                Instantiate(shovelSlotPrefab, new Vector3(70, 256, 0), Quaternion.identity);
               shovel = Instantiate(shovelPrefab, new Vector3(68, 254, 0), Quaternion.identity);
            }
        }

        if (Input.GetMouseButtonDown(0))//左键
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    switch(c.tag)
                    {
                        case "ground":
                            //获取虚化的植物对象 然后在该位置种植植物
                            GameObject blurPlant = GameObject.FindGameObjectWithTag("blur_plant");
                            if (blurPlant != null)
                            {
                                //实例化植物对象
                                Instantiate(clickedCard.GetComponent<Card>().plantPrefab, blurPlant.transform.position, Quaternion.identity);
                                //种植植物后让取消碰撞器材 不再进行碰撞检测
                                //c.GetComponent<BoxCollider2D>().enabled = false;

                                isHadPlanting = true;//植物已经种植
                                sunNum -= clickedCard.GetComponent<Card>().cost;//阳光减少
                                Destroy(withMousePlant);

                                Instantiate(soilPrefab, new Vector3(blurPlant.transform.position.x, blurPlant.transform.position.y - 32, 0), Quaternion.identity);
                                
                                AudioSource.PlayClipAtPoint(plantClip, Vector3.zero);
                            }
                            break;
                        case "card":
                            isClickedCard = true;//点击卡片
                            isHadPlanting = false;//还没有种植
                            //保存点击的卡片对象
                            clickedCard = c.gameObject;
                            //实例化跟随鼠标移动的对象
                            withMousePlant = Instantiate(clickedCard.GetComponent<Card>().cardPlantPrefab, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
                            //更改跟随鼠标移动对象的显示
                            withMousePlant.GetComponent<SpriteRenderer>().sprite = clickedCard.GetComponent<Card>().cardPlantSprite;
                            clickedCard.GetComponent<Card>().cardBk = Instantiate(clickedCard.GetComponent<Card>().cardBkPrefab, clickedCard.transform.position, Quaternion.identity);
                            break;
                        case "sun":
                            //点击阳光后播放音效
                            AudioSource.PlayClipAtPoint(sunClickClip, Vector3.zero);
                            c.GetComponent<sunMove>().isClicked = true;
                            break;
                        case "shovel":
                            print("shovel");
                            isShovelClicked = true;
                            break;
                        case "plant":
                            if (isShovelClicked)
                            {
                                Destroy(c.gameObject);
                                isShovelClicked = false;
                                shovel.transform.position = new Vector3(68, 254, 0);
                            }
                            break;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))//右键
        {
            if (isClickedCard)
            {
                Destroy(withMousePlant);//按下右键有摧毁跟随鼠标移动的对象
                Destroy(clickedCard.GetComponent<Card>().cardBk);
                isClickedCard = false;
            }
            if (isShovelClicked)
            {
                isShovelClicked = false;
                shovel.transform.position = new Vector3(68, 254, 0);
            }
            
        }
        else if (Input.GetMouseButtonDown(2))//中键
        {

        }

        //当需要种植植物的时候实例化出来一个植物跟随鼠标移动
        if (withMousePlant != null||isShovelClicked)
        {
            //获取鼠标在相机中（世界中）的位置，转换为屏幕坐标；
            screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            //获取鼠标在场景中坐标
            mousePositionOnScreen = Input.mousePosition;
            //让场景中的Z=鼠标坐标的Z
            mousePositionOnScreen.z = screenPosition.z;
            //将相机中的坐标转化为世界坐标
            mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            if(withMousePlant != null)
            {
                //物体跟随鼠标移动
                withMousePlant.transform.position = mousePositionInWorld;
            }
            else if (isShovelClicked)
            {
                shovel.transform.position = mousePositionInWorld;
            }
        }

        if (flagMeterFull != null)//如果进度条不为空
        {
            float temp = (float)((10 - zombieWave) / 10.0);//总波数为10 变化量
            //改变进度条的scale 同时必须改变position 因为scale是从两边来压缩和伸展的
            flagMeterFull.transform.localScale = Vector3.MoveTowards(flagMeterFull.transform.localScale, new Vector3(-temp, 1, 1), progressBarSpeed * Time.deltaTime);
            flagMeterFull.transform.localPosition = Vector3.MoveTowards(flagMeterFull.transform.localPosition, new Vector3(72 - temp*70, 0, 0), progressBarSpeed * Time.deltaTime*70);
            //改变僵尸头的位置
            flagMeterHead.transform.localPosition = Vector3.MoveTowards(flagMeterHead.transform.localPosition, new Vector3(61 - temp*130, 0, 0), progressBarSpeed * Time.deltaTime*130);
        }

        for(int i = 0; i < 5; i++)
        {
            if (lawnCleaner[i] != null)
            {
                //由快到慢
                if (lawnCleaner[i].GetComponent<LawnCleaner>().startMove == false)
                {
                    lawnCleaner[i].transform.position = Vector3.Lerp(lawnCleaner[i].transform.position, new Vector3(-336, lawnCleanery[i], 0), lawnCleanerSpeed * Time.deltaTime);
                } 
            }
        }
    }

    /*
     * 初始的时候显示文字的
     */
    IEnumerator ShowText(float t)
    {
        yield return new WaitForSeconds(t);//第一次等待时间
        hintText.text = PlayerPrefs.GetString("username", "SmallZombieZombie") + "的房子";
        hintText.color = new Vector4(255, 255, 255, 255);//显示文本 把Text从透明到不透明
        yield return new WaitForSeconds(3);//第二次显示时间
        moveState = 1;
        yield return new WaitForSeconds(3);//第三次显示时间
        moveState = 2;
    }

    /*
     * 第一次生成僵尸
     */
    IEnumerator SpawnZombieFirst(float t)
    {
        yield return new WaitForSeconds(t);//第一次等待时间
        AudioSource.PlayClipAtPoint(zombieComing, Vector3.zero);//播放僵尸快来的音效
        yield return new WaitForSeconds(2);//第二次等待时间
        StartCoroutine(SpawnZombie(0.1f, true));//生成僵尸
        zombieWave -= 1;//波数-1
        //实例化进度条对象
        GameObject progressBar = Instantiate(progressBarPrefab, new Vector3(351, -286, 0), Quaternion.identity);
        //查找进度条下的子物体
        Transform[] father = progressBar.GetComponentsInChildren<Transform>();
        foreach (Transform child in father)
        {
            if(child.name == "FlagMeterParts1")//僵尸头
            {
                flagMeterHead = child.gameObject;
            }
            else if(child.name == "FlagMeterFull2")//进度条满状态的时候
            { 
                flagMeterFull = child.gameObject;
                flagMeterFull.transform.localScale = new Vector3(0, 1, 1);
                flagMeterFull.transform.localPosition = new Vector3(72, 0, 0);
            }
        }
        nextZombieWaveTime = Random.Range(25, 31);//下一波生成的绝对时间 到这个时间必须生成下一波僵尸
        InvokeRepeating("SpawnZombieGoon", 1, 1);//后续僵尸的生成
    }

    int times = 0;
    void SpawnZombieGoon()
    {
        times++;
        float totalBlood = 0;//总的血量
        float curtotalBlood = 0;//当前的总血量
        if (times > nextZombieWaveTime)
        {
            StartCoroutine(SpawnZombie(0.1f, false));//生成下一波僵尸
            zombieWave -= 1;
            nextZombieWaveTime = Random.Range(25, 31);//重新生成时间
            times = 0;//计时清零
            return;
        }

        for(int i = 0; i < totalZombie; i++)
        {
            if (zombie[i] != null)
            {
                curtotalBlood += zombie[i].GetComponent<Helth>().bloodNumber;//统计当前血量
                totalBlood += zombie[i].GetComponent<Helth>().blood;//统计总的血量
            }
        }
        
        if(totalBlood< totalBlood * 0.5)//当前血量小于总血量的时候一半的时候生成下一波
        {
            StartCoroutine(SpawnZombie(0.1f, false));//生成下一波
            zombieWave -= 1;
            nextZombieWaveTime = Random.Range(25, 31);//下一波时间重新生成
            return;
        }
    }

    /*
     * 生成阳光
     */
    void SpawnSun()
    {
        int x = Random.Range(-300, 250);
        GameObject sun = Instantiate(sunPrefab, new Vector3(x, 242, 0), Quaternion.identity);
        sun.GetComponent<sunMove>().isSunflowerCreate = false;
    }

    /*
     * 生成僵尸
     */
    IEnumerator SpawnZombie(float t,bool first)
    {
        yield return new WaitForSeconds(t);//第一次等待时间
        int count = first ? 1: Random.Range(1, 5);
        for (int i = 0; i < count; i++)
        {
            int y = Random.Range(0, 5);
            zombie[totalZombie] = Instantiate(zombiePrefab, new Vector3(530, spawnY[y], 0), Quaternion.identity);
            yield return new WaitForSeconds(2);//每一次生成都间隔2秒
        }
    }

    /*
     * 更新UI的显示
     */
    void UpdateUI()
    {
        sunText.text = "" + sunNum;
    }
}
