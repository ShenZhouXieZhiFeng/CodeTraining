using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    public float amongTime = 0;
    // Update is called once per frame
    void Update()
    {
        // this.moveCircle();
        // this.moveOval();
    }

	void moveDrift()
	{

	}

    void moveOval()
    {
        float a = 20f;
        float b = 30f;
        float y = this.transform.position.y;

        this.amongTime += Time.deltaTime;

        float x = a * Mathf.Cos(this.amongTime);
        float z = b * Mathf.Sin(this.amongTime);

		Vector3 target = new Vector3(x, this.transform.position.y, z);
		this.transform.LookAt(target);
        this.transform.position = target;
    }

    void moveCircle()
    {
        float r = 10f;
        this.amongTime += Time.deltaTime;
        float x = r * Mathf.Cos(this.amongTime);
        float z = r * Mathf.Sin(this.amongTime);
        this.transform.position = new Vector3(x, this.transform.position.y, z);
    }
}
