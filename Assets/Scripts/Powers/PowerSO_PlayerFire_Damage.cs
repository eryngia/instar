
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerFire_Damage", menuName = "PowerSO_PlayerFire_Damage", order = 0)]
public class PowerSO_PlayerFire_Damage : PowerSO {

    public float damageMultiplier;
    public float rangeMultiplier;

    public override void PlayerFirePower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.damage = (int) (weaponSO.projectileSO.damage * damageMultiplier);
        weaponSO.projectileSO.range *= rangeMultiplier;
    }

}