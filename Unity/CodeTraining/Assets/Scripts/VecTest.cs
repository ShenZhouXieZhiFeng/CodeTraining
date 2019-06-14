using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VecTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

	public float thera = 0;
	public float delta = 0.02f;
    // Update is called once per frame
    void Update()
    {
		// this.transform.forward = new Vector3(2,2,2);
		this.thera += this.delta;
		Vector3 v1 = this.transform.forward;
		float cos = Mathf.Cos(this.thera);
		float sin = Mathf.Sin(this.thera);
		float x = v1.x;
		float z = v1.z;
		v1.x = x * cos - z * sin;
		v1.z = z * cos + x * sin;
		this.transform.forward = v1;
    }

    private void OnDrawGizmos()
    {
        this.drawVec1();
    }

    void drawVec1()
    {
        Vector3 begin = this.transform.position;
        Vector3 end = this.transform.position;
        end.x += 10;
        end.y += 10;
        end.z += 10;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(begin, end);

        Vector3 v1 = end - begin;
        Vector3 v2 = new Vector3(-v1.x, v1.y, v1.z);
        v2 = v2.normalized;
        v2 = v2 * 10;
        Vector3 end2 = begin + v2;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(begin, end2);

        Vector3 v3 = new Vector3(v1.x, v1.y, -v1.z);
        v3 = v3.normalized;
        v3 = v3 * 10;
        Vector3 end3 = begin + v3;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(begin, end3);
    }
}
