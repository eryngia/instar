using System.Collections.Generic;
using UnityEngine;

// The environment on the edges of the screen.
public class Environment : MonoBehaviour, IHittable {

    [SerializeField] private int hitDamage = 1;
    [SerializeField] private int width = 500;
    [SerializeField] private int height = 3000;
    [SerializeField] private int radius = 60;
    [SerializeField] private float edgeThickness = 1f;
    [SerializeField] private int totalSections = 10;
    [SerializeField] private int initialSections = 3;
    [SerializeField] private Color edgeColor = Color.white;


    private void Start() {
        // create environment with curve and hitbox 
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        LevelCreator levelCreator = gameObject.AddComponent<LevelCreator>();
        List<VoronoiCell> voronoiCells = levelCreator.CreateCurve(spriteRenderer, totalSections, initialSections, width, height, radius, edgeThickness, edgeColor);            
        CreateHitbox(voronoiCells);
    }

    // Create composite collider out of polygon colliders for each Voronoi boundary.
    private void CreateHitbox(List<VoronoiCell> voronoiCells) {
        List<Vector2[]> boundaries = VoronoiDiagram.GetBoundaryVertexPaths(voronoiCells);
        CompositeCollider2D composite = GetComponentInChildren<CompositeCollider2D>();
        foreach (Vector2[] vertices in boundaries) {
            if (vertices.Length < 3) continue;
            PolygonCollider2D polygon = composite.gameObject.AddComponent<PolygonCollider2D>();
            polygon.SetPath(0, vertices);
            polygon.usedByComposite = true;
        }
        Physics2D.SyncTransforms();
        composite.gameObject.transform.localScale /= GameManager.Constants.pixelsPerUnit;
    }

    public void ReceiveHit(IHittable source, int damage) {}

    public int Hit(IHittable target) {
        // player impacts environment
        return hitDamage;
    }
}
