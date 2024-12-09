using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UnitGunAssault : UnitGunWeaponBehaviour
{
    public string name_pool_impact = "Impact";
    public override void SetupGun(UnitGunData unitGunData)
    {
        Debug.LogError("Asuslt gun");
        i_gunHandle = new UnitGunAssaultHandle();
        i_gunHandle.Setup(this);

        this.data = unitGunData;
        numberBullet = clip_size;
    }
    public void Reload()
    {
        StopCoroutine("ReloadProgress");
        StartCoroutine("ReloadProgress");
    }
    IEnumerator ReloadProgress()
    {
        reloading = true;
        yield return new WaitForSeconds(timeReload);
        reloading = false;
        numberBullet = clip_size;
    }

}

public class UnitGunAssaultHandle : IGunUnitHandle
{
    UnitGunAssault wp;

    public void FireHandle()
    {
        float dis = Vector3.Distance(wp.muzzleFlash.transform.position, wp.data.target.position);
        if(dis>2)
        {
            Transform bullet = BYPoolManager.instance.GetPool("Bullet").Spawn();
            bullet.position = wp.muzzleFlash.transform.position;
            Vector3 target = wp.data.target.position;
            target.y = bullet.position.y;
            Vector3 dir = target - bullet.position;
            dir.Normalize();
            wp.data.dir = dir;
            bullet.GetComponent<UnitGunBullet>().Setup(wp.data);
        }
        else
        {
            Vector3 target = wp.data.target.position;
           Vector3 pos= wp.muzzleFlash.transform.position;
            target.y = pos.y;
            Vector3 dir = target - pos;
            RaycastHit hitinfo;
            if (Physics.Raycast(wp.muzzleFlash.transform.position, dir.normalized, out hitinfo, 1, 1<<6))
            {
                Transform impact = BYPoolManager.instance.GetPool(wp.name_pool_impact).Spawn();
                impact.position = hitinfo.point;
                impact.forward = hitinfo.normal;
                DamageData damageData = new DamageData();
                damageData.damage = wp.data.damage;
                hitinfo.collider.GetComponent<EnemyControl>().OnDamage(damageData);

            }
        }
      
    }

    public void ReloadHandle()
    {
        wp.Reload();
    }

  
    public void Setup(UnitGunWeaponBehaviour wp)
    {
       this.wp=(UnitGunAssault)wp;
    }
}