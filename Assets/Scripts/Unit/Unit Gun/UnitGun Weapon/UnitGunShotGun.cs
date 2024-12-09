using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunShotGun : UnitGunWeaponBehaviour
{
    public string name_pool_impact = "Impact";
    public override void SetupGun(UnitGunData unitGunData)
    {
        Debug.LogError("Shot gun");
        i_gunHandle = new UnitGunShotGunHandle();
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
    // Start is called before the first frame update

}
public class UnitGunShotGunHandle : IGunUnitHandle
{
    UnitGunShotGun wp;
    public void FireHandle()
    {
        Debug.LogError(" shotgun fire");
        for(int i=0;i<5;i++)
        {
            float x = UnityEngine.Random.Range(-5f, 5f);
            float y = UnityEngine.Random.Range(-5f, 5f);
            float dis = Vector3.Distance(wp.muzzleFlash.transform.position, wp.data.target.position);
            if (dis > 2)
            {
                Transform bullet = BYPoolManager.instance.GetPool("Bullet").Spawn();
                bullet.position = wp.muzzleFlash.transform.position;
                Vector3 target = wp.data.target.position;
                target.y = bullet.position.y;
                Vector3 dir = target - bullet.position;
                dir.Normalize();
                dir = Quaternion.Euler(x, y, 0) * dir;
                wp.data.dir = dir;
                bullet.GetComponent<UnitGunBullet>().Setup(wp.data);
            }
            else
            {
                Vector3 target = wp.data.target.position;
                Vector3 pos = wp.muzzleFlash.transform.position;
                target.y = pos.y;
                Vector3 dir = target - pos;
                RaycastHit hitinfo;
                dir = Quaternion.Euler(x, y, 0) * dir;
                if (Physics.Raycast(wp.muzzleFlash.transform.position, dir.normalized, out hitinfo, 1, 1 << 6))
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
    }

    public void ReloadHandle()
    {
        wp.Reload();
    }

    public void Setup(UnitGunWeaponBehaviour wp)
    {
        this.wp = (UnitGunShotGun)wp;
    }
}