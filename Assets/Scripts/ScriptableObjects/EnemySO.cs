
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "EnemySO", order = 0)]
public class EnemySO : ScriptableObject {
    public int health; // health
    public int collisionDamage = 1; // damage received on collision
    public int score; // score gained when defeated
    public float speed = 5f; // movement speed
    public float height; // hitbox height
    public float width; // hitbox width
    public WeaponSO weaponSO; // weapon
    public Sprite[] sprites; // animation frames
    public float animationSpeed = .4f; // seconds per animation frame
    public ExplosionSO deathExplosionSO; // explosion on death, null if none
    public float trackingStrength = 0f; // degrees turned towards player per second
}