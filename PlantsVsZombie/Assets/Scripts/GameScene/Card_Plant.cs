using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *点击植物卡片后生成的植物对象的脚本 
 */
public class Card_Plant : MonoBehaviour
{
    //虚化的植物预制体
    public GameObject blurPlantPrefab;
    //实例化虚化植物的返回值
    private GameObject plant;

    //碰撞检测 待在碰撞体中除法
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            if (plant == null&&collision.GetComponent<Ground>().plant==null)
            {
                //实例化虚化的植物
                plant = Instantiate(blurPlantPrefab, collision.gameObject.transform.position, Quaternion.identity);
                //更改虚化植物的显示 sprite
                plant.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            //移出碰撞器之后销毁物体
            Destroy(plant);
        }
    }
}
