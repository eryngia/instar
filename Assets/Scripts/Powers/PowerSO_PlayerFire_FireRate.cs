
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerFire_FireRate", menuName = "PowerSO_PlayerFire_FireRate", order = 0)]
public class PowerSO_PlayerFire_FireRate : PowerSO {

    public float fireRateMultiplier;
    public float damageMultiplier;

    public override void PlayerFirePower(PlayerWeaponSO weaponSO) {
        weaponSO.fireRate *= fireRateMultiplier;
        weaponSO.projectileSO.damage = (int) (weaponSO.projectileSO.damage * damageMultiplier);
    }

}