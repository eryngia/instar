using UnityEngine;

// Generate enemy scriptable objects with given difficulty.
public class EnemySOGenerator : MonoBehaviour {

    private WeaponSOGenerator weaponSOGenerator;

    private void Awake() {
        weaponSOGenerator = GetComponent<WeaponSOGenerator>();
    }

    public EnemySO Generate(int difficulty) {        
        EnemySO enemySO = ScriptableObject.CreateInstance<EnemySO>();

        if (difficulty > 1 && Random.value < 0.05) {
            enemySO.deathExplosionSO = ScriptableObject.CreateInstance<ExplosionSO>();
            enemySO.deathExplosionSO.damage = 1;
            enemySO.deathExplosionSO.size = RandomUtils.RangeHigh(10, 20);
            enemySO.deathExplosionSO.rate = 2;
            enemySO.deathExplosionSO.sprite = GameManager.Constants.enemyProjectileSprite;
        }

        // divide difficulty between aspects
        int[] difficultyDivisions = RandomUtils.Divide(difficulty, 6);
        int healthDifficulty = difficultyDivisions[0];
        int trackingDifficulty = difficultyDivisions[1];
        int weaponDifficulty = difficultyDivisions[2] + difficultyDivisions[3] + difficultyDivisions[4];

        enemySO.collisionDamage = 1;

        enemySO.score = (difficulty + 1) * 2;

        enemySO.speed = RandomUtils.RangeLow(2, 4);
        if (Random.value < 0.1) {
            weaponDifficulty = 1;
            healthDifficulty = 0;
            enemySO.speed = RandomUtils.RangeAvg(4, 6);
        }

        enemySO.trackingStrength = RandomUtils.RangeAvg(5, 10) * trackingDifficulty;

        enemySO.animationSpeed = 1 / RandomUtils.RangeAvg(2f, 4f);

        (Texture2D[] textures, int width, int height) = InsectTextureGenerator.Generate(64, difficulty / 2);
        Sprite sprite1 = TextureUtils.ToSprite(textures[0], new Vector2(0.5f, 0.5f));
        Sprite sprite2 = TextureUtils.ToSprite(textures[1], new Vector2(0.5f, 0.5f));
        enemySO.sprites = new Sprite[] { sprite1, sprite2 };
        enemySO.width = width;
        enemySO.height = height;

        enemySO.health = Mathf.RoundToInt((enemySO.height * enemySO.width / 100) + RandomUtils.RangeAvg(0, 3) + healthDifficulty * 3 + difficulty);

        enemySO.weaponSO = weaponSOGenerator.Generate(weaponDifficulty);
        
        return enemySO;
    }

}