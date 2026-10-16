
using UnityEngine;

[CreateAssetMenu(fileName = "PathSO", menuName = "PathSO", order = 0)]
public class PathSO : ScriptableObject {
    
    public Transform[] waypoints; // points along path

}
