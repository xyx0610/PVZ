using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 关键帧触发事件 
 * 关键帧触发的 但是也需要挂载在物体上
 */
public class Shoot : MonoBehaviour
{
    //子弹的预制体
    public GameObject bulletPrefab;
    //攻击的音效
    public AudioClip shootClip;
    void SpawnBullet()
    {
        //判断植物所在的那一行 是否存在僵尸 存在僵尸才发起攻击
        if (gameObject.GetComponent<PlantRow>().isHaveZombie)
        {
            //实例化子弹
            Instantiate(bulletPrefab, new Vector3(gameObject.transform.position.x + 23, gameObject.transform.position.y + 24, gameObject.transform.position.z), Quaternion.identity);
            //播放攻击音效
            AudioSource.PlayClipAtPoint(shootClip, Vector3.zero);
        }
    }
}
