using UnityEngine;

public class Card : MonoBehaviour
{
    //植物卡片种植的cd
    public float cd;
    //植物卡片的花费
    public int cost;
    //卡片点击后的阴影预制体
    public GameObject cardBkPrefab;
    //卡片对应的植物预制体
    public GameObject plantPrefab;
    //跟随鼠标移动的植物预制体
    public GameObject cardPlantPrefab;
    //跟随鼠标移动的植物预制体的sprite
    public Sprite cardPlantSprite;
    //阴影实例化后的返回值
    public GameObject cardBk;
    //植物卡片费用不够时的图片
    public Sprite disablePrefab;
    //植物卡片费用充足时的图片
    public Sprite enablePrefab;
    //植物卡片是否可以被点击的标志
    private bool isCanClick = true;
    private void Update()
    {
        //当前阳光数量小于花费的时候 卡片变灰且不可以点击
        if (GameManage.sunNum < cost)
        {
            isCanClick = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = disablePrefab;
        }
        else
        {
            isCanClick = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = enablePrefab;
        }
        if (GameManage.isHadPlanting)//已经种植的标志
        {
            if (cardBk != null)
            {
                //冷却时间不可以点击
                isCanClick = false;
                //通过缩小物体的方法实现阴影的变化
                cardBk.transform.localScale = Vector3.MoveTowards(cardBk.transform.localScale, new Vector3(1, 0, 1), cd * Time.deltaTime);
                cardBk.transform.localPosition = Vector3.MoveTowards(cardBk.transform.localPosition, new Vector3(cardBk.transform.localPosition.x, 263+(1- cardBk.transform.localScale.y)*35, 0), cd * Time.deltaTime* 35);
                //缩没了之后销毁物体
                if (cardBk.transform.localScale.y == 0)
                {
                    isCanClick = true;
                    Destroy(cardBk);
                }
            }
        }
        //如果卡片不可以点击 则失能它的碰撞器组件
        gameObject.GetComponent<BoxCollider2D>().enabled = isCanClick ? true : false;
    }
}
