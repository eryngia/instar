using UnityEngine;

// Creates an explosion game object based on scriptable object.
public static class ExplosionCreator {

    public static GameObject Create(Entity owner, ExplosionSO explosionSO, Vector3 position) {
        GameObject explosion = new GameObject("Explosion");
        explosion.SetActive(false);
        explosion.layer = owner.GetProjectileLayer();

        Explosion explosionScript = explosion.AddComponent<Explosion>(); 
        explosionScript.Initialize(owner, explosionSO);

        CreateVisuals(explosion, owner);
        CreateHitBox(explosion, owner);

        explosion.transform.position = position;
        explosion.SetActive(true);
        
        return explosion;
    }

    private static GameObject CreateVisuals(GameObject projectile, Entity owner) {
        GameObject visuals = new GameObject("Visuals");
        visuals.layer = owner.GetProjectileLayer();
        visuals.transform.SetParent(projectile.transform);

        SpriteRenderer spriteRenderer = visuals.AddComponent<SpriteRenderer>();

        return visuals;
    }

    private static GameObject CreateHitBox(GameObject projectile, Entity owner) {
        GameObject hitBox = new GameObject("HitBox");
        hitBox.layer = owner.GetProjectileLayer();
        hitBox.transform.SetParent(projectile.transform);

        CircleCollider2D collider = hitBox.AddComponent<CircleCollider2D>();
        collider.radius = 0;
        collider.isTrigger = true;

        hitBox.AddComponent<HitBox>();

        return hitBox;
    }
}