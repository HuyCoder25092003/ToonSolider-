using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEquipDialog : BaseDialog
{
    public GameObject[] rare_objects;
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text level;
    public List<DeckEquipItemControl> deck_items;
    public UnitData cur_UnitData;
    public override void Setup(DialogParam param)
    {
        DeckEquipDialogParam dl_param = (DeckEquipDialogParam)param;
        cur_UnitData = dl_param.unitData;
        List<UnitData> decks = DataController.instance.GetDeck();
        for (int i = 0; i < decks.Count; i++)
        {
            deck_items[i].Setup(decks[i], cur_UnitData, i);
        }

        ConfigUnitRecord config_unit = ConfigManager.instance.configUnit.GetRecordBykeySearch(cur_UnitData.id);
        name_lb.text = config_unit.Name;
        ConfigUnitLevelRecord cf_level = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(config_unit.ID);
        if (cur_UnitData.level < cf_level.Maxlv)
            level.text = $"Lv {cur_UnitData.level}";
        else
            level.text = "MAX LV ";
        for (int i = 0; i < rare_objects.Length; i++)
        {
            rare_objects[i].SetActive(i + 1 == (int)config_unit.Rare);
        }
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);

    }
}
