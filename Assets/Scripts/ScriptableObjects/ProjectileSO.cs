
using UnityEngine;

public class ProjectileSO : ScriptableObject {

    public int damage; // damage
    public float speed; // movement speed
    public float range = 100f; // maximum range before deleted
    public float height; // hitbox height
    public float width; // hitbox width
    public float homingStrength = 0f; // degrees turned per second towards target
    public ExplosionSO explosionSO; // explosion when hitting target or reaching range, null if none
    public bool piercing = false; // not destroyed when hits target
    public bool ray = false; // beam mode
    public float rayWarmUp = 0.2f; // warmup seconds before ray activated
    public float rayDuration = 1f; // duration of ray
}