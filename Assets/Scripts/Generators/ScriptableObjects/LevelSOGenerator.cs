using System.Collections.Generic;
using UnityEngine;

// Generate level scriptable objects with given difficulty.
public class LevelSOGenerator : MonoBehaviour {
                                         
    private WaveSOGenerator waveSOGenerator;

    private void Awake() {
        waveSOGenerator = GetComponent<WaveSOGenerator>();
    }

    public LevelSO Generate(int difficulty) {
        LevelSO levelSO = ScriptableObject.CreateInstance<LevelSO>();
        List<EnemySO[]> enemies = GameManager.Instance.GetGeneratedEnemySOs();

        levelSO.waveSOs = new List<WaveSO>{ waveSOGenerator.Generate(RandomUtils.FromArray(enemies[difficulty - 1]), Random.Range(3, 4)) };
        levelSO.waveIntervals = new List<float>{ RandomUtils.RangeAvg(5f, 8f) };

        // initial waves
        for (int i = 0; i < Random.Range(3, 5); i++) {
            for (int j = 0; j < Random.Range(2, 5); j++) {
                levelSO.waveIntervals.Add(RandomUtils.RangeAvg(3, 6));
                int enemyDifficulty = RandomUtils.Weighted(difficulty - 1, 5-i, difficulty, 2, difficulty + 1, i); // prefer easier enemies
                levelSO.waveSOs.Add(
                    waveSOGenerator.Generate(
                        RandomUtils.FromArray(enemies[enemyDifficulty]), // random enemy of given difficulty
                        RandomUtils.RangeAvgInt(3, 5) + difficulty - enemyDifficulty // more enemies if easy, less if hard
                )); 
            }
            levelSO.waveIntervals.Add(RandomUtils.RangeAvg(6, 9));
        }
        
        // final waves
        for (int i = 0; i < Random.Range(4, 6); i++) { 
            levelSO.waveIntervals.Add(RandomUtils.RangeAvg(1.5f, 3.5f));
            int enemyDifficulty = RandomUtils.Weighted(difficulty - 1, 1, difficulty, 2, difficulty + 1, 3); // prefer difficulty enemies
            levelSO.waveSOs.Add(
                waveSOGenerator.Generate(
                    RandomUtils.FromArray(enemies[enemyDifficulty]),// random enemy of given difficulty
                    RandomUtils.RangeAvgInt(3, 6) // many enemies regardless of difficulty
            ));
        }

        // level end interval
        levelSO.waveIntervals[levelSO.waveIntervals.Count - 1] = 15; 

        return levelSO;
    }

}