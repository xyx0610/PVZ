using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *挂载在植物上面检测植物一行是否有僵尸
 *因为植物通用 所以单独一个脚本
 */
public class PlantRow : MonoBehaviour
{
    //判断植物所在的这一行是否存在僵尸
    public bool isHaveZombie = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "row")//行的空物体
        {
            //获取物体上是否有僵尸的标志位
            isHaveZombie = collision.GetComponent<Row>().isHaveZombie;
        }
    }
}
