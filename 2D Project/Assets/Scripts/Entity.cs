using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float attraction = 1;
    public float effectdistance = 1;
    public GameObject parent;
}