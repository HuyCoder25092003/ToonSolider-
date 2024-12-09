using System;
using System.Collections.Generic;
using UnityEngine;

public class DataController : BYSingletonMono<DataController>
{
    [SerializeField] DataModel dataModel;
    public void CheckDataFirebase(PlayerData data, Action callback)
    {
        if (data == null)
        {
            Debug.LogError("Data firebase null");
            InitDataLocal(callback);
        }
        else
        {
            Debug.LogError(" data cloud version: " + data.version_data);
            dataModel.CheckDataLocalWithCloud(data);
            callback.Invoke();
        }
        FirebaseDataControl.instance.SaveAllData();
    }
    public void InitDataLocal(Action callback)
    {
        dataModel.InitData(callback);
    }
    public PlayerInfo GetPlayerInfo()
    {
        PlayerInfo info = dataModel.ReadData<PlayerInfo>(DataSchema.INFO);
        return info;
    }
    public int GetGem()
    { 
        return dataModel.ReadData<int>(DataSchema.GEM);
    }
    public int GetGold()
    {
       return dataModel.ReadData<int>(DataSchema.GOLD);
    }
    public void AddGold(int number)
    {
        int gold = GetGold();
        gold += number;
        if (gold < 0)
            gold = 0;
        dataModel.UpdateData(DataSchema.GOLD, gold);
    }
    public void AddGem(int number)
    {
        int gem = GetGem();
        gem += number;
        if (gem < 0)
            gem = 0;
        dataModel.UpdateData(DataSchema.GEM, gem);
    }
    public void UpdateUnitLevel(int id)
    {
        UnitData unit = dataModel.ReadDicData<UnitData>(DataSchema.DIC_UNIT, id.Tokey());
        unit.level++;
        dataModel.UpdateDicData<UnitData>(DataSchema.DIC_UNIT, id.Tokey(), unit);

    }
    public UnitData GetUnitData(int id)
    {
        return dataModel.ReadDicData<UnitData>(DataSchema.DIC_UNIT, id.Tokey()) ;
    }
    public void UnlockUnit(ConfigUnitLevelRecord configUnitLevelRecord,Action callback)
    {
        UnitData unitData = GetUnitData(configUnitLevelRecord.ID);
        if(unitData==null)
        {
            unitData = new UnitData();
            unitData.id = configUnitLevelRecord.ID;
            unitData.level = 1;
            int gold = GetGold();
            int min_cost= configUnitLevelRecord.GetCost(1);
            if (gold>= min_cost)
            {
                gold -= min_cost;
                dataModel.UpdateData(DataSchema.GOLD, gold);
                 dataModel.UpdateDicData<UnitData>(DataSchema.DIC_UNIT, unitData.id.Tokey(),unitData);

            }
        }
        callback();
    }
    public void UpgradeUnit(ConfigUnitLevelRecord cf_unit_lv, Action callback)
    {
        UnitData unitData = GetUnitData(cf_unit_lv.ID);
        if (unitData != null)
        {
            if(unitData.level<cf_unit_lv.Maxlv)
            {
                int costlevel_next = cf_unit_lv.GetCost(unitData.level + 1);
                int gold = GetGold();
                if (gold >= costlevel_next)
                {
                    unitData.level = unitData.level + 1;
                    gold -= costlevel_next;
                    dataModel.UpdateData(DataSchema.GOLD, gold);
                    dataModel.UpdateDicData<UnitData>(DataSchema.DIC_UNIT, unitData.id.Tokey(), unitData);

                }
            }
           
        }
        callback();
    }
    public void OnShopBuy(ConfigShopRecord cf)
    {
        if(cf.Shop_type==1)
        {
            AddGold(cf.Value);
        }
        else
        {
            AddGem(cf.Value);
        }
    }
    public List<UnitData> GetDeck()
    {
        return dataModel.ReadData<List<UnitData>>(DataSchema.DECK);
    }
    public void ChangeDeck(UnitData unitData,int index)
    {
        List<UnitData> deck= dataModel.ReadData<List<UnitData>>(DataSchema.DECK);
        deck[index] = unitData;
        dataModel.UpdateData(DataSchema.DECK, deck);
    }
}
