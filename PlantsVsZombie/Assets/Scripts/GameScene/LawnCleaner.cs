using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 小推车的脚本文件
 */
public class LawnCleaner : MonoBehaviour
{
    public float moveSpeed;
    public bool startMove = false;

    private void Update()
    {
        if (startMove)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(586, gameObject.transform.position.y,0), moveSpeed * Time.deltaTime);
            
            if (gameObject.transform.position.x == 586)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "zombie")
        {
            startMove = true;
            collision.GetComponent<Animator>().SetTrigger("isCarCollision");
            collision.GetComponent<ZombieMove>().isDie = true;
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
