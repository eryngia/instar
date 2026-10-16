

using UnityEngine;

// Projectile belonging to enemy.
public class EnemyProjectile : Projectile {

    protected override Transform FindTarget() {
        // find player
        return FindObjectOfType<Player>().transform;
    }

}
