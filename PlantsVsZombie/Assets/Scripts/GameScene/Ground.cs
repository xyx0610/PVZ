using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 地面的脚本
 */
public class Ground : MonoBehaviour
{
    public GameObject plant;//存储地面上的植物
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "plant")
        {
            plant = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "plant")
        {
            plant = null;
        }
    }
}
