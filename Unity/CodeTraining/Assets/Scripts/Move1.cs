using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("updatePos", 0, 0.05f);
    }

    void updatePos()
    {
        float dt = (float)Time.time;
        float posX = Mathf.Sin(dt);
        float posY = Mathf.Cos(dt);
        this.transform.position = new Vector3(posX, posY, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(this.transform.position, 10);
        // Gizmos.DrawLine()
        
    }
}
