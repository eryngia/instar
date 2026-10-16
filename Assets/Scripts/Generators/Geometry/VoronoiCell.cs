using UnityEngine;

// A Voronoi cell with site and edges.
public class VoronoiCell {

    public Vector2 site {get;}
    public Edge2[] edges {get;}
    public Vector2[] vertices {get;}

    public VoronoiCell(Vector2 site, Edge2[] edges) {
        this.site = site;
        this.edges = edges;
        
        vertices = Edge2.GetVertexPath(edges);
    }

}