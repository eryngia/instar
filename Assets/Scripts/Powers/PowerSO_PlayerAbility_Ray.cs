
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerAbility_Ray", menuName = "PowerSO_PlayerAbility_Ray", order = 0)]
public class PowerSO_PlayerAbility_Ray : PowerSO {

    public float newHeight;
    public float widthMultiplier;
    public float rayWarmUp;
    public float rayDuration;
    public float damageMultiplier;

    public override void PlayerAbilityPower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.ray = true;
        weaponSO.projectileSO.rayWarmUp = rayWarmUp;
        weaponSO.projectileSO.rayDuration = rayDuration;
        weaponSO.projectileSO.height = newHeight;
        weaponSO.projectileSO.width *= widthMultiplier;
        weaponSO.projectileSO.piercing = true;
        weaponSO.projectileSO.damage = (int) (weaponSO.projectileSO.damage * damageMultiplier);
        weaponSO.projectileCount -= 4;
    }

}