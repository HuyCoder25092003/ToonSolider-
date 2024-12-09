using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InfoUnitDialog : BaseDialog
{
    public GameObject[] rare_objects;
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text level;
    public TMP_Text stamina_lb;
    public TMP_Text cd_lb;
    private ConfigUnitRecord config_unit;
    public Button btn_unlock;
    public Button btn_Upgrade;
    private UnitData data;
    public TMP_Text cost_lb;
    private ConfigUnitLevelRecord cf_unit_lv;
    private InfoUnitDialogParam dl_param;
    int gold;

    public InfoDialogStatControl hp_stat;

    public InfoDialogStatControl damage_stat;

    public InfoDialogStatControl attack_range_stat;

    public InfoDialogStatControl attack_speed_stat;

    public InfoDialogStatControl power_skill_stat;
    public override void Setup(DialogParam param)
    {
        dl_param = (InfoUnitDialogParam)param;   
        UpdateData(null);
    }
    public override void OnShowDialog()
    {
        DataTrigger.RegisterValueChange(DataSchema.DIC_UNIT + "/" + config_unit.ID.Tokey(), UpdateData);
    }
    public override void OnHideDialog()
    {
        DataTrigger.UnRegisterValueChange(DataSchema.DIC_UNIT + "/" + config_unit.ID.Tokey(), UpdateData);
    }
    private void UpdateData(object dataChange)
    {
        int lv = 1;
        gold = DataController.instance.GetGold();
        cf_unit_lv = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(dl_param.cf_unit.ID);
        config_unit = dl_param.cf_unit;
        cd_lb.text = $"{config_unit.Cool_down}";
        stamina_lb.text = $"{config_unit.Stamina}";
        name_lb.text = config_unit.Name;
        //  
        for (int i = 0; i < rare_objects.Length; i++)
        {
            rare_objects[i].SetActive(i + 1 == (int)config_unit.Rare);
        }
        cost_lb.gameObject.SetActive(true);
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);
        data = DataController.instance.GetUnitData(config_unit.ID);
        if (data != null)
        {
            lv = data.level;
            level.text = $"Lv {data.level}";
            btn_unlock.gameObject.SetActive(false);
            btn_Upgrade.gameObject.SetActive(true);
            int costlevel_next = cf_unit_lv.GetCost(data.level + 1);
            cost_lb.text = $"{costlevel_next}";
            btn_Upgrade.interactable = gold >= costlevel_next;
            if(lv>= cf_unit_lv.Maxlv)
            {
                level.text = "MAX LV";
                btn_Upgrade.gameObject.SetActive(false);
                cost_lb.gameObject.SetActive(false) ;
            }
        }
        else
        {
            lv = 1;
            level.text = "Lv 1";
            int min_cost = cf_unit_lv.GetCost(1);
            cost_lb.text = $"{min_cost}";
            btn_unlock.gameObject.SetActive(true);
            btn_Upgrade.gameObject.SetActive(false);
            btn_unlock.interactable = gold >= min_cost;
        }

        // hp
        int hp_cur = cf_unit_lv.GetHP(lv);
        hp_stat.SetupStat(hp_cur, cf_unit_lv.Max_hp);

        // famage
        int damage_cur = cf_unit_lv.GetDamage(lv);
        damage_stat.SetupStat(damage_cur, cf_unit_lv.Max_damage);

        // range
        int range_cur = cf_unit_lv.GetRange(lv);
        attack_range_stat.SetupStat(range_cur, cf_unit_lv.Max_range);

        // hp
        float speed_cur = cf_unit_lv.GetAttackSpeed(lv);
        attack_speed_stat.SetupStatInvert(speed_cur, cf_unit_lv.Max_attack_speed);

        // hp
        int power_skill_cur = cf_unit_lv.GetPowerSkill(lv);
        power_skill_stat.SetupStat(power_skill_cur, cf_unit_lv.Max_power_skill);

    }
    public void OnClose()
    {
        DialogManager.instance.HideDialog(DialogIndex.InfoUnitDialog);
    }
    public void OnUpgrade()
    {
        DataController.instance.UpgradeUnit(cf_unit_lv, () =>
        {

        });
    }

    public void OnUnlock()
    {
        DataController.instance.UnlockUnit(cf_unit_lv, () =>
        {

        });
    }
}
