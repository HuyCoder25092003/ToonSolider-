using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopViewItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text value_lb;
    public TMP_Text cost_lb;
    private ConfigShopRecord cf;
    public void Setup(ConfigShopRecord cf)
    {
        this.cf = cf;
        name_lb.text=cf.Name;
        if(cf.Shop_type==1)
        {
             value_lb.text = $"<color=#FFBE23>{cf.Value}</color>";
        }
        else
        {
            value_lb.text = $"<color=#F9392D>{cf.Value}</color>";
        }
       
        cost_lb.text = cf.Price;
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(cf.Image);
    }
    public void OnBuy()
    {
        DataController.instance.OnShopBuy(cf);
    }
}
