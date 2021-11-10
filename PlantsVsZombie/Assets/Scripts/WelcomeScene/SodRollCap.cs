using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SodRollCap : MonoBehaviour
{
    public float moveSpeed;
    public float scaleSpeed;
    public float rotationSpeed;
    private float rotation = 0;
    public Image loadBarGrass;

    void Update()
    {
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y-(1-transform.localScale.y)*Time.deltaTime*60, 0);
        loadBarGrass.rectTransform.sizeDelta = new Vector2(loadBarGrass.rectTransform.sizeDelta.x + (moveSpeed + 5) * Time.deltaTime, loadBarGrass.rectTransform.sizeDelta.y);
        loadBarGrass.transform.localPosition = new Vector3(loadBarGrass.transform.localPosition.x + (moveSpeed + 5) * Time.deltaTime / 2, loadBarGrass.transform.localPosition.y, 0);
        moveSpeed += 100 * Time.deltaTime;
        
        transform.localScale = new Vector3(transform.localScale.x - scaleSpeed * Time.deltaTime, transform.localScale.y - scaleSpeed * Time.deltaTime, 0);
        scaleSpeed +=(float) 0.15 * Time.deltaTime;
        rotation -= rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        if (transform.position.x > 154)
        {
            Destroy(gameObject);
            GameObject.Find("Start").GetComponent<Text>().color = new Color(212 / 255f, 176 / 255f, 32 / 255f, 255 / 255f);
        }
    }
}
