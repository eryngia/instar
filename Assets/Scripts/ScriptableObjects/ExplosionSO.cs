
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionSO", menuName = "ExplosionSO", order = 0)]
public class ExplosionSO : ScriptableObject {

    public int damage; // damage
    public float size; // pixel radius
    public float rate; // pixels grown in 0.1 seconds
    public Sprite sprite; // sprite
}