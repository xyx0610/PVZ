using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 子弹打击之后的控制脚本 主要是该子弹没有动画 所以需要脚本增加控制动画
 */
public class PeaBulletHit : MonoBehaviour
{
    public float bfSpeed;
    void Update()
    {
        gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), bfSpeed * Time.deltaTime);
        if (gameObject.transform.localScale.x == 1.5f)
        {
            Destroy(gameObject);
        }
    }
}
