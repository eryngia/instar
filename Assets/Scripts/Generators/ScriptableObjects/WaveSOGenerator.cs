using UnityEngine;

// Generate wave scriptable objects with given difficulty.
public class WaveSOGenerator : MonoBehaviour {
                                         
    [SerializeField] private PathSO[] pathSOs;

    public WaveSO Generate(EnemySO enemySO, int count) {
        WaveSO waveSO = ScriptableObject.CreateInstance<WaveSO>();

        // waves are comprised of the same enemy
        waveSO.enemySOs = new EnemySO[count];
        for (int i = 0; i < count; i++) {
            waveSO.enemySOs[i] = enemySO;
        }

        waveSO.pathSO = RandomUtils.FromArray(pathSOs);
        waveSO.spawnInterval = RandomUtils.RangeAvg(0.5f, 2f);

        return waveSO;
    }
}