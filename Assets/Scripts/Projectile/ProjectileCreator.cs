using UnityEngine;

// Create projectile game object based on scriptable object.
public static class ProjectileCreator {

    public static GameObject Create(Entity owner, ProjectileSO projectileSO, Vector3 position, Quaternion rotation) {
        GameObject projectile = new GameObject("Projectile");
        projectile.SetActive(false);
        projectile.layer = owner.GetProjectileLayer();

        Projectile projectileScript = null;
        Sprite sprite = null;
        if (projectileSO is EnemyProjectileSO) {
            projectileScript = projectile.AddComponent<EnemyProjectile>();
            if (projectileSO.ray)
               sprite = GameManager.Constants.enemyRaySprite;
            else
                sprite = GameManager.Constants.enemyProjectileSprite;
        } else if (projectileSO is PlayerProjectileSO) {
            projectileScript = projectile.AddComponent<PlayerProjectile>();
            if (projectileSO.ray)
               sprite = GameManager.Constants.playerRaySprite;
            else 
                sprite = GameManager.Constants.playerProjectileSprite;
        }
        projectileScript.Initialize(owner, projectileSO);

        CreateVisuals(projectile, sprite, owner, projectileSO);
        CreateHitBox(projectile, owner, projectileSO);

        projectile.transform.position = position;
        projectile.transform.rotation = rotation;
        projectile.SetActive(true);
        
        return projectile;
    }

    private static GameObject CreateVisuals(GameObject projectile, Sprite sprite, Entity owner, ProjectileSO projectileSO) {
        GameObject visuals = new GameObject("Visuals");
        visuals.layer = owner.GetProjectileLayer();
        visuals.transform.SetParent(projectile.transform);

        SpriteRenderer spriteRenderer = visuals.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        float width = projectileSO.width / GameManager.Constants.pixelsPerUnit / (sprite.texture.width / sprite.pixelsPerUnit);  
        float height = projectileSO.height / GameManager.Constants.pixelsPerUnit / (sprite.texture.height / sprite.pixelsPerUnit);

        visuals.transform.localScale = new Vector3(width, height, 1f);

        return visuals;
    }

    private static GameObject CreateHitBox(GameObject projectile, Entity owner, ProjectileSO projectileSO) {
        GameObject hitBox = new GameObject("HitBox");
        hitBox.layer = owner.GetProjectileLayer();
        hitBox.transform.SetParent(projectile.transform);

        CapsuleCollider2D collider = hitBox.AddComponent<CapsuleCollider2D>();
        collider.size = new Vector2(projectileSO.width / GameManager.Constants.pixelsPerUnit, projectileSO.height / GameManager.Constants.pixelsPerUnit);
        collider.isTrigger = true;

        hitBox.AddComponent<HitBox>();

        return hitBox;
    }
}