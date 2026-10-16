using System;
using System.Collections;
using UnityEngine;

// Core enemy representation.
public class Enemy : Entity {

    public event EventHandler OnEnemyDamaged; 
    public event EventHandler OnEnemyDied;

    [SerializeField] private EnemySO enemySO;
    public PathSO pathSO;
    private float health;
    private Weapon weapon;
    private int currentWaypointIndex = 1;
    private bool startFiring = false;
    private Transform target;
    private bool deathAmmo, deathHealth, deathExplosion;
    private int deathAmmoGained, deathHealthKillsRequired;
    private ExplosionSO deathExplosionSO;

    private static int kills = 0;

    private void Start() {
        weapon = gameObject.AddComponent<EnemyWeapon>();
        weapon.Initialize(this, enemySO.weaponSO);

        health = enemySO.health;
        gameObject.layer = GetLayer();

        target = FindObjectOfType<Player>()?.transform;

        StartCoroutine(RandomInitialOffset());

        PowerSO_KillAmmo killAmmoPower = GameManager.Instance.GetChosenPower<PowerSO_KillAmmo>();
        if (killAmmoPower != null) {
            deathAmmo = true;
            deathAmmoGained = killAmmoPower.ammoGained;
        }
        PowerSO_KillHealth killHealthPower = GameManager.Instance.GetChosenPower<PowerSO_KillHealth>();
        if (killHealthPower != null) {
            deathHealth = true;
            deathHealthKillsRequired = killHealthPower.killsRequired;
        }
        PowerSO_KillExplosion killExplosionPower = GameManager.Instance.GetChosenPower<PowerSO_KillExplosion>();
        if (killExplosionPower != null) {
            deathExplosion = true;
            deathExplosionSO = killExplosionPower.explosionSO;
        }
    }

    public void Initialize(Transform firePoint, EnemySO enemySO, PathSO pathSO) {
        Initialize(firePoint);
        this.enemySO = enemySO;
        this.pathSO = pathSO;
    }

    // Offset the initial time to fire slightly so that not all enemies in the wave fire in a strict pattern.
    private IEnumerator RandomInitialOffset() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
        startFiring = true;
    }

    private void Update() {
        if(startFiring && IsOnScreen()) weapon.Fire();
        Move();
        Track();
    }

    // If the enemy is currently visible on screen.
    private bool IsOnScreen() {
        return transform.position.x < GameManager.Constants.screenWidth / 2
            && transform.position.x > - GameManager.Constants.screenWidth / 2
            && transform.position.y < GameManager.Constants.screenHeight / 2
            && transform.position.y > - GameManager.Constants.screenHeight / 2;
    }

    public override void ReceiveHit(IHittable source, int damage) {
        TakeDamage(damage);
    }

    public override int Hit(IHittable target) {
        return enemySO.collisionDamage;
    }

    // Take damage and possible be destroyed.
    private void TakeDamage(float damage) {
        if (damage <= 0) return;
        health -= damage;
        OnEnemyDamaged?.Invoke(this, EventArgs.Empty);
        if (health <= 0) Die();
    }

    // Destroy this and trigger all death effects.
    private void Die() {
        GameManager.Instance.AddScore(enemySO.score);
        OnEnemyDied?.Invoke(this, EventArgs.Empty);
        kills++;
        DeathExplode();
        DeathPowers();
        Destroy(gameObject);
    }

    // Explode on death, harming the player.
    private void DeathExplode() {
        if (enemySO.deathExplosionSO != null)
            ExplosionCreator.Create(this, enemySO.deathExplosionSO, transform.position);
    }

    // Handle various powers that trigger on killing an enemy.
    private void DeathPowers() {
        if (deathAmmo && GameManager.Instance.GetAmmo() > 0) 
            GameManager.Instance.GainAmmo(deathAmmoGained);
        if (deathHealth && kills % deathHealthKillsRequired == 0)
            GameManager.Instance.GainHealth(1);
        if (deathExplosion)
            ExplosionCreator.Create(FindObjectOfType<Player>(), deathExplosionSO, transform.position);
    }

    public override int GetLayer() {
        return GameManager.Constants.enemyLayer;
    }
    public override int GetProjectileLayer() {
        return GameManager.Constants.enemyProjectileLayer;
    }

    // Follow the path waypoints at its speed.
    private void Move() {
        Transform currentWaypoint = pathSO.waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, enemySO.speed * Time.deltaTime);
        
        if (Mathf.Approximately(Vector3.Distance(transform.position, currentWaypoint.position), 0)) {
            currentWaypointIndex++;
            if (currentWaypointIndex == pathSO.waypoints.Length) {
                Destroy(gameObject);
            }
        }
    }

    // Rotate gradually towards the player.
    private void Track() {
        if (enemySO.trackingStrength <= 0 || gameObject == null || target == null) return;

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, enemySO.trackingStrength * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
