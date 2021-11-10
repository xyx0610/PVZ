using System;
using UnityEngine;

public class ChangeUsername : MonoBehaviour
{
    //保存鼠标位置
    Vector3 mousePosition;
    //鼠标的世界坐标
    Vector3 mouseWorldPos;
    //点击时x的偏移量
    public int xoff;
    //点击时y的偏移量
    public int yoff;
    //物体的当前x坐标
    private float cUcurx;
    //物体当前的y坐标
    private float cUcury;
    //保存点击按下的时间 老的时间
    private DateTime oldTime;
    //新时间 实时刷新
    private DateTime newTime;
    //判断本次操作是否是点击 还是拖动 才按识别为拖动
    private bool isClick = true;

    private void OnMouseDrag()//鼠标拖动的事件 拖动时每一帧都会调用 只是对点击和拖动不灵敏
    {
        if (isClick == false)
        {
            mousePosition = Input.mousePosition; //获取屏幕坐标
            mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition); //屏幕坐标转世界坐标   
            GameObject changeUsername = GameObject.FindWithTag("ChangeUserName");
            GameObject changeUsernameUI = GameObject.Find("ChangeUsernameUI");
            //把鼠标位置传给物体
            if (changeUsername != null)
            {
                cUcurx = changeUsername.transform.localPosition.x;//保存物体当前坐标
                cUcury = changeUsername.transform.localPosition.y;
                //更改坐标随鼠标一起移动
                changeUsername.transform.localPosition = new Vector3(mouseWorldPos.x - gameObject.transform.localPosition.x + xoff, mouseWorldPos.y - gameObject.transform.localPosition.y + yoff, 0);
                //更改UI的坐标一起移动
                changeUsernameUI.transform.localPosition = new Vector3(changeUsernameUI.transform.localPosition.x + 40 / 56f * (changeUsername.transform.localPosition.x - cUcurx), changeUsernameUI.transform.localPosition.y + 40 / 56f * (changeUsername.transform.localPosition.y - cUcury), 0);
            }
        }
    }

    private void Update()
    {
        newTime = DateTime.Now;
        TimeSpan t = newTime - oldTime;
        if (t.Milliseconds > 120)//如果点击的时间大于120ms就认为是拖动
        {
            isClick = false;
        }
    }

    private void OnMouseDown()
    {
        isClick = true;
        oldTime = DateTime.Now;//保存点击的时间
    }
}
