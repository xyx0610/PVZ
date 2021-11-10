using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManageHelp : MonoBehaviour
{
    public float speed;
    public GameObject blackBackground;
    public Sprite[] buttonSprite = new Sprite[2];
    private GameObject button;

    void Update()
    {
        if (blackBackground != null)
        {
            blackBackground.GetComponent<SpriteRenderer>().color = Vector4.MoveTowards(blackBackground.GetComponent<SpriteRenderer>().color, new Vector4(255, 255, 255, 0), speed * Time.deltaTime);
            if (blackBackground.GetComponent<SpriteRenderer>().color.a == 0)
            {
                Destroy(blackBackground);
            }

        }

        Collider2D[] c = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (c.Length > 0)
        {
            foreach (Collider2D d in c)
            {
                if (d.name == "MainMenu")
                {
                    d.GetComponent<Image>().sprite = buttonSprite[1];
                    button = d.gameObject;
                    Transform[] father = d.GetComponentsInChildren<Transform>();
                    foreach (Transform child in father)
                    {
                        if (child.name == "Text")
                        {
                            child.GetComponent<Text>().color = new Color(255 / 255f, 252 / 255f, 98 / 255f);
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        SceneManager.LoadScene(1);
                    }
                }
            }
        }
        else
        {
            if (button != null)
            {
                button.GetComponent<Image>().sprite = buttonSprite[0];
                Transform[] father = button.GetComponentsInChildren<Transform>();
                foreach (Transform child in father)
                {
                    if (child.name == "Text")
                    {
                        child.GetComponent<Text>().color = new Color(212 / 255f, 158 / 255f, 42 / 255f);
                    }
                }
            }
        }
    }
}
