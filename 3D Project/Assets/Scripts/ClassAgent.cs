using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public abstract Vector3 Seek(Vector3 target);

    public abstract Vector3 Arrive(Vector3 target, float radius);

    public abstract Vector3 CalcSteering();

}
