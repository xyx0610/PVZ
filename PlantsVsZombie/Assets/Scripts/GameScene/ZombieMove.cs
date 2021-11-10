using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 僵尸的脚本
 * 控制僵尸的各种动画
 */
public class ZombieMove : MonoBehaviour
{
    //移动速度
    public float moveSpeed;
    //是否开始攻击
    private bool isHitPlant = false;
    //保存攻击的植物对象
    private GameObject hitPlant;
    //僵尸是否死亡标志
    public bool isDie=false;
    //保存实例化僵尸头部返回的对象
    private GameObject zombieHead;
    void Update()
    {
        //寻找子对象
        Transform[] father = GetComponentsInChildren<Transform>();
        foreach (Transform child in father)
        {
            if (child.name == "ZombieHead")
            {
                zombieHead = child.gameObject;//保存子对象 也就是僵尸的头部
            }
        }
        if (isHitPlant == false&&isDie==false)//如果僵尸没有 攻击植物和死亡的情况下才能攻击植物
        {
            //僵尸的移动
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(-484, gameObject.transform.position.y, gameObject.transform.position.z), moveSpeed * Time.deltaTime);
        }
        else
        {
            if (hitPlant != null)
            {
                //增量时间的用法 每一秒造成1的伤害
                hitPlant.GetComponent<Helth>().AcceptDamage(1 * Time.deltaTime);
            }
        }

        if (gameObject.GetComponent<Helth>().bloodNumber < gameObject.GetComponent<Helth>().blood/3)
        {
            //僵尸血量小于2的时候播放掉头的动画 和没有头行走的动画
            if (zombieHead != null)
            {
                zombieHead.GetComponent<Animator>().SetTrigger("isWillDie");
            }
            gameObject.GetComponent<Animator>().SetTrigger("isWillDie");
        }

        if (zombieHead != null)
        {
            //头部动画播放完成后删除物体
            AnimatorStateInfo anifo = zombieHead.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (anifo.normalizedTime >= 1f && anifo.IsName("zombieHead"))
            {
                Destroy(zombieHead);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "plant")
        {
            isHitPlant = true;
            gameObject.GetComponent<Animator>().SetTrigger("isAttack");//播放攻击动画
            hitPlant = collision.gameObject;//保存正在被攻击的植物 然后执行减血操作
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //植物被消灭 继续前进
        if (collision.tag == "plant")
        {
            isHitPlant = false;
            gameObject.GetComponent<Animator>().SetTrigger("isPlantDie");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
