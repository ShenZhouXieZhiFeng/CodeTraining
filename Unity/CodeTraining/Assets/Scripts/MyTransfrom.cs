using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransfrom : MonoBehaviour
{
    public Matrix4x4 matrix;

    public Vector4 v;

    // Use this for initialization
    void Start()
    {
        // this.matrix.SetTRS(transform.position, transform.rotation, transform.localScale);
		this.translate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void translate()
    {
        v = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);

        matrix = Matrix4x4.identity;

        matrix.m03 = 3;
        matrix.m13 = 4;
        matrix.m23 = 5;

        v = matrix * v;

        transform.position = new Vector3(v.x, v.y, v.z);
    }
}
