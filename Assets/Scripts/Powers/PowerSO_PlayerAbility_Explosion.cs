
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO_PlayerAbility_Explosion", menuName = "PowerSO_PlayerAbility_Explosion", order = 0)]
public class PowerSO_PlayerAbility_Explosion : PowerSO {

    public ExplosionSO explosionSO;

    public override void PlayerAbilityPower(PlayerWeaponSO weaponSO) {
        weaponSO.projectileSO.explosionSO = explosionSO;
    }

}