
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponSO", menuName = "PlayerWeaponSO", order = 0)]
public class PlayerWeaponSO : WeaponSO {
    public int ammoUsed = 1; // ammo used per shot
    public float kickback = 0.2f; // distance shifted when shooting
    public new PlayerProjectileSO projectileSO {
        get => (PlayerProjectileSO) base.projectileSO;
        set => base.projectileSO = value;
    }
}