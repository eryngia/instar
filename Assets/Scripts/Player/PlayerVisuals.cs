using System;
using System.Collections;
using UnityEngine;

// Handles visual effects for the player.
public class PlayerVisuals : Visuals {
    
    [SerializeField] private Material playerFlashMaterial;
    [SerializeField] private float playerDamagedFlashAlpha = 0.1f;
    [SerializeField] private float playerDamagedFlashInterval = 0.2f;
    [SerializeField] private float playerDamagedShakeMagnitude = 0.25f;
    [SerializeField] private float playerDamagedShakeDuration = 0.5f;
    [SerializeField] private float playerDashShakeMagnitude = 0.15f;
    [SerializeField] private float playerDashShakeDuration = 0.05f;
    [SerializeField] private float playerFireShakeMagnitude = 0.1f;
    [SerializeField] private float playerFireShakeDuration = 0.01f;
    [SerializeField] private GameObject playerAfterimagePrefab;
    [SerializeField] private ParticleSystem playerDiedParticles;


    private const string ANIMATOR_ISMOVINGRIGHT = "isMovingRight";
    private const string ANIMATOR_ISMOVINGLEFT = "isMovingLeft";
    private const string ANIMATOR_ISBOOSTED = "isBoosted";

    private Player player;
    private Animator animator;
    private PlayerMove playerMove;
    private PlayerScoreBoost playerScoreBoost;
    
    private void Start() {
        flashMaterial = playerFlashMaterial;

        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerMove = player.GetComponent<PlayerMove>();
        playerScoreBoost = player.GetComponent<PlayerScoreBoost>();

        player.OnPlayerDamaged += OnPlayerDamagedHandler;
        player.OnPlayerDied += OnPlayerDiedHandler;
        
        player.GetComponent<PlayerDash>().OnPlayerDash += OnPlayerDashHandler;
        player.GetComponentInChildren<PlayerFire>().GetComponent<Weapon>().OnWeaponFired += OnPlayerFireHandler;
        player.GetComponentInChildren<PlayerAbility>().GetComponent<Weapon>().OnWeaponFired += OnPlayerAbilityHandler;
    }

    private void Update() {
        // set the correct animation state
        if (playerMove.IsMovingRight())
            animator.SetBool(ANIMATOR_ISMOVINGRIGHT, true);
        else animator.SetBool(ANIMATOR_ISMOVINGRIGHT, false);

        if (playerMove.IsMovingLeft())
            animator.SetBool(ANIMATOR_ISMOVINGLEFT, true);
        else animator.SetBool(ANIMATOR_ISMOVINGLEFT, false);

        if (playerScoreBoost.IsBoosted())
            animator.SetBool(ANIMATOR_ISBOOSTED, true);
        else animator.SetBool(ANIMATOR_ISBOOSTED, false);
    }

    // Shake screen and flash when damaged.
    private void OnPlayerDamagedHandler(object sender, Player.DurationEventArgs e) {
        StartCoroutine(ShakeScreen(playerDamagedShakeMagnitude, playerDamagedShakeDuration));
        StartCoroutine(Flashing(playerDamagedFlashAlpha, e.duration, playerDamagedFlashInterval));
    }

    // Explode when destroyed.
    private void OnPlayerDiedHandler(object sender, EventArgs e) {
        PlayParticles(playerDiedParticles);
    }

    // Shake screen and leave trail when dashing.
    private void OnPlayerDashHandler(object sender, Player.DurationEventArgs e) {
        StartCoroutine(ShakeScreen(playerDashShakeMagnitude, playerDashShakeDuration));
        StartCoroutine(LeaveAfterimages(e.duration, e.duration / 5, e.duration));
    }

    // Small screen shake when firing.
    private void OnPlayerFireHandler(object sender, EventArgs e) {
        StartCoroutine(ShakeScreen(playerFireShakeMagnitude, playerFireShakeDuration));
    }

    // Small screen shake when firing.
    private void OnPlayerAbilityHandler(object sender, EventArgs e) {
        StartCoroutine(ShakeScreen(playerFireShakeMagnitude, playerFireShakeDuration));
    }

    // Spawn afterimages at set intervals.
    private IEnumerator LeaveAfterimages(float duration, float interval, float lifetime) {
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            GameObject afterimage = Instantiate(playerAfterimagePrefab, transform.position, transform.rotation);
            StartCoroutine(Fade(afterimage, lifetime));

            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }
    }

    // Gradually reduce the alpha of an afterimage and destroy it when it is transparent.
    private IEnumerator Fade(GameObject afterimage, float lifetime) {
        SpriteRenderer spriteRenderer = afterimage.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        float elapsedTime = 0;

        while (elapsedTime < lifetime) {
            color.a = elapsedTime / lifetime;
            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(afterimage);
    }
}
