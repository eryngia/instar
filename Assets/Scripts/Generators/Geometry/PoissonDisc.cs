using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Performs Poisson disc algorithm to generate points.
public static class PoissonDisc {
    
    // Generate randomly evenly distributed points at least radius apart.
    public static List<Vector2> GeneratePoints(float width, float height, float radius, int maxAttempts = 30) {
            
        float radiusSquared = radius * radius;
        float annulus = 3 * radiusSquared;
        float cellSize = radius * Mathf.Sqrt(0.5f);

        // initialize grid
        int gridWidth = Mathf.CeilToInt(width / cellSize);
        int gridHeight = Mathf.CeilToInt(height / cellSize);
        Vector2[,] grid = new Vector2[gridWidth, gridHeight];

        // list of active points
        Vector2[] queue = new Vector2[gridWidth * gridHeight];
        int queueLength = 0;

        AddPoint(new Vector2(Random.Range(0, width), Random.Range(0, height)));
        
        while (queueLength > 0) {
            bool found = false;

            // pick random active point
            int i = Random.Range(0, queueLength);
            Vector2 point = queue[i];

            // try to generate a new point in the annulus, up to max attempts
            for (int attempt = 0; attempt < maxAttempts; attempt++) {
                Vector2 nextPoint = GenerateNewPoint(point);
                if (IsWithinBounds(nextPoint) && !IsTooClose(nextPoint, radius)) {
                    AddPoint(nextPoint);
                    found = true;
                    break;
                }
            }
            
            if (!found) 
                queue[i] = queue[queueLength-- - 1];
        }

        return grid.Cast<Vector2>().ToList();
    
        // add point to active list and grid
        void AddPoint(Vector2 point) {
            queue[queueLength++] = point;
            grid[(int) (point.x / cellSize), (int) (point.y / cellSize)] = point;
        }

        // generate new point in annulus around point
        Vector2 GenerateNewPoint(Vector2 aroundPoint) {
            float angle = Random.value * Mathf.PI * 2;
            float distance = Mathf.Sqrt(Random.Range(radiusSquared, annulus + radiusSquared));
            return new Vector2(aroundPoint.x + distance * Mathf.Cos(angle), aroundPoint.y + distance * Mathf.Sin(angle));
        }
    
        // check if point is in bounds
        bool IsWithinBounds(Vector2 point) {
            return 0 <= point.x && point.x <= width 
                && 0 <= point.y && point.y <= height;
        }

        // check if point is too close to another point
        bool IsTooClose(Vector2 point, float radius) {
            foreach (var otherPoint in GetNeighbourhood(point, 2)) {
                if (Vector2.Distance(point, otherPoint) < radius)
                    return true;
            }

            return false;
        }
    
        // return the points in the neighbouring grid cells of a point
        List<Vector2> GetNeighbourhood(Vector2 point, int range) {
            List<Vector2> neighbourhood = new List<Vector2>();

            int pointX = (int) (point.x / cellSize);
            int pointY = (int) (point.y / cellSize);
            int minX = Mathf.Max(pointX - range, 0);
            int minY = Mathf.Max(pointY - range, 0);
            int maxX = Mathf.Min(pointX + range + 1, gridWidth);
            int maxY = Mathf.Min(pointY + range + 1, gridHeight);

            for (int x = minX; x < maxX; x++) {
                for (int y = minY; y < maxY; y++) {
                    Vector2 otherPoint = grid[x, y];
                    if (otherPoint != Vector2.zero)
                        neighbourhood.Add(otherPoint);
                }
            }

            return neighbourhood;
        }
    }
}