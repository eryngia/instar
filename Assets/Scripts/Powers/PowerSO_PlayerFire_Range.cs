
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerFire_Range", menuName = "PowerSO_PlayerFire_Range", order = 0)]
public class PowerSO_PlayerFire_Range : PowerSO {

    public float rangeMultiplier;
    public float speedMultiplier;
    public float fireRateMultiplier;

    public override void PlayerFirePower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.range *= rangeMultiplier;
        weaponSO.projectileSO.speed *= speedMultiplier;
        weaponSO.fireRate *= fireRateMultiplier;
    }

}