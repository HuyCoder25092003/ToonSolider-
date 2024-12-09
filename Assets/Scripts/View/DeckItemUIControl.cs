using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckItemUIControl : MonoBehaviour
{
    public GameObject[] rare_objects;
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text level;
    public GameObject btn_Equip;
    public GameObject btn_info;
    private ConfigUnitRecord config_unit;
    private UnitData data;
 
    public void Setup(UnitData data_)
    {
        config_unit = ConfigManager.instance.configUnit.GetRecordBykeySearch(data_.id);
        data = DataController.instance.GetUnitData(data_.id);
        name_lb.text = config_unit.Name;
        ConfigUnitLevelRecord cf_level = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(config_unit.ID);
        if (data.level < cf_level.Maxlv)
            level.text = $"Lv {data.level}";
        else
            level.text = "MAX LV ";
        for (int i = 0; i < rare_objects.Length; i++)
        {
            rare_objects[i].SetActive(i + 1 == (int)config_unit.Rare);
        }
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);
        btn_info.SetActive(true);
        btn_Equip.SetActive(false);

    }
    public void ShowInfo()
    {
        InfoUnitDialogParam param = new InfoUnitDialogParam { cf_unit = config_unit };
        DialogManager.instance.ShowDialog(DialogIndex.InfoUnitDialog,param);
    }
  
}
