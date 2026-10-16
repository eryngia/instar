using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;

// Manages global variables and behaviour across scenes.
public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public static GameConstants Constants => Instance.constants;

    private GameConstants constants;

    public event EventHandler<EventArgs> OnHealthChanged; 
    public event EventHandler<EventArgs> OnAmmoChanged;
    public event EventHandler<EventArgs> OnScoreChanged;
   
    [SerializeField] private int startAmmo;
    [SerializeField] private int startHealth;
    [SerializeField] private List<PowerSO> powers;

    private int maxAmmo, maxHealth;
    private int ammo, health, score, totalScore;
    private List<PowerSO> unseenPowers;
    private List<PowerSO> chosenPowers;
    private List<PowerSO> notChosenPowers;
    private int level = 0;
    private List<EnemySO[]> generatedEnemySOs;
    public float scoreMultiplier;

    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            constants = GetComponent<GameConstants>();
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        ammo = maxAmmo;
    }

    private void Start() {
        Restart();
    }

    // Reset all game manager variables to their initial state.
    public void Restart() {
        Random.InitState(Random.Range(0, int.MaxValue));
        ammo = maxAmmo = startAmmo;
        health = maxHealth = startHealth;
        score = totalScore = 0;
        level = 0;
        unseenPowers = new List<PowerSO>(powers);
        chosenPowers = new List<PowerSO>();
        notChosenPowers = new List<PowerSO>();
        generatedEnemySOs = new List<EnemySO[]>();
        scoreMultiplier = 1f;
    }

    // Set ammo to maximum.
    public void ReloadAmmo() {
        ammo = maxAmmo;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    // Add to current ammo.
    public void GainAmmo(int ammoGained) {
        ammo += ammoGained;
        if (ammo > maxAmmo) ammo = maxAmmo;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    // Remove from current ammo.
    public void UseAmmo(int ammoUsed) {
        ammo -= ammoUsed;
        if (ammo < 0) ammo = 0;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get current ammo.
    public int GetAmmo() {
        return ammo;
    }

    // Has at least one current ammo.
    public bool HasAmmo() {
        return ammo > 0;
    }

    // Add to maximum ammo.
    public void AddMaxAmmo(int maxAmmoAdded) {
        maxAmmo += maxAmmoAdded;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get maximum ammo.
    public int GetMaxAmmo() {
        return maxAmmo;
    }

    // Remove current health.
    public void LoseHealth(int damage) {
        health -= damage;
        if (health < 0) health = 0;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    // Add to current health.
    public void GainHealth(int healthGained) {
        health += healthGained;
        if (health > maxHealth) health = maxHealth;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get current health.
    public int GetHealth() {
        return health;
    }

    // Has zero current health.
    public bool NoHealth() {
        return health == 0;
    }

    // Add to maximum health.
    public void AddMaxHealth(int maxHealthAdded) {
        maxHealth += maxHealthAdded;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get maximum health.
    public int GetMaxHealth() {
        return maxHealth;
    }

    // Get current score.
    public int GetScore() {
        return score;
    }

    // Get total score.
    public int GetTotalScore() {
        return totalScore;
    }

    // Add to score.
    public void AddScore(int scoreAdded) {
        scoreAdded = (int) (scoreAdded * scoreMultiplier);
        score += scoreAdded;
        totalScore += scoreAdded;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    // Remove from score.
    public void SpendScore(int scoreSpent) {
        score -= scoreSpent;
        totalScore -= scoreSpent;
        if (score < 0) score = 0;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    // Select power from options, adding it to active powers.
    public void ChoosePower(PowerSO power) {
        chosenPowers.Add(power);
        notChosenPowers.Remove(power);
    }
    
    // Get active powers.
    public List<PowerSO> GetChosenPowers() {
        return chosenPowers; 
    }

    // Get active power of type.
    public T GetChosenPower<T>() {
        return chosenPowers.OfType<T>().FirstOrDefault();
    }

    // Get a random power from those not offered as choices yet.
    public PowerSO GetPowerChoice() {
        if (unseenPowers.Count == 0) 
            unseenPowers.AddRange(notChosenPowers);

        PowerSO power = unseenPowers.OrderBy(x => Random.value).First();

        unseenPowers.Remove(power);
        notChosenPowers.Add(power);
        return power;
    }

    // Return and increment level number.
    public int NextLevel() {
        return ++level;
    }

    // Get current array of generated enemy SOs by difficulty.
    public List<EnemySO[]> GetGeneratedEnemySOs() {
        return generatedEnemySOs;
    }

    // Add array of enemies of the next difficulty to the array of enemy SOs.
    public void AddGeneratedEnemySO(EnemySO[] enemySO) {
        generatedEnemySOs.Add(enemySO);
    }

}
