using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFunctions
{
    static public  Vector3 XZPlane(Vector3 _vector)
    {
        return new Vector3(_vector.x, 0, _vector.z);
    }
}
