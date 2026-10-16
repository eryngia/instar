using System.Collections.Generic;
using UnityEngine;

// Performs Voronoi partitioning.
public static class VoronoiDiagram {

    // Performs Delaunay triangulation, then convert to Voronoi cells.
    public static List<VoronoiCell> Partition(List<Vector2> sites, float width, float height, float maxEdgeLength) {
        List<Triangle2> delauneyTriangles = DelauneyTriangulation.Triangulate(sites, width, height);
        return DelauneyToVoronoi(delauneyTriangles, width, height, maxEdgeLength);
    }

    // Convert Delaunay triangles to Voronoi cells.
    public static List<VoronoiCell> DelauneyToVoronoi(List<Triangle2> delauneyTriangles, float width, float height, float maxEdgeLength) {
        
        // map edges to the circumcentres of their triangles
        Dictionary<Edge2, List<Vector2>> edgeToCircumcentres = new Dictionary<Edge2, List<Vector2>>();
        foreach (Triangle2 triangle in delauneyTriangles) {
            Vector2 circumcenter = triangle.Circumcircle().c;
            foreach (Edge2 edge in triangle.edges) {
                AddListValue(edgeToCircumcentres, edge, circumcenter);
            }
        }

        // map sites to Voronoi edges
        Dictionary<Vector2, List<Edge2>> siteToEdges = new Dictionary<Vector2, List<Edge2>>();
        foreach (var edgeWithCircumcentres in edgeToCircumcentres) {
            if (edgeWithCircumcentres.Value.Count == 2) {
                Vector2 site1 = edgeWithCircumcentres.Key.v1;
                Vector2 site2 = edgeWithCircumcentres.Key.v2;
                Edge2 edge = new Edge2(edgeWithCircumcentres.Value[0], edgeWithCircumcentres.Value[1]);

                if (IsEdgeWithinBounds(edge)) {
                    AddListValue(siteToEdges, site1, edge);
                    AddListValue(siteToEdges, site2, edge);
                }
            } 
        }

        // form voronoi cells from sites and edges
        List<VoronoiCell> voronoiCells = new List<VoronoiCell>();
        foreach (var siteWithEdges in siteToEdges) {
            Edge2[] sortedEdges = Edge2.GetSingleEdgePath(siteWithEdges.Value);
            bool tooLong = false;

            // filter edges that do not form continuous cycle
            if (sortedEdges != null) {
                foreach (var edge in sortedEdges) {
                    if (Vector2.Distance(edge.v1, edge.v2) > maxEdgeLength) tooLong = true;
                }
            }

            // filter cells with edges that are too long
            if (sortedEdges != null && !tooLong) {
                voronoiCells.Add(new VoronoiCell(siteWithEdges.Key, sortedEdges));
            }
        }

        return voronoiCells;

        // if both vertices of the edge are within bounds
        bool IsEdgeWithinBounds(Edge2 edge) {
            return IsVertexWithinBounds(edge.v1) && IsVertexWithinBounds(edge.v2);
        }

        // if a vertex is within bounds
        bool IsVertexWithinBounds(Vector2 vector) {
            return vector.x >= 0 && vector.x <= width && vector.y >= 0 && vector.y <= height;
        }

        // add a value to a dictionary list, or create a new list if none exists
        void AddListValue<K, V>(Dictionary<K, List<V>> dict, K key, V value) {
            if (!dict.ContainsKey(key)) {
                dict[key] = new List<V> {value};
            } else {
                dict[key].Add(value);
            }
        }
    }

    // Return the vertex paths forming the boundary of the Voronoi diagram.
    public static List<Vector2[]> GetBoundaryVertexPaths(List<VoronoiCell> cells) {
        return Edge2.GetVertexPaths(GetBoundaryEdgePaths(cells));
    }
    
    // Return the edge paths forming the boundary of the Voronoi diagram.
    public static List<Edge2[]> GetBoundaryEdgePaths(List<VoronoiCell> cells) {
        List<Edge2> outerEdges = new List<Edge2>();

        // filter outer edges, which are not shared between multiple cells
        foreach (VoronoiCell cell in cells) {
            foreach (Edge2 edge in cell.edges) {
                bool outer = true;
                foreach (VoronoiCell otherCell in cells) {
                    if (cell != otherCell) {
                        foreach (Edge2 otherEdge in otherCell.edges) {
                            if (edge.Equals(otherEdge)) {
                                outer = false;
                                break;
                            }
                        }
                    }
                    if (!outer) break;
                }
                if (outer) outerEdges.Add(edge);
            }
        }

        return Edge2.GetEdgePaths(outerEdges);
    }

    
}