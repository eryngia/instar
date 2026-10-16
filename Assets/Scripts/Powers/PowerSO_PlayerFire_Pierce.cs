
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerFire_Pierce", menuName = "PowerSO_PlayerFire_Pierce", order = 0)]
public class PowerSO_PlayerFire_Pierce : PowerSO {

    public float speedMultiplier;

    public override void PlayerFirePower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.piercing = true;
        weaponSO.projectileSO.speed *= speedMultiplier;
    }

}