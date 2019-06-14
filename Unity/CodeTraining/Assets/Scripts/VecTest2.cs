using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("test")]
public class VecTest2 : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    public Transform p3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("test")]
    void test()
    {
        Vector3 p1 = this.p1.position;
        Vector3 p2 = this.p2.position;
        Vector3 p3 = this.p3.position;

        Vector3 v1 = p2 - p1;
        Vector3 v2 = p3 - p1;

        float dot = Vector3.Dot(v1, v2);
        Debug.Log(dot);
    }

    private void OnDrawGizmos()
    {
        Vector3 p1 = this.p1.position;
        Vector3 p2 = this.p2.position;
        Vector3 p3 = this.p3.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p1, p3);
    }
}
