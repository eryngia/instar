using System.Collections;
using UnityEngine;

// Manages progress of waves within a level.
public class LevelManager : MonoBehaviour {

    [SerializeField] private LevelSO levelSO;

    private void Start() {
        int difficulty = GameManager.Instance.NextLevel();
        
        // Generate new enemies for the new difficulty.
        EnemySOGenerator enemySOGenerator = GetComponentInChildren<EnemySOGenerator>();
        int generatedEnemyCount = GameManager.Instance.GetGeneratedEnemySOs().Count;
        for (int i = generatedEnemyCount; i < 2 + difficulty; i++) {
            GameManager.Instance.AddGeneratedEnemySO(new[] {
                enemySOGenerator.Generate(i), 
                enemySOGenerator.Generate(i)
            });
        }

        levelSO = GetComponentInChildren<LevelSOGenerator>().Generate(difficulty);
        StartCoroutine(StartLevel());
    }

    // Create waves at set intervals following level SO, then trigger level complete.
    private IEnumerator StartLevel() {
        for (int i = 0; i < levelSO.waveSOs.Count; i++) {
            yield return new WaitForSeconds(levelSO.waveIntervals[i]);
            StartCoroutine(RunWaveManager(levelSO.waveSOs[i]));
        }

        yield return new WaitForSeconds(levelSO.waveIntervals[levelSO.waveIntervals.Count - 1]);
        SceneManager.Instance.LoadContinueScene();
    }
    
    // Create a new wave manager object to run wave.
    private IEnumerator RunWaveManager(WaveSO waveSO) {
        GameObject container = new GameObject("Wave Manager");
        container.transform.SetParent(gameObject.transform);

        WaveManager waveManager = container.AddComponent<WaveManager>();
        waveManager.Initialize(waveSO);
        yield return waveManager.StartCoroutine(waveManager.SpawnWave());

        Destroy(container);
    }

}
