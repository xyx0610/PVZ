using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ×Óµ¯µÄ½Å±¾
 */
public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public GameObject bulletHitPrefab;
    private void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(700, gameObject.transform.position.y, gameObject.transform.position.z), moveSpeed * Time.deltaTime);
        if (gameObject.transform.position.x == 700)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "zombie")
        {
            Instantiate(bulletHitPrefab, gameObject.transform.position, Quaternion.identity);
            collision.GetComponent<Helth>().AcceptDamage(1);
            Destroy(gameObject);
        }
    }
}
