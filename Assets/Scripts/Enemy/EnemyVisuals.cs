using System;
using UnityEngine;

// Handles visual effects for the enemy.
public class EnemyVisuals : Visuals {
    [SerializeField] private Color enemyDamagedFlashColor = Color.white;
    [SerializeField] private float enemyDamagedFlashDuration = 0.1f;
    [SerializeField] private float enemyDamagedShakeMagnitude = 0.1f;
    [SerializeField] private float enemyDamagedShakeDuration = 0.03f;
    [SerializeField] private ParticleSystem enemyDiedParticles;

    private Enemy enemy;
    
    private void Start() {
        enemy = GetComponentInParent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        enemy.OnEnemyDamaged += OnEnemyDamagedHandler;
        enemy.OnEnemyDied += OnEnemyDiedHandler;
    }

    public void Initialize(Material enemyFlashMaterial, ParticleSystem enemyDiedParticles, float enemyDamagedFlashDuration = 0.1f, float enemyDamagedShakeMagnitude = 0.1f, float enemyDamagedShakeDuration = 0.03f) {
        flashMaterial = enemyFlashMaterial;
        this.enemyDiedParticles = enemyDiedParticles;
        this.enemyDamagedFlashDuration = enemyDamagedFlashDuration;
        this.enemyDamagedShakeMagnitude = enemyDamagedShakeMagnitude;
        this.enemyDamagedShakeDuration = enemyDamagedShakeDuration;
    }

    // Flash and shake screen slightly when damaged.
    private void OnEnemyDamagedHandler(object sender, EventArgs e) {
        StartCoroutine(Flash(enemyDamagedFlashColor, enemyDamagedFlashDuration));
        StartCoroutine(ShakeScreen(enemyDamagedShakeMagnitude, enemyDamagedShakeDuration));
    }

    // Explode when destroyed.
    private void OnEnemyDiedHandler(object sender, EventArgs e) {
        PlayParticles(enemyDiedParticles);
    }
}
