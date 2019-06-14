using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    Mesh mesh;
    public Vector3[] vertices;
    public int[] triangle;

    // Use this for initialization
    void Start()
    {
		this.mesh = new Mesh();
		mesh.vertices = this.vertices;
		mesh.triangles = this.triangle;

		GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
