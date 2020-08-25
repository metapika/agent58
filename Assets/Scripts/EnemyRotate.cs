using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyRotate : MonoBehaviour
{
    public AIPath aIPath;
    public Rigidbody2D rb;

    void Update()
    {
        if(aIPath.desiredVelocity.x >= 0.01f)
        {
            transform.eulerAngles = Vector3.up * 180;
        }
        else if (aIPath.desiredVelocity.x <= -0.01f)
        {
            transform.eulerAngles = Vector3.up;
        }
    }
}
