
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerFire_ProjectileCount", menuName = "PowerSO_PlayerFire_ProjectileCount", order = 0)]
public class PowerSO_PlayerFire_ProjectileCount : PowerSO {

    public int projectileCountIncrease;
    public float projectileRotation;
    public int ammoUsedIncrease;

    public override void PlayerFirePower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileCount += projectileCountIncrease;
        weaponSO.projectileRotation = projectileRotation;
        weaponSO.ammoUsed += ammoUsedIncrease;
    }

}