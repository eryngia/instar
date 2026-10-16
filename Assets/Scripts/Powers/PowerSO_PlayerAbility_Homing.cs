
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerAbility_Homing", menuName = "PowerSO_PlayerAbility_Homing", order = 0)]
public class PowerSO_PlayerAbility_Homing : PowerSO {

    public float rangeMultiplier;
    public float speedMultiplier;
    public float homingStrengthIncrease;

    public override void PlayerAbilityPower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.range *= rangeMultiplier;
        weaponSO.projectileSO.speed *= speedMultiplier;
        weaponSO.projectileSO.homingStrength += homingStrengthIncrease;
    }

}