using System;
using System.Collections;
using UnityEngine;

// Core player representation.
public class Player : Entity {

    public event EventHandler OnPlayerDied;    
    public event EventHandler<DurationEventArgs> OnPlayerDamaged; 
    public class DurationEventArgs : EventArgs {
        public float duration;
    }

    [SerializeField] private float invulnerableDuration = 1f;
    private bool invulnerable = false;

    private void Start() {
        gameObject.layer = GetLayer();
    }

    public override void ReceiveHit(IHittable source, int damage) {
        // no damage if invulnerable
        if (invulnerable || damage <= 0) return;

        // remove health in game manager
        GameManager.Instance.LoseHealth(damage);
        OnPlayerDamaged?.Invoke(this, new DurationEventArgs { duration = invulnerableDuration });

        // if 0 health, proceed to die
        if (GameManager.Instance.NoHealth()) Die();

        // invulnerability period after getting hit
        BeInvulnerable(invulnerableDuration);
    }

    public override int Hit(IHittable damage) {
        // destroy any enemy this hits
        return int.MaxValue;
    }

    // Load the game over scene and destroy this.
    private void Die() {
        OnPlayerDied?.Invoke(this, EventArgs.Empty);
        SceneManager.Instance.LoadEndScene();
        Destroy(gameObject);
    }

    public override int GetLayer() {
        return GameManager.Constants.playerLayer;
    }

    public override int GetProjectileLayer() {
        return GameManager.Constants.playerProjectileLayer;
    }

    // Activate invulnerability for a time.
    public void BeInvulnerable(float time) {
        if (!invulnerable) {
            StartCoroutine(BeingInvulnerable(time));
        }
    }

    // Set invulnerability to true, then return to false after time.
    private IEnumerator BeingInvulnerable(float time) {
        invulnerable = true;
        yield return new WaitForSeconds(time);
        invulnerable = false;
    }

}
