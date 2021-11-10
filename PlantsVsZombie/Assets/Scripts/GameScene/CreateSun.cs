using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 生成阳光的脚本
 */
public class CreateSun : MonoBehaviour
{
    //阳光的预制体
    public GameObject sunPrefab;
    //阳光的生成速度
    private float createSpeed = 10f;
    private float firstCreateSpeed = 3f;
    private bool isFirstCreate = true;
    private bool isHadSpawn = false;

    private void Update()
    {
        AnimatorStateInfo animatorInfo = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if(isHadSpawn == false)
        {
            if (isFirstCreate)
            {
                if (animatorInfo.normalizedTime >= firstCreateSpeed)
                {
                    Spawn();
                    isHadSpawn = true;
                    isFirstCreate = false;
                }
            }
            else
            {
                if (animatorInfo.normalizedTime >= createSpeed)
                {
                    Spawn();
                    isHadSpawn = true;
                }
            }
        }

        if (animatorInfo.IsName("sunFlowerCreate"))
        {
            isHadSpawn = false;
        }
    }
    //生成阳光的方法
    void Spawn()
    {
        gameObject.GetComponent<Animator>().SetTrigger("isCreateSun");
    }

    void SpawnSun()
    {
        //实例化阳光
        GameObject sun = Instantiate(sunPrefab, transform.position, Quaternion.identity);
        //更改阳光的初始化位置
        sun.transform.position = new Vector3(transform.position.x - 4, transform.position.y + 19, transform.position.z);
        //更改阳光的透明度
        sun.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 233);
    }
}
