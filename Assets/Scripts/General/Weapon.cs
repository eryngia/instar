using System;
using System.Collections;
using UnityEngine;

// An abstract weapon that can fire projectiles in various patterns based on a scripable object.
public abstract class Weapon : MonoBehaviour {

    public event EventHandler OnWeaponFired;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Entity owner;
    private bool canFire = true;

    public void Initialize(Entity owner, WeaponSO weaponSO) {
        this.owner = owner;
        this.weaponSO = weaponSO;
    }

    private void Awake() {
        if (weaponSO != null) {
            weaponSO = Instantiate(weaponSO);
            weaponSO.projectileSO = Instantiate(weaponSO.projectileSO);
        } 
    }

    // Attack with the weapon.
    public void Fire() {
        if (canFire) {
            FireProjectile();
            if (weaponSO.burstCount > 0) StartCoroutine(BurstFire());
            StartCoroutine(Cooldown());
        }
    }

    // Fire a single set of projectiles.
    protected virtual void FireProjectile() {
        // get rotations of each projectile
        Quaternion[] projectileRotations = GetProjectileRotations(owner.GetFirePoint().rotation, weaponSO.projectileCount, weaponSO.projectileRotation);

        // create each projectile
        for (int i = 0; i < weaponSO.projectileCount; i++) {
            Quaternion rotation = projectileRotations[i] * Quaternion.Euler(0, 0, UnseededRandomUtils.Range(-weaponSO.inaccuracy, weaponSO.inaccuracy)); // modify by inaccuracy
            ProjectileCreator.Create(owner, weaponSO.projectileSO, owner.GetFirePoint().position, rotation);
        }

        OnWeaponFired?.Invoke(this, EventArgs.Empty);
    }

    // Run the cooldown before the weapon can attack again.
    private IEnumerator Cooldown() {
        canFire = false;
        if (weaponSO.projectileSO.ray) yield return new WaitForSeconds(weaponSO.projectileSO.rayWarmUp + weaponSO.projectileSO.rayDuration);
        yield return new WaitForSeconds(weaponSO.burstCount * weaponSO.burstDelay);
        yield return new WaitForSeconds(1f / weaponSO.fireRate);
        canFire = true;
    }

    // Fire multiple sets of projectiles in a burst.
    private IEnumerator BurstFire() {
        for (int i = 0; i < weaponSO.burstCount; i++) {
            yield return new WaitForSeconds(weaponSO.burstDelay);
            FireProjectile();
        }
    }

    // If the weapon can currently fire.
    public bool CanFire() {
        return canFire;
    }

    // Calculate the projectile rotations for an even spread of multiple projectiles.
    public static Quaternion[] GetProjectileRotations(Quaternion firingRotation, int projectileCount, float projectileRotation) {
        if (projectileCount == 1) return new Quaternion[] { firingRotation };
       
        Quaternion[] projectileRotations = new Quaternion[projectileCount];

        Vector3 rotationAxis = firingRotation * Vector3.forward;
        float totalSpread = (projectileCount - 1) * projectileRotation;
        Quaternion startRotation = firingRotation * Quaternion.AngleAxis(-totalSpread / 2, rotationAxis);

        for (int i = 0; i < projectileCount; i++) {
            projectileRotations[i] = startRotation * Quaternion.AngleAxis(i * projectileRotation, rotationAxis);
        }

        return projectileRotations;
    }

    protected abstract Color GetProjectileInnerColor();
    protected abstract Color GetProjectileOuterColor();
    protected abstract Sprite GetProjectileSprite();

    public WeaponSO GetWeaponSO() {
        return weaponSO;
    }

    public void SetWeaponSO(WeaponSO weaponSO) {
        this.weaponSO = weaponSO;
    }
}

