using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // this.drawCircleFunc();
        this.drawOvalFunc();
    }

    void drawOvalFunc()
    {
        float a = 20f;
        float b = 30f;
        float y = this.transform.position.y;
        Vector3 begin = Vector3.zero;
		begin.y = y;
        for (float the = 0; the < 2 * Mathf.PI; the += 0.001f)
        {
            float x = a * Mathf.Cos(the);
            float z = b * Mathf.Sin(the);
			Vector3 end = new Vector3(x, y, z);
			if(the == 0)
			{
				begin = end;
				continue;
			}
            Gizmos.DrawLine(begin, end);
            begin = end;
        }
    }

    void drawCircleFunc()
    {
        Vector3 begin = Vector3.zero;
        Vector3 first = Vector3.zero;
        float r = 10f;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.01f)
        {
            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);
            Vector3 end = new Vector3(x, this.transform.position.y, z);
            if (theta == 0)
            {
                first = end;
            }
            else
            {
                Gizmos.DrawLine(begin, end);
            }
            begin = end;
        }

        Gizmos.DrawLine(first, begin);
    }
}
