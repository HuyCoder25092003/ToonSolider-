using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitInitData
{
    public ConfigUnitRecord configUnit;
    public ConfigUnitLevelRecord configUnit_lv;
    public UnitData unitData;
}
public class UnitControl : FSM_System
{
    [NonSerialized]
    public Transform trans;
    public NavMeshAgent u_agent;
    public Vector3 pos_ogrinal;
    public UnitInitData data;

    public float range;
    public int hp;
    private int max_hp;
    private HPHub hpHub;
    public Transform anchorHub;
    // Start is called before the first frame update
    public virtual void Setup(UnitInitData unitInitData)
    {
        this.data = unitInitData;
        this.data.configUnit_lv = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(this.data.configUnit.ID);
        trans = transform;
        u_agent.updateRotation = false;
        pos_ogrinal = trans.position;
        hp = data.configUnit_lv.GetHP(data.unitData.level);
        max_hp = hp;
        range = data.configUnit_lv.GetRange(data.unitData.level);

        Transform hub_trans = BYPoolManager.instance.GetPool("HPHub").Spawn();

        IngameView ingameView = (IngameView)ViewManager.instance.cur_view;
        hub_trans.transform.SetParent(ingameView.parent_hub, false);
        hpHub = hub_trans.GetComponent<HPHub>();
        hpHub.SetUp(anchorHub, ingameView.parent_hub, Color.green);
    }
    public virtual void OnDamage(DamageData damageData)
    {
        //  Destroy(gameObject);
        hpHub.UpdateHP(hp, max_hp);
    }
    public void OnDead()
    {
        hpHub.OnDetachHub();
        Destroy(gameObject);
    }
}
