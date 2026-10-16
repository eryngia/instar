
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerAbility_ProjectileCount", menuName = "PowerSO_PlayerAbility_ProjectileCount", order = 0)]
public class PowerSO_PlayerAbility_ProjectileCount : PowerSO {

    public int projectileCountIncrease;
    public float sizeMultiplier;

    public override void PlayerAbilityPower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileCount += projectileCountIncrease;
        weaponSO.projectileSO.height *= sizeMultiplier;
        weaponSO.projectileSO.width *= sizeMultiplier;
    }

}