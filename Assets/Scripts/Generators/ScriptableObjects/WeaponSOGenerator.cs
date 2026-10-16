using UnityEngine;

// Generate weapon scriptable objects with given difficulty.
public class WeaponSOGenerator : MonoBehaviour {

    private ProjectileSOGenerator projectileSOGenerator;

    private void Awake() {
        projectileSOGenerator = GetComponent<ProjectileSOGenerator>();
    }

    public EnemyWeaponSO Generate(int difficulty) {

        // divide difficulty between aspects
        int[] difficultyDivisions = RandomUtils.Divide(difficulty, 5);
        int fireRateDifficulty = difficultyDivisions[0];
        int countDifficulty = difficultyDivisions[1];
        int burstDifficulty = difficultyDivisions[2];
        int projectileDifficulty = difficultyDivisions[3] + difficultyDivisions[4];
        
        EnemyWeaponSO weaponSO = ScriptableObject.CreateInstance<EnemyWeaponSO>();

        weaponSO.fireRate = RandomUtils.RangeAvg(0.5f, 1f) + fireRateDifficulty * 0.3f;

        weaponSO.projectileRotation = RandomUtils.RangeAvg(10f, 25f);
        weaponSO.projectileCount = 1 + Mathf.RoundToInt(RandomUtils.RangeAvg(1f, 1.3f) * countDifficulty);
    
        weaponSO.burstDelay = RandomUtils.RangeAvg(0.25f, 0.75f);
        weaponSO.burstCount = burstDifficulty;
        
        if (Random.value < 0.2) weaponSO.inaccuracy = RandomUtils.RangeAvg(5f, 15f);

        EnemyProjectileSO projectileSO = projectileSOGenerator.Generate(projectileDifficulty);
        weaponSO.projectileSO = projectileSO;

        return weaponSO;
    }
}