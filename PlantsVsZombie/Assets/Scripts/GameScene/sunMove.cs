using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 阳光的脚本
 * 控制阳光的移动
 */
public class sunMove : MonoBehaviour
{
    //太阳花生成阳光后移动的最高高度
    int height = 60;
    //阳光的初始高度
    float origin_y;
    //阳光移动的方向控制
    bool dir = false;

    //判断阳光是否被点击
    public bool isClicked = false;
    //阳光移动的速度
    public float moveSpeed = 1f;
    //阳光变小的速度
    public float shrinkSpeed = 2f;

    //是否是太阳花生成的阳光
    public bool isSunflowerCreate = true;
    //阳光下降的速度
    public float fallSpeed = 100f;
    

    void Start()
    {
        if (isSunflowerCreate)
        {
            //保存最开始的y坐标
            origin_y = gameObject.transform.position.y - 30;
            //重复执行移动函数
            InvokeRepeating("move", 0, 0.02f);
        }
    }

    private void Update()
    {
        if (isClicked)
        {
            CancelInvoke();
            //由快到慢移动到指定位置
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,new Vector3(-460, 280, 0), moveSpeed * Time.deltaTime);
            //由大到小变化
            //gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), shrinkSpeed * Time.deltaTime);
        }

        if (isSunflowerCreate == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, -200, gameObject.transform.position.z), fallSpeed * Time.deltaTime);
        }
    }

    void move()
    {
        //获取物体的当前坐标
        Vector3 position = gameObject.transform.position;
        if (position.y - origin_y > height)
        {
            dir = true;//移动位置大于指定高度后方向取反
        }
        if (position.y == origin_y&&dir)
        {
            CancelInvoke();//回到原来的位置后取消重复执行的函数
        }
        if (dir)
        {
            //向下移动
            gameObject.transform.position = new Vector3(position.x, position.y - 3, position.z);
        }
        else
        {
            //向上移动
            gameObject.transform.position = new Vector3(position.x-1.5f, position.y + 3, position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "SunCollection")
        {
            GameManage.sunNum += 25;
            Destroy(gameObject);
        }
    }
}
