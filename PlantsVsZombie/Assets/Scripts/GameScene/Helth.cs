using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 植物和僵尸的生命值脚本
 */
public class Helth : MonoBehaviour
{   
    //血量
    public  float blood = 5;
    public float bloodNumber = 5;

    //收到攻击 如果血量小于0则销毁
    public void AcceptDamage(float damage)
    {
        bloodNumber -= damage;
        if (bloodNumber < 0)
        {
            if (gameObject.tag == "zombie")
            {
                gameObject.GetComponent<Animator>().SetTrigger("isDie");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<ZombieMove>().isDie = true;
                //StartCoroutine(DestoryObject(1.45f));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator DestoryObject(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
