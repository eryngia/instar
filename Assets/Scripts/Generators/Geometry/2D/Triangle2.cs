using System.Collections.Generic;
using UnityEngine;

// A 2D triangle.
public class Triangle2 {

    public Vector2 v1 {get;}
    public Vector2 v2 {get;}
    public Vector2 v3 {get;}

    public List<Edge2> edges {get;}

    public List<Vector2> vertices {get;}

    public Triangle2(Vector2 v1, Vector2 v2, Vector2 v3) {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;

        vertices = new List<Vector2> {v1, v2, v3};
        edges = new List<Edge2> {new Edge2(v1, v2), new Edge2(v2, v3), new Edge2(v3, v1)};
    }

    public Circle2 Circumcircle() {

        float d1 = Vector2.Distance(v2, v3);
        float d2 = Vector2.Distance(v1, v3);
        float d3 = Vector2.Distance(v1, v2);

        float area = 0.5f * Mathf.Abs(v1.x * (v2.y - v3.y) + v2.x * (v3.y - v1.y) + v3.x * (v1.y - v2.y));

        float D = 2 * (v1.x * (v2.y - v3.y) + v2.x * (v3.y - v1.y) + v3.x * (v1.y - v2.y));
        
        float cx = ((v1.x * v1.x + v1.y * v1.y) * (v2.y - v3.y) + (v2.x * v2.x + v2.y * v2.y) * (v3.y - v1.y) + (v3.x * v3.x + v3.y * v3.y) * (v1.y - v2.y)) / D;
        float cy = ((v1.x * v1.x + v1.y * v1.y) * (v3.x - v2.x) + (v2.x * v2.x + v2.y * v2.y) * (v1.x - v3.x) + (v3.x * v3.x + v3.y * v3.y) * (v2.x - v1.x)) / D;

        Vector2 c = new Vector2(cx, cy);

        float r = d1 * d2 * d3 / (4 * area);

        return new Circle2(c, r);
    }

    public bool SharesVertex(Triangle2 other) {
        return v1.Equals(other.v1)
            || v1.Equals(other.v2)
            || v1.Equals(other.v3)
            || v2.Equals(other.v1)
            || v2.Equals(other.v2)
            || v2.Equals(other.v3)
            || v3.Equals(other.v1)
            || v3.Equals(other.v2)
            || v3.Equals(other.v3);
    }

    public override bool Equals(object obj) {
        if (obj is Triangle2 other)
            return (v1 == other.v1 && v2 == other.v2 && v3 == other.v3) 
                || (v1 == other.v1 && v2 == other.v3 && v3 == other.v2) 
                || (v1 == other.v2 && v2 == other.v1 && v3 == other.v3) 
                || (v1 == other.v2 && v2 == other.v3 && v3 == other.v1) 
                || (v1 == other.v3 && v2 == other.v1 && v3 == other.v2) 
                || (v1 == other.v3 && v2 == other.v2 && v3 == other.v1);   
        else 
            return false;
    }

    public override int GetHashCode() {
        return v1.GetHashCode() ^ v2.GetHashCode() ^ v3.GetHashCode();
    }
}