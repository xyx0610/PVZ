using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 五行碰撞器上的脚本 还是用于本行是否存在僵尸
 */
public class Row : MonoBehaviour
{
    /*
     * 总共五行 每一行上面都一个碰撞器 检测是否有僵尸在上面
     */

    //在这一行是否有僵尸的标志
    public bool isHaveZombie = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "zombie")
        {
            //检测到有僵尸
            isHaveZombie = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "zombie")
        {
            //僵尸没有了
            isHaveZombie = false;
        }
    }
}
