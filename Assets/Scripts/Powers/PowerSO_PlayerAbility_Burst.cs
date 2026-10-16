
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerAbility_Burst", menuName = "PowerSO_PlayerAbility_Burst", order = 0)]
public class PowerSO_PlayerAbility_Burst : PowerSO {

    public int burstCountIncrease = 1;
    public float burstDelay = 0.2f;

    public override void PlayerAbilityPower(PlayerWeaponSO weaponSO) {
        weaponSO.burstCount += burstCountIncrease;
        weaponSO.burstDelay = burstDelay;
    }

}