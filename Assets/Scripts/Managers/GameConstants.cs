using UnityEngine;

// Global constants accessible for entire game through game manager.
public class GameConstants : MonoBehaviour {

    public int playerLayer {get; private set;}
    public int playerProjectileLayer {get; private set;}
    public int enemyLayer {get; private set;}
    public int enemyProjectileLayer {get; private set;}
    public float screenWidth;
    public float screenHeight;
    public float pixelsPerUnit;
    public Sprite playerProjectileSprite;
    public Sprite playerRaySprite;
    public Sprite enemyProjectileSprite;
    public Sprite enemyRaySprite;
    public AudioClip[] enemyFireAudio;
    public AudioClip[] enemyDamagedAudio;
    public AudioClip[] enemyDiedAudio;
    public AudioClip[] explosionAudio;

    private void Awake() {
        playerLayer = LayerMask.NameToLayer("Player");
        playerProjectileLayer = LayerMask.NameToLayer("PlayerProjectile");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyProjectileLayer = LayerMask.NameToLayer("EnemyProjectile");
    }

}