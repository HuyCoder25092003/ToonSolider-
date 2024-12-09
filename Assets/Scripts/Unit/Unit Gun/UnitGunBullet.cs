using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunBullet : MonoBehaviour
{
    public string name_pool = "Bullet";
    public string name_pool_impact = "Impact";
    private Transform trans;
    public float life_time=1;
    public LayerMask mask;
    private UnitGunData data;
    private void Awake()
    {
        trans = transform;
    }
    public void  Setup(UnitGunData data)
    {
        this.data = data;
        trans.forward = data.dir;
    }
    void Update()
    {
        trans.Translate(Vector3.forward * Time.deltaTime*40);
        RaycastHit hitinfo;
        if(Physics.Raycast(trans.position,trans.forward,out hitinfo,1,mask))
        {
            BYPoolManager.instance.GetPool(name_pool).Despawn(trans);
            Transform impact = BYPoolManager.instance.GetPool(name_pool_impact).Spawn();
            impact.position = hitinfo.point;
            impact.forward = hitinfo.normal;
            DamageData damageData = new DamageData();
            damageData.damage = data.damage;
            hitinfo.collider.GetComponent<EnemyControl>().OnDamage(damageData);

        }
    }
    public void OnSpawn()
    {
        StopCoroutine("Delay");
        StartCoroutine("Delay");
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(life_time);
        BYPoolManager.instance.GetPool(name_pool).Despawn(trans);
    }
}
