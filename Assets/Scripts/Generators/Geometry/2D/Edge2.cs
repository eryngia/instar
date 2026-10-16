using System.Collections.Generic;
using UnityEngine;

// A 2D line segment.
public class Edge2 {

    public Vector2 v1 {get; private set;}
    public Vector2 v2 {get; private set;}

    public Edge2(Vector2 v1, Vector2 v2) {
        this.v1 = v1;
        this.v2 = v2;
    }

    public override bool Equals(object obj) {
        if (obj is Edge2 other)
            return (v1 == other.v1 && v2 == other.v2) || (v1 == other.v2 && v2 == other.v1);
        else 
            return false;
    }

    public override int GetHashCode() {
        return v1.GetHashCode() ^ v2.GetHashCode();
    }

    public override string ToString() {
        return $"Edge({v1}, {v2})";
    }

    // Convert a list of edge paths to a list of vertex paths.
    public static List<Vector2[]> GetVertexPaths(List<Edge2[]> edgePaths) {
        List<Vector2[]> vertexPaths = new List<Vector2[]>();
        foreach (Edge2[] edgePath in edgePaths) {
            vertexPaths.Add(GetVertexPath(edgePath));
        }
        return vertexPaths;
    }
    
    // Convert an edge path into a vertex path.
    public static Vector2[] GetVertexPath(Edge2[] edges) {
        Vector2[] path = new Vector2[edges.Length];

        path[0] = edges[0].v1;

        for (int i = 1, j = 0; j < edges.Length - 1; j++) {
            // choose which end of the edge connects to the previous
            if (edges[j].v1 == path[i - 1]) {
                path[i++] = edges[j].v2;
            } else {
                path[i++] = edges[j].v1;
            }
        }

        return path;
    }

    // Return a single continuous cycle formed of the given edges, or null otherwise.
    public static Edge2[] GetSingleEdgePath(List<Edge2> edges) {
        int count = edges.Count;
        Edge2[] edgePath = GetEdgePath(edges);
        return edgePath != null && edgePath.Length == count ? edgePath : null;
    }

    // Return any continuous cycle formed of the given edges, or null otherwise.
    // The edges in the path are removed from the list.
    public static Edge2[] GetEdgePath(List<Edge2> edges) {
        if (edges.Count < 3) return null;

        List<Edge2> sortedEdges = new List<Edge2> { edges[0] };
        edges.RemoveAt(0);

        Vector2 lastVertex = sortedEdges[0].v2;

        while (edges.Count > 0) {
            Edge2 nextEdge = null;

            // find valid next edge
            foreach (Edge2 edge in edges) {
                if (edge.v1 == lastVertex || edge.v2 == lastVertex) {
                    nextEdge = edge;
                    break;
                }
            }

            if (nextEdge != null) {
                // add next edge to path
                sortedEdges.Add(nextEdge);
                edges.Remove(nextEdge);
                lastVertex = nextEdge.v1 == lastVertex ? nextEdge.v2 : nextEdge.v1;
            } else {
                // no further edges found
                break;
            }
        }

        if (lastVertex == sortedEdges[0].v1 || lastVertex == sortedEdges[0].v2) {
            // edges form continous cycle
            return sortedEdges.ToArray();                    
        } else {
            // not a cycle
            return null;
        }
    }

    // Return a list of continuous cycles formed from given edges.
    public static List<Edge2[]> GetEdgePaths(List<Edge2> edges) {
        List<Edge2[]> paths = new List<Edge2[]>();

        // find cycles iteratively
        while (edges.Count > 0) {
            Edge2[] path = GetEdgePath(edges);
            if (path != null && path.Length > 1) {
                paths.Add(path);
            }
        }
        return paths.Count > 0 ? paths : null;
    }

}