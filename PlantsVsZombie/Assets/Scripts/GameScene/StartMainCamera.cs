using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainCamera : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 mouseWorldPos;
    void Update()
    {
        mousePosition = Input.mousePosition; //获取屏幕坐标
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition); //屏幕坐标转世界坐标

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D hit;

    }
}
