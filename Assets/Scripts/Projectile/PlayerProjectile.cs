using UnityEngine;

// Projectile belonging to player.
public class PlayerProjectile : Projectile {

    protected override Transform FindTarget() {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0) return null;

        Enemy nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        // find nearest enemy to target
        foreach (Enemy enemy in enemies) {
            float distance = Vector2.Distance(currentPosition, enemy.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy.transform;
    }

}
