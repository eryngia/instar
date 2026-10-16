using UnityEngine;

// A weapon belonging to an enemy.
public class EnemyWeapon : Weapon {

    public new EnemyWeaponSO weaponSO {
        get => (EnemyWeaponSO) GetWeaponSO();
        set => SetWeaponSO(value);
    }
    
    protected override Color GetProjectileInnerColor() {
        return Color.white;
    }

    protected override Color GetProjectileOuterColor() {
        return Color.red;
    }

    protected override Sprite GetProjectileSprite() {
        return GameManager.Constants.enemyProjectileSprite;
    }

}