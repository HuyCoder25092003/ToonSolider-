using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInitData
{
    public ConfigEnemyRecord cf;

}
public class DamageData
{
    public int damage;
}
public class EnemyControl : FSM_System
{
    public Transform trans;
    public Transform trans_Detect;
    public Transform anchorHub;
    public float range_detect;
    public float range_attack;
    public NavMeshAgent agent;
    public ConfigEnemyRecord cf;
    public int hp;
    public float timeAttack;
    public DamageData damageData = new DamageData();
    public LayerMask maskUnit;
    public HPHub hp_hub;
    public virtual void Setup(EnemyInitData enemyInitData)
    {
        trans = transform;
        agent.updateRotation = false;
        agent.Warp(trans.position);
        cf = enemyInitData.cf;
        hp = cf.HP;
        damageData.damage = cf.Damage;
        Transform hub_trans = BYPoolManager.instance.GetPool("HPHub").Spawn();

        IngameView ingameView = (IngameView)ViewManager.instance.cur_view;
        hub_trans.transform.SetParent(ingameView.parent_hub, false);
        hp_hub=hub_trans.GetComponent<HPHub>();
        hp_hub.SetUp(anchorHub, ingameView.parent_hub,Color.red);
    }
    public virtual void OnDamage(DamageData enemyOnDamageData)
    {
        hp_hub.UpdateHP(hp, cf.HP);
    }
    public void OnDead()
    {
        hp_hub.OnDetachHub();
        GameController.instance.EnemyDead(this);
        Destroy(gameObject);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        timeAttack += Time.deltaTime;
    }
}
