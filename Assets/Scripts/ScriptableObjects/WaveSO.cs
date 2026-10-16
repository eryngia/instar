
using UnityEngine;

[CreateAssetMenu(fileName = "PathSO", menuName = "WaveSO")]
public class WaveSO : ScriptableObject {
    public EnemySO[] enemySOs; // enemies in wave
    public PathSO pathSO; // path of wave
    public float spawnInterval; // seconds between enemy spawns
}