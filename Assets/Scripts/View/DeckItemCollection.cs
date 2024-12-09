using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckItemCollection : MonoBehaviour
{
    public GameObject[] rare_objects;
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text level;
    public GameObject btn_Equip;
    public GameObject btn_info;
    public GameObject lock_object;
    private ConfigUnitRecord config_unit;
    private UnitData data;
    public void Setup(ConfigUnitRecord cf)
    {
        data = DataController.instance.GetUnitData(cf.ID);
        config_unit = cf;
        name_lb.text = config_unit.Name;
        for (int i = 0; i < rare_objects.Length; i++)
        {
            rare_objects[i].SetActive(i + 1 == (int)config_unit.Rare);
        }
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);
        if(data!=null)
        {
            ConfigUnitLevelRecord cf_level = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(cf.ID);
            if (data.level < cf_level.Maxlv)
                level.text = $"Lv {data.level}";
            else
                level.text = "MAX LV ";
        }
        else
             level.text = "";
        btn_Equip.SetActive(data != null);
        lock_object.SetActive(data == null);
    }
    public void OnInfo()
    {
        InfoUnitDialogParam param = new InfoUnitDialogParam { cf_unit = config_unit };
        DialogManager.instance.ShowDialog(DialogIndex.InfoUnitDialog, param);
    }
    public void OnEquip()
    {
        DeckEquipDialogParam param = new DeckEquipDialogParam();
        param.unitData = data;
        DialogManager.instance.ShowDialog(DialogIndex.DeckEquipDialog, param);
    }
}
