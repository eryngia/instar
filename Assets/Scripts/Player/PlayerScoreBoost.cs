using System;
using UnityEngine;

// Streak system to activate boost when quickly acquiring score.
public class PlayerScoreBoost : MonoBehaviour {

    public event EventHandler<EventArgs> OnScoreBoostActivated;
    public event EventHandler<EventArgs> OnScoreBoostDeactivated;
    
    [SerializeField] private float decay = 5f;
    [SerializeField] private float boostDecay = 20f;
    [SerializeField] private float boostScoreMultiplier = 2f;
    [SerializeField] private float boostThreshold = 100f;
    [SerializeField] private float boost = 10f;

    [SerializeField] private float weaponDamageBoost;
    [SerializeField] private float weaponSpeedBoost;
    [SerializeField] private float weaponSizeBoost;
    [SerializeField] private float weaponRangeBoost;
    [SerializeField] private float playerSpeedBoost;
    [SerializeField] private float scoreMultiplier;

    [SerializeField] private float boostScore  = 0f;
    private int previousScore = 0;
    private bool isBoosted = false;
    private PlayerMove playerMove;
    private float baseMoveSpeed;
    private PlayerWeapon playerWeaponMain;
    private PlayerProjectileSO baseProjectileMainSO;
    private PlayerWeapon playerWeaponSpecial;
    private PlayerProjectileSO baseProjectileSpecialSO;
    
    private void Start() {
        Player player = GetComponent<Player>();
        playerMove = GetComponent<PlayerMove>();
        playerWeaponMain = player.GetComponentInChildren<PlayerFire>().GetComponent<PlayerWeapon>();
        playerWeaponSpecial = player.GetComponentInChildren<PlayerAbility>().GetComponent<PlayerWeapon>();

        GameManager.Instance.OnScoreChanged += OnScoreChangedHandler;
        player.OnPlayerDamaged += OnPlayerDamagedHandler; 
    }

    private void Update() {
        // decay faster when boosted
        boostScore -= (isBoosted ? boostDecay : decay) * Time.deltaTime;

        // if threshold reached, activate boost
        if (!isBoosted && boostScore >= boostThreshold)
            ActivateBoost();

        // if boost run out, deactivate boost
        if (isBoosted && boostScore <= 0)
            DeactivateBoost();     

        // cannot have more boost score than threshold
        boostScore = Mathf.Clamp(boostScore, 0f, boostThreshold);
    }

    private void OnScoreChangedHandler(object sender, EventArgs e) {
        // calculate change in score from previous score
        int scoreGained = GameManager.Instance.GetScore() - previousScore;
        if (scoreGained <= 0) return;
        previousScore += scoreGained;

        // add to boost score
        // easier to maintain boost than to acquire it
        boostScore += (isBoosted ? boostScoreMultiplier : 1f) * boost;
    }

    private void OnPlayerDamagedHandler(object sender, EventArgs e) {
        // deactivate boost if hit
        if (isBoosted) DeactivateBoost();
    }

    // Activate boost benefits for player weapon, speed and score multiplier.
    private void ActivateBoost() {
        isBoosted = true;
        boostScore = boostThreshold;
        GameManager.Instance.scoreMultiplier = scoreMultiplier;

        baseProjectileMainSO = playerWeaponMain.weaponSO.projectileSO;
        baseProjectileSpecialSO = playerWeaponSpecial.weaponSO.projectileSO;
        ActivateWeaponBoost(playerWeaponMain);
        ActivateWeaponBoost(playerWeaponSpecial);

        baseMoveSpeed = playerMove.moveSpeed;
        playerMove.moveSpeed *= playerSpeedBoost;

        OnScoreBoostActivated?.Invoke(this, EventArgs.Empty);
    }

    // Activate boost benefits for given weapon.
    private void ActivateWeaponBoost(PlayerWeapon weapon) {
        PlayerProjectileSO projectileSO = Instantiate(weapon.weaponSO.projectileSO);
        projectileSO.damage = (int) (projectileSO.damage * weaponDamageBoost);
        projectileSO.speed = (int) (projectileSO.speed * weaponSpeedBoost);
        projectileSO.width = (int) (projectileSO.width * weaponSizeBoost);
        projectileSO.height = (int) (projectileSO.height * weaponSizeBoost);
        projectileSO.range = (int) (projectileSO.range * weaponRangeBoost);
        weapon.weaponSO.projectileSO = projectileSO;
    }

    // Deactivate boost benefits for player weapon, speed and score multiplier.
    private void DeactivateBoost() {
        isBoosted = false;
        boostScore = 0f;
        GameManager.Instance.scoreMultiplier = 1f;

        playerWeaponMain.weaponSO.projectileSO = baseProjectileMainSO;
        playerWeaponSpecial.weaponSO.projectileSO = baseProjectileSpecialSO;
        playerMove.moveSpeed = baseMoveSpeed;

        OnScoreBoostDeactivated?.Invoke(this, EventArgs.Empty);
    }

    // Get percentage process towards activating boos.
    public float GetBoostProgress() {
        return boostScore / boostThreshold;
    }

    // Is player currently boosted.
    public bool IsBoosted() {
        return isBoosted;
    }


}