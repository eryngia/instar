using UnityEngine;

public class WeaponSO : ScriptableObject {
    public float fireRate; // projectiles shot per second
    public float projectileRotation = 0f; // degrees between multiple projectiles
    public ProjectileSO projectileSO; // projectile
    public int projectileCount = 1; // number of projectiles per shot
    public int burstCount = 0; // additional shots immediately after shooting
    public float burstDelay = 0f; // delay before bursts
    public float inaccuracy = 0f; // maximum degrees randomly deviated from direction
}