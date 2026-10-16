using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private WaveSO waveSO;
    private EnemyCreator enemyCreator;
    
    public void Initialize(WaveSO waveSO) {
        this.waveSO = waveSO;
        enemyCreator = gameObject.GetComponentInParent<EnemyCreator>();
    }

    // Spawn enemies in given intervals.
    public IEnumerator SpawnWave() {        
        foreach (var enemySO in waveSO.enemySOs) {
            enemyCreator.Create(enemySO, waveSO.pathSO);
            yield return new WaitForSeconds(waveSO.spawnInterval);
        }
    }

}
