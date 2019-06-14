using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBezie : MonoBehaviour
{

    public Transform p0;
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

    float delta = 0.1f;
    private void OnDrawGizmos()
    {
        this.drawBezie2();
    }

    void drawBezie2()
    {
        int count = this.transform.childCount;

        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        Transform first = this.transform.GetChild(0);
        if (first == null)
        {
            return;
        }
        Vector3 begin = first.position;

        for (int i = 0; i < count; i++)
        {
            if (i <= 0)
            {
                p0 = this.transform.GetChild(count - 1).position;
            }
            else
            {
                p0 = this.transform.GetChild(i - 1).position;
            }
            p1 = this.transform.GetChild(i).position;
            p2 = this.transform.GetChild((i + 1) % count).position;
            p3 = this.transform.GetChild((i + 2) % count).position;

            float delta = 0;
            while (delta <= 1f)
            {
                delta += 0.01f;
                Gizmos.color = Color.white;
                Vector3 end = this.calculatePoint(p0, p1, p2, p3, delta);
                Gizmos.DrawLine(begin, end);
                begin = end;
            }
        }
    }

    void drawBezie()
    {
        if (this.p0 == null) return;
        Vector3 p0 = this.p0.position;
        Vector3 p1 = this.p1.position;
        Vector3 p2 = this.p2.position;
        // Debug.LogWarning(p0);
        Vector3 begin = p0;
        float delta = 0;
        Gizmos.color = Color.white;
        while (delta <= 1f)
        {
            delta += this.delta;
            Vector3 t1 = Vector3.Lerp(p0, p1, delta);
            Vector3 t2 = Vector3.Lerp(p1, p2, delta);
            Vector3 end = Vector3.Lerp(t1, t2, delta);
            Gizmos.DrawLine(begin, end);
            begin = end;
        }
    }

    Vector3 calculatePoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return p1 + (0.5f * (p2 - p0) * t) + 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                0.5f * (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t;
    }
}
