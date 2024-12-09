using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : BaseView
{
    public Transform parent_item;
    public ShopViewItem prefab_item;
    private List<ShopViewItem> items = new List<ShopViewItem>();
    // Start is called before the first frame update
    public override void Setup(ViewParam param)
    {
        base.Setup(param);
        List<ConfigShopRecord> configShops = ConfigManager.instance.configShop.records;

        if (items.Count == 0)
        {
            for (int i = 0; i < configShops.Count; i++)
            {
                ShopViewItem item = Instantiate(prefab_item);
                items.Add(item);
                item.transform.SetParent(parent_item, false);
            }
        }

        for(int i=0;i<configShops.Count;i++)
        {
            items[i].Setup(configShops[i]);
        }
       
    }
    public void OnBack()
    {
        ViewManager.instance.SwitchView(ViewIndex.HomeView);
    }
}
