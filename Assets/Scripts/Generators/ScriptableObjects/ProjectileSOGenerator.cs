using UnityEngine;

// Generate enemy scriptable objects with given difficulty.
public class ProjectileSOGenerator : MonoBehaviour {

    public EnemyProjectileSO Generate(int difficulty) {
        EnemyProjectileSO projectileSO = ScriptableObject.CreateInstance<EnemyProjectileSO>();

        // chance to have a special ability
        if (difficulty >= 1 && Random.value < 0.3) {
            difficulty -= 1;
            float rnd = Random.value;
            if (rnd < 1/3) {
                projectileSO.homingStrength = RandomUtils.RangeAvg(15, 30);
            } else if (rnd < 1/3) {
                projectileSO.explosionSO = ScriptableObject.CreateInstance<ExplosionSO>();
                projectileSO.explosionSO.damage = 1;
                projectileSO.explosionSO.size = RandomUtils.RangeAvg(10, 20);
                projectileSO.explosionSO.rate = 1;
                projectileSO.explosionSO.sprite = GameManager.Constants.enemyProjectileSprite;
            } else {
                projectileSO.ray = true;
                projectileSO.rayWarmUp = 1f;
                projectileSO.rayDuration = 2f;
            }        
        }

        // divide difficulty between aspects
        int[] difficultyDivisions = RandomUtils.Divide(difficulty, 2);
        int speedDifficulty = difficultyDivisions[0];
        int sizeDifficulty = difficultyDivisions[1];

        projectileSO.damage = 1;

        if (projectileSO.explosionSO != null)
            projectileSO.range = 6f;
        else if (projectileSO.homingStrength > 0) 
            projectileSO.range = 20f;
        else 
            projectileSO.range = 14f;

        projectileSO.speed = RandomUtils.RangeAvg(3f, 5f) + speedDifficulty * 1f;
        
        projectileSO.height = projectileSO.ray ? 200f : RandomUtils.RangeAvg(6f, 10f) + sizeDifficulty * 1f;
        projectileSO.width = RandomUtils.RangeAvg(3f, 5f) + sizeDifficulty * 1f;
        if (projectileSO.ray) projectileSO.width *= 1.5f;


        return projectileSO;
    }
}