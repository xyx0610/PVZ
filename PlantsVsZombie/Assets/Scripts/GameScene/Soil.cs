using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 泥土的脚本
 * 播放完动画后销毁物体 采用关键帧的方式也可以
 */
public class Soil : MonoBehaviour
{
    void Update()
    {
        //动画播放完之后销毁物体
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
