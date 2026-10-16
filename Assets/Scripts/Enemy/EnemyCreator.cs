using UnityEngine;

// Creates an enemy game object following the details in a scriptable object.
public class EnemyCreator : MonoBehaviour {

    [SerializeField] private Material enemyFlashMaterial;
    [SerializeField] private ParticleSystem enemyDiedParticles;

    public GameObject Create(EnemySO enemySO, PathSO pathSO) {
        GameObject enemy = new GameObject("Enemy");
        enemy.SetActive(false);
        enemy.layer = GameManager.Constants.enemyLayer;

        Enemy enemyScript = enemy.AddComponent<Enemy>();
        enemy.AddComponent<EnemyAudio>();

        CreateVisuals(enemy, enemySO);
        GameObject firePoint = CreateFirePoint(enemy, enemySO);
        CreateHitBox(enemy, enemySO);

        enemy.transform.position = pathSO.waypoints[0].position;
        enemy.transform.rotation = Quaternion.Euler(0, 0, 180);
        enemyScript.Initialize(firePoint.transform, enemySO, pathSO);
        enemy.SetActive(true);
        
        return enemy;
    }

    private GameObject CreateVisuals(GameObject enemy, EnemySO enemySO) {
        GameObject enemyVisuals = new GameObject("Visuals");
        enemyVisuals.layer = GameManager.Constants.enemyLayer;
        enemyVisuals.transform.SetParent(enemy.transform);
        enemyVisuals.transform.rotation = Quaternion.Euler(0, 0, 180);

        SpriteRenderer spriteRenderer = enemyVisuals.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemySO.sprites[0];

        EnemyVisuals enemyVisualsScript = enemyVisuals.AddComponent<EnemyVisuals>();
        enemyVisualsScript.Initialize(enemyFlashMaterial, enemyDiedParticles);

        SpriteAnimator animator = enemyVisuals.AddComponent<SpriteAnimator>();
        animator.Initialize(new[] { enemySO.sprites[0], enemySO.sprites[1] }, enemySO.animationSpeed);
        
        return enemyVisuals;
    }


    private GameObject CreateFirePoint(GameObject enemy, EnemySO enemySO) {
        GameObject firePoint = new GameObject("FirePoint");
        firePoint.layer = GameManager.Constants.enemyLayer;
        firePoint.transform.SetParent(enemy.transform);
        firePoint.transform.localPosition = new Vector3(0, enemySO.height / GameManager.Constants.pixelsPerUnit / 2, 0);
       
        return firePoint;
    }

    private GameObject CreateHitBox(GameObject enemy, EnemySO enemySO) {
        GameObject hitBox = new GameObject("HitBox");
        hitBox.layer = GameManager.Constants.enemyLayer;
        hitBox.transform.SetParent(enemy.transform);

        hitBox.AddComponent<HitBox>();

        CapsuleCollider2D hitCollider = hitBox.AddComponent<CapsuleCollider2D>();
        hitCollider.size = new Vector2(enemySO.width / GameManager.Constants.pixelsPerUnit, enemySO.height / GameManager.Constants.pixelsPerUnit);
        hitCollider.direction = (enemySO.width >= enemySO.height) ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;
        hitCollider.isTrigger = true;

        Rigidbody2D hitRb = hitBox.AddComponent<Rigidbody2D>();
        hitRb.bodyType = RigidbodyType2D.Kinematic;

        return hitBox;
    }

}