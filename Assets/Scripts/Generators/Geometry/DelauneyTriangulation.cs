using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Performs Delaunay triangulation. 
public static class DelauneyTriangulation {

    public static List<Triangle2> Triangulate(List<Vector2> vertices, float width, float height) {

        // make super triangle and add to list
        Triangle2 superTriangle = MakeSuperTriangle();
        List<Triangle2> triangles = new List<Triangle2> {superTriangle};

        // add each vertex to triangulation
        foreach (Vector2 vertex in vertices) {
            AddVertex(vertex);
        }
        
        // remove triangles sharing vertex with super triangle
        return triangles.Where(triangle => !triangle.SharesVertex(superTriangle)).ToList();


        void AddVertex(Vector2 vertex) {
            List<Edge2> edges = new List<Edge2>();

            // remove triangles whose circumcircle contains vertex
            triangles = triangles.Where(triangle => {
                    if (triangle.Circumcircle().Contains(vertex)) {
                        edges.AddRange(triangle.edges);
                        return false;
                    } else {
                        return true;
                    }
                }).ToList();

            // add triangles to connect vertex with unique edges of removed triangles
            foreach (Edge2 edge in Unique(edges)) {
                triangles.Add(new Triangle2(edge.v1, edge.v2, vertex));
            }
        }

        // make right triangle around rectangular area
        Triangle2 MakeSuperTriangle() {
            return new Triangle2(new Vector2(-1, -1), new Vector2(-1, width + height + 1), new Vector2(width + height + 1, -1));
        }

        // filter unique items
        List<T> Unique<T>(List<T> list) {
            return list.GroupBy(item => item)         
                .Where(group => group.Count() == 1)  
                .Select(group => group.Key)    
                .ToList();
        }
    }
}