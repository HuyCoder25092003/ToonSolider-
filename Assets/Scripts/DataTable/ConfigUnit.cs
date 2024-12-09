using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum CardRare
{
    COMMON = 1,
    UN_COMMON = 2,
    EPIC = 3,
    LEGENDARY = 4
}
[Serializable]
public class ConfigUnitRecord
{
    //id	name	prefab	rare
    [SerializeField]
    private int id;
    public int ID
    {
        get
        {
            return id;
        }
    }

    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }

    [SerializeField]
    private string prefab;
    public string Prefab
    {
        get
        {
            return prefab;
        }
    }
    [SerializeField]
    private CardRare rare;
    public CardRare Rare
    {
        get
        {
            return rare;
        }
    }
    //id	name	prefab	rare
    [SerializeField]
    private int stamina;
    public int Stamina
    {
        get
        {
            return stamina;
        }
    }
    //id	name	prefab	rare
    [SerializeField]
    private int cool_down;
    public int Cool_down
    {
        get
        {
            return cool_down;
        }
    }

}
public class ConfigUnit : BYDataTable<ConfigUnitRecord>
{
    public override ConfigCompare<ConfigUnitRecord> DefindCompare()
    {
        configCompare = new ConfigCompare<ConfigUnitRecord>("id");
        return configCompare;
    }

    public List<ConfigUnitRecord> GetUnitConfigCollection()
    {
        List<UnitData> decks = DataController.instance.GetDeck();

        List<ConfigUnitRecord> ls = new List<ConfigUnitRecord>();

        foreach (ConfigUnitRecord x in records)
        {
            bool isInDeck = false;
            foreach (UnitData d in decks)
            {
                if (d.id == x.ID)
                {
                    isInDeck = true;
                    break;
                }
            }
            if (isInDeck == false)
            {
                ls.Add(x);
            }
        }
        // return records.Where(x => decks.Where(d => d.id == x.ID).Count() == 0).ToList();
        return ls;
    }
}
