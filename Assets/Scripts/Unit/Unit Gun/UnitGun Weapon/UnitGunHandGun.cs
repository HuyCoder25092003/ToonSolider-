using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunHandGun : UnitGunWeaponBehaviour
{
    public override void SetupGun(UnitGunData unitGunData)
    {
        Debug.LogError("Hand gun"+unitGunData.rof);
        this.data = unitGunData;
        numberBullet = clip_size;
        i_gunHandle = new UnitGunHandGunHandle();
        i_gunHandle.Setup(this);
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
public class UnitGunHandGunHandle : IGunUnitHandle
{
    UnitGunHandGun wp;
    public void FireHandle()
    {
        // Debug.LogError(" hand gun fire");
        Transform bullet = BYPoolManager.instance.GetPool("Bullet").Spawn() ;
        bullet.position = wp.muzzleFlash.transform.position;
        Vector3 target = wp.data.target.position;
        target.y = bullet.position.y;
        Vector3 dir = target - bullet.position;
        dir.Normalize();
        wp.data.dir = dir;
        bullet.GetComponent<UnitGunBullet>().Setup(wp.data);
    }

    public void ReloadHandle()
    {
        wp.Reload();
    }

    public void Setup(UnitGunWeaponBehaviour wp)
    {
        this.wp = (UnitGunHandGun)wp;
    }
}