using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour
{
    [Serializable]
    public class Point
    {
        public Color color;
        public Vector3 offset;
    }
    public Point[] points;
    public int frequency = 1;

    private Mesh mesh = new Mesh();
    private Vector3[] vertices;
    private Color[] colors;
    public Color centerColor;
    private int[] triangles;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Star Mesh";

        if (frequency < 2)
        {
            frequency = 2;
        }
        if (points == null || points.Length == 0)
        {
            points = new Point[]{ new Point()};
        }

        int numberOfPoints = frequency * points.Length;
        vertices = new Vector3[numberOfPoints + 1];
        colors = new Color[numberOfPoints + 1];
        triangles = new int[numberOfPoints * 3];
        float angle = -360f / numberOfPoints;
        colors[0] = centerColor;
        for (int iF = 0, v = 1, t = 1; iF < frequency; iF++)
        {
            for (int iP = 0; iP < points.Length; iP += 1, v += 1, t += 3)
            {
                vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * points[iP].offset;
                colors[v] = points[iP].color;
                triangles[t] = v;
                triangles[t + 1] = v + 1;
            }
        }
        triangles[triangles.Length - 1] = 1;

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
    }
}