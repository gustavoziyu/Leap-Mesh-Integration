﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeshByRotation : MonoBehaviour
{
    [Range(0.001f, Mathf.PI/2)]
    public float alpha = Mathf.PI/2; //step of revolution, should divide 2*PI exactly to better results
    public Vector3[] curve; //set of points that form the curve
    private List<Vector3> verticesList; //vertices of the mesh
    private Dictionary<Vector3, int> positionInVerticesList; //used to calculate the triangles indexes
    private int numberOfVertices = 0; //enumerate the vertices
    private List<int> triangles = new List<int>(); //triangles of the mesh
    private float previousAlpha;


    // Start is called before the first frame update
    void Start()
    {
        previousAlpha = alpha;
        positionInVerticesList = new Dictionary<Vector3, int>();
        verticesList = new List<Vector3>();
        curve = new Vector3[50];
        float x = 0;
        for(int i=0; i < 50; i++)
        {
            curve[i] = new Vector3(x, Mathf.Sin(x),0);
            x += 0.25f;
        }

        curve = new Vector3[] { new Vector3(-1, 2, 0), new Vector3(1, 1, 0), new Vector3(-1, -3, 0), new Vector3(-5, -3, 0), new Vector3(-6, -5, 0) };
        
        this.gameObject.GetComponent<MeshFilter>().mesh = createMesh();


    }

    // Update is called once per frame
    void Update()
    {
        if (alpha != previousAlpha)
        {
            previousAlpha = alpha;
            positionInVerticesList.Clear();
            verticesList.Clear();
            numberOfVertices = 0;
            triangles.Clear();
            this.gameObject.GetComponent<MeshFilter>().mesh = createMesh();
        }
    }

    //creates the mesh by rotating each pair of consecutive points.
    public Mesh createMesh()
    {
        Mesh m = new Mesh();

        for(int i=0; i + 1 < curve.Length; i++)
        {
            createMeshByPair(curve[i], curve[i+1]);

        }

        m.vertices = verticesList.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        m.RecalculateBounds();

        return m;

    }
    //calculates the vertices and triangles of the mesh generated by rotating the line between A and B
    //rotation fixed in y axis
    //we assume that the x component is posiive
    //we assume that the z component is zero
    private void createMeshByPair(Vector3 A, Vector3 B)
    {
        
        Vector3[] verticesQuad;

        float currentAlpha = 0;
        Vector3 currentA = A;
        Vector3 currentB = B;
        float radiusA = A.x;
        float radiusB = B.x;
        bool isTop = A == curve[0];
        bool isBottom = B == curve[curve.Length-1];

        while (currentAlpha < 2*Mathf.PI)
        {
            verticesQuad = createQuad(currentA, currentB, currentAlpha, radiusA, radiusB, isTop, isBottom);
            currentA = verticesQuad[0];
            currentB = verticesQuad[1];
            currentAlpha += alpha;
            int indexA_2 = positionInVerticesList[verticesQuad[0]];
            int indexB = positionInVerticesList[verticesQuad[2]];
            triangles.AddRange( new int[]{ indexA_2, positionInVerticesList[verticesQuad[1]], indexB, indexA_2, positionInVerticesList[verticesQuad[2]], positionInVerticesList[verticesQuad[3]] });   
        }
    }

    //creates the quad formed by the pair of points and the rotation points.
    //calculates the triangles when either A or B is a bound vertice.
    private Vector3[] createQuad(Vector3 A, Vector3 B, float currentAlpha, float radiusA, float radiusB, bool isTop, bool isBottom)
    {

        Vector3[] verticesList = new Vector3[4];
        Vector3 A_2 = new Vector3(Mathf.Cos(currentAlpha + alpha) * radiusA, A.y, Mathf.Sin(currentAlpha + alpha) * radiusA);

        Vector3 B_2 = new Vector3(Mathf.Cos(currentAlpha + alpha) * radiusB, B.y, Mathf.Sin(currentAlpha + alpha) * radiusB);
        verticesList[0] = A_2;
        verticesList[1] = B_2;
        verticesList[2] = B;
        verticesList[3] = A;

        foreach (Vector3 vertice in verticesList)
        {
            addVertice(vertice);
        }

        //add the top mesh
        if (isTop)
        {
            Vector3 center = new Vector3(0, A.y, 0);
            addVertice(center);

            triangles.AddRange(new int[] { positionInVerticesList[A], positionInVerticesList[center], positionInVerticesList[A_2] });
        }

        //add the bottom mesh
        if (isBottom)
        {
            Vector3 center = new Vector3(0, B.y, 0);
            addVertice(center);

            triangles.AddRange(new int[] { positionInVerticesList[B], positionInVerticesList[B_2], positionInVerticesList[center] });
        }



        return verticesList;

    }

    //add the vertice to the list if it's not already in there
    private void addVertice(Vector3 vertice)
    {
        if (!positionInVerticesList.ContainsKey(vertice))
        {
            positionInVerticesList[vertice] = numberOfVertices;
            numberOfVertices++;
            this.verticesList.Add(vertice);
        }
    }
}
