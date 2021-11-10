using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManageWelcome : MonoBehaviour
{
    void Update()
    {
        Collider2D[] c = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (GameObject.Find("SodRollCap") == null)
        {
            if (c.Length > 0)
            {
                foreach (Collider2D d in c)
                {
                    switch (d.name)
                    {

                        case "Start":
                            GameObject.Find("Start").GetComponent<Text>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                            if (Input.GetMouseButtonDown(0))
                            {
                                SceneManager.LoadScene(1);
                            }
                            break;
                    }
                }
            }
            else
            {
                //设置文本的颜色需要除255f
                GameObject.Find("Start").GetComponent<Text>().color = new Color(212 / 255f, 176 / 255f, 32 / 255f, 255 / 255f);
            }
        }
    }
}
