using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlatform : MonoBehaviour, IPlatform
{
    public Vector3 velocity()
    {
        return Vector3.zero;
    }
}
