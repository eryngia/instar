using System;
using UnityEngine;

// Plays audio for events relating to the attached enemy.
public class EnemyAudio : MonoBehaviour {

    [SerializeField] private AudioClip[] damagedAudio;
    [SerializeField] private AudioClip[] diedAudio;
    [SerializeField] private AudioClip[] fireAudio;
    [SerializeField] private float volume = 0.75f;

    private void Start() {
        damagedAudio = GameManager.Constants.enemyDamagedAudio;
        diedAudio = GameManager.Constants.enemyDiedAudio;
        fireAudio = GameManager.Constants.enemyFireAudio;

        GetComponent<Enemy>().OnEnemyDamaged += OnEnemyDamagedHandler;
        GetComponent<Enemy>().OnEnemyDied += OnEnemyDiedHandler;
        GetComponent<EnemyWeapon>().OnWeaponFired += OnEnemyFireHandler;
    }

    private void OnEnemyFireHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(fireAudio, volume / 5);
    }

    private void OnEnemyDiedHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(diedAudio, volume);
    }

    private void OnEnemyDamagedHandler(object sender, EventArgs e) { 
        AudioUtils.PlayRandom(damagedAudio, volume);
    }
}