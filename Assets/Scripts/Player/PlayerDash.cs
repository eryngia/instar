using System;
using System.Collections;
using UnityEngine;

// Player dash.
public class PlayerDash : MonoBehaviour {

    public event EventHandler<Player.DurationEventArgs> OnPlayerDash;

    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.5f;

    private PlayerController playerController;
    private Player player;

    private bool dashOnCooldown = false;

    private void Awake() {
        playerController = GetComponent<PlayerController>();    
        player = GetComponent<Player>();
    }

    // If not on cooldown, begin dash.
    public void Dash(Vector2 dashDirection) {
        if (dashDirection == Vector2.zero) dashDirection = new Vector2(0, 1);
        if (CanDash()) StartCoroutine(Dashing(dashDirection));
    }

    // Perform dash, moving straight in current direction for the duration.
    private IEnumerator Dashing(Vector2 dashDirection) {
        // disable all input and activate cooldown
        playerController.DisableInput();
        dashOnCooldown = true;
        OnPlayerDash?.Invoke(this, new Player.DurationEventArgs { duration = dashDuration });

        // activate invulnerability during dash
        player.BeInvulnerable(dashDuration);

        // move in direction in increments
        float dashTime = 0f;
        while (dashTime < dashDuration) {
            Vector3 delta = dashDirection * dashSpeed * Time.deltaTime;
            transform.position += delta;
            dashTime += Time.deltaTime;
            yield return null;
        }

        // enable input and reload ammo
        playerController.EnableInput();
        GameManager.Instance.ReloadAmmo();

        // wait for cooldown before enabling dashing again
        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    private bool CanDash() {
        return !dashOnCooldown;
    }
}
