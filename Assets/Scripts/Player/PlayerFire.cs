using System;
using System.Collections;
using UnityEngine;

// Player fire.
public class PlayerFire : MonoBehaviour {

    public event EventHandler<EventArgs> OnPlayerFireNoAmmo;
    
    private PlayerWeapon weapon;
    private bool firingInput = false;
    private bool firingCoroutine = false;

    private void Awake() {
        weapon = GetComponent<PlayerWeapon>();
    }

    private void Start() {
        PlayerWeaponSO weaponSO = weapon.weaponSO;

        // activate powers for this weapon
        foreach (PowerSO powerSO in GameManager.Instance.GetChosenPowers()) {
            powerSO.PlayerFirePower(weaponSO);
        }
        weapon.weaponSO = weaponSO;
    }

    // Firing input received, start firing.
    public void StartFire() {
        firingInput = true;
        if (!firingCoroutine) { 
            firingCoroutine = true;
            StartCoroutine(Firing());
        }
    }

    // Firing input stopped, stop firing.
    public void StopFire() {
        firingInput = false;
    }

    // Continue firing while input received and player has ammo.
    private IEnumerator Firing() {
        while (firingInput && GameManager.Instance.HasAmmo()) {
            Fire();
            while (!weapon.CanFire()) yield return null;
        }

        if (firingInput && !GameManager.Instance.HasAmmo())
            OnPlayerFireNoAmmo?.Invoke(this, EventArgs.Empty);

        firingCoroutine = false;
    }

    // Fire a single shot.
    private void Fire() {
        weapon.Fire();
        GameManager.Instance.UseAmmo(weapon.GetAmmoUsed());
    }

    
}
