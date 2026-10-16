

using System;
using UnityEngine;

// Player weapon.
public class PlayerWeapon : Weapon {
    
    public new PlayerWeaponSO weaponSO {
        get => (PlayerWeaponSO) GetWeaponSO();
        set => SetWeaponSO(value);
    }
    private Transform playerTransform;
    private int extraLowHealthDamage;
    private int lowHealthMaxBonusDamage;
    private float lowHealthCurveExponent;

    private void Start() {
        playerTransform = transform.parent.transform;

        PowerSO_LowHealth_Damage lowHealthPower = GameManager.Instance.GetChosenPower<PowerSO_LowHealth_Damage>();
        if (lowHealthPower != null) {
            lowHealthMaxBonusDamage = lowHealthPower.maxBonusDamage;
            lowHealthCurveExponent = lowHealthPower.curveExponent;
            GetComponentInParent<Player>().OnPlayerDamaged += LowHealthPower;
        }
    }

    protected override void FireProjectile() {
        base.FireProjectile();
        // apply additional kickback
        playerTransform.position += new Vector3(0, - weaponSO.kickback, 0);
    }

    // Return ammo used per shot by current weapon.
    public int GetAmmoUsed() {
        return weaponSO.ammoUsed;
    }

    protected override Color GetProjectileInnerColor() {
        return Color.white;
    }

    protected override Color GetProjectileOuterColor() {
        return Color.white;
    }

    protected override Sprite GetProjectileSprite() {
        return GameManager.Constants.playerProjectileSprite;
    }

    // Apply power benefits for weapon at low health.
    private void LowHealthPower(object sender, EventArgs e) {
        weaponSO.projectileSO.damage -= extraLowHealthDamage;

        float normalizedHealth = (float) GameManager.Instance.GetHealth() / GameManager.Instance.GetMaxHealth();
        float curve = Mathf.Pow(1 - normalizedHealth, lowHealthCurveExponent);
        extraLowHealthDamage = (int)(curve * lowHealthMaxBonusDamage);        
        
        weaponSO.projectileSO.damage += extraLowHealthDamage;
    }

}