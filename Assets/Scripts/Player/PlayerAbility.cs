using System;
using UnityEngine;

// Player special attack.
public class PlayerAbility : MonoBehaviour {
    
    public event EventHandler<EventArgs> OnPlayerSpecialNoAmmo;

    private PlayerWeapon weapon;
    private bool halfAmmoPower;

    private void Awake() {
        weapon = GetComponent<PlayerWeapon>();
    }

    private void Start() {
        PlayerWeaponSO weaponSO = weapon.weaponSO;
        
        // activate powers for this weapon
        foreach (PowerSO powerSO in GameManager.Instance.GetChosenPowers()) {
            powerSO.PlayerAbilityPower(weaponSO);
        }
        weapon.weaponSO = weaponSO;

        if (GameManager.Instance.GetChosenPower<PowerSO_PlayerAbility_HalfAmmo>() != null)
            halfAmmoPower = true;
    }

    // Use special attack if player has ammo left.
    public void Ability() {
        if (!weapon.CanFire()) return;
        if (GameManager.Instance.HasAmmo()) {
            weapon.Fire();
            int ammo = GameManager.Instance.GetMaxAmmo();
            GameManager.Instance.UseAmmo(halfAmmoPower ? ammo / 2 : ammo);
        } else {
            OnPlayerSpecialNoAmmo?.Invoke(this, EventArgs.Empty);
        }
    }
    
}
