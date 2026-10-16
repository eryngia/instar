
using UnityEngine;

// The background behind the action.
public class Background : MonoBehaviour {

    [SerializeField] private int width = 500;
    [SerializeField] private int height = 3000;
    [SerializeField] private int radius = 60;
    [SerializeField] private float edgeThickness = 1f;
    [SerializeField] private int totalSections = 10;
    [SerializeField] private int initialSections = 3;
    [SerializeField] private Color edgeColor = Color.grey;

    private void Start() {
        // create background with no curve or hitbox
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        LevelCreator levelCreator = gameObject.AddComponent<LevelCreator>();
        levelCreator.Create(spriteRenderer, totalSections, initialSections, width, height, radius, edgeThickness, edgeColor);            
    }

}
