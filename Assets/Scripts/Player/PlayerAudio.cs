using System;
using UnityEngine;

// Sound effects for player.
public class PlayerAudio : MonoBehaviour {

    [SerializeField] private AudioClip[] damagedAudio;
    [SerializeField] private AudioClip[] diedAudio;
    [SerializeField] private AudioClip[] dashAudio;
    [SerializeField] private AudioClip[] fireAudio;
    [SerializeField] private AudioClip[] specialAudio;
    [SerializeField] private AudioClip[] noAmmoAudio;
    [SerializeField] private AudioClip[] scoreBoostActivatedAudio;
    [SerializeField] private AudioClip[] scoreBoostDeactivatedAudio;

    private void Start() {
        GetComponent<Player>().OnPlayerDamaged += OnPlayerDamagedHandler;
        GetComponent<Player>().OnPlayerDied += OnPlayerDiedHandler;
        GetComponent<PlayerDash>().OnPlayerDash += OnPlayerDashHandler;
        GetComponentInChildren<PlayerFire>().GetComponent<Weapon>().OnWeaponFired += OnPlayerFireHandler;
        GetComponentInChildren<PlayerAbility>().GetComponent<Weapon>().OnWeaponFired += OnPlayerAbilityHandler;
        GetComponentInChildren<PlayerFire>().OnPlayerFireNoAmmo += OnPlayerNoAmmoHandler;
        GetComponentInChildren<PlayerAbility>().OnPlayerSpecialNoAmmo += OnPlayerNoAmmoHandler;
        GetComponent<PlayerScoreBoost>().OnScoreBoostActivated += OnScoreBoostActivatedHandler;
        GetComponent<PlayerScoreBoost>().OnScoreBoostDeactivated += OnScoreBoostDeactivatedHandler;
    }

    private void OnPlayerAbilityHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(specialAudio);
    }

    private void OnPlayerFireHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(fireAudio);
    }

    private void OnPlayerDashHandler(object sender, Player.DurationEventArgs e) {
        AudioUtils.PlayRandom(dashAudio, 0.5f);
    }

    private void OnPlayerDiedHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(diedAudio);
    }

    private void OnPlayerDamagedHandler(object sender, EventArgs e) { 
        AudioUtils.PlayRandom(damagedAudio);
    }

    private void OnPlayerNoAmmoHandler(object sender, EventArgs e) { 
        AudioUtils.PlayRandom(noAmmoAudio);
    }

    private void OnScoreBoostActivatedHandler(object sender, EventArgs e) { 
        AudioUtils.PlayRandom(scoreBoostActivatedAudio);
    }

    private void OnScoreBoostDeactivatedHandler(object sender, EventArgs e) { 
        AudioUtils.PlayRandom(scoreBoostDeactivatedAudio);
    }
}