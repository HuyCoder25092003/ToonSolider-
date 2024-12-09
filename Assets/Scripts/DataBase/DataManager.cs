using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : BYSingletonMono<DataManager>
{
    public List<UnitData> deck;
    public PlayerData InitData()
    {
        PlayerData playerData = new PlayerData();
        PlayerInfo info = new PlayerInfo
        {
            nickname = "Brayang",
            level = 1,
            exp = 0,
            deck = deck
        };
        playerData.info = info;

        PlayerInventory inventory = new PlayerInventory
        {
            gold = 100,
            gem = 10
        };
        Dictionary<string, UnitData> dic = new Dictionary<string, UnitData>();
        foreach (UnitData unit in deck)
        {
            dic.Add(unit.id.Tokey(), unit); ;
        }
        inventory.dic_unit = dic;
        playerData.inventory = inventory;

        PlayerMissionData missionData = new PlayerMissionData
        {
            currentMission = 1
        };
        playerData.missionData = missionData;
        return playerData;
    }
}
