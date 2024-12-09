using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunData
{
    public int damage;
    public float rof;
    public Transform target;
    public Vector3 dir;
    public float force;
    public UnitGunControl unitControl;

}
public abstract class UnitGunWeaponBehaviour : MonoBehaviour
{
    public IGunUnitHandle i_gunHandle;
    public UnitGunData data;
    public float timeReload;
    public float clip_size;
    public float numberBullet;
    private float timeFire;
    public bool isFire;
    public bool reloading;
    public Transform enemy_target;
    public UnitGunMuzzleFlash muzzleFlash;
    public abstract void SetupGun(UnitGunData unitGunData);
    public virtual void SetupGunData(UnitGunData unitGunData)
    {
        data = unitGunData;
    }
    public void Ready()
    {
        Debug.LogError(" ready gun ");
    }
    void Update()
    {
        timeFire += Time.deltaTime;
        if(isFire&&!reloading&&numberBullet>0)
        {
            if (timeFire>=data.rof)
            {
                timeFire = 0;
                numberBullet--;
                muzzleFlash.Fire();
                i_gunHandle.FireHandle();
                data.unitControl.databinding.Fire = true;
            }
        }
        if(numberBullet<=0&&!reloading)
        {
            data.unitControl.databinding.Reload = true;
            i_gunHandle.ReloadHandle();
        }
       
    }
}
public interface IGunUnitHandle
{
    void Setup(UnitGunWeaponBehaviour wp);
    void FireHandle();
    void ReloadHandle();
}