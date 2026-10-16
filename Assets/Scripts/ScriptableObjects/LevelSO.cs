
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSO", menuName = "LevelSO")]
public class LevelSO : ScriptableObject {
    public List<WaveSO> waveSOs; // waves
    public List<float> waveIntervals; // intervals between waves
}