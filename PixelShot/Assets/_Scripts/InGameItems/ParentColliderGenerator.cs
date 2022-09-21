using System.Collections.Generic;
using UnityEngine;

public class ColliderGenerator
{
    private static Vector3 colliderCenter;
    private static Vector3 colliderSize;

    public static void GenerateColliderX(int numChild, float anchor, float scale)
    {
        colliderCenter = new Vector3(((float)numChild/ 2 - anchor) * scale, 0.04f, 0);
        colliderSize = new Vector3(numChild * scale, scale * 1.5f, scale);
    }

    public static void GenerateColliderZ(int numChild, float anchor, float scale)
    {
        colliderCenter = new Vector3(0, 0.04f, - ((float)numChild / 2 - anchor) * scale);
        colliderSize = new Vector3(scale, scale * 1.5f, numChild * scale);
    }

    public static void SetBoxCollider(BoxCollider collider)
    {
        collider.center = colliderCenter;
        collider.size = colliderSize;
    }

    public static Vector3 ColliderCenter
    {
        get { return colliderCenter;  }
    }

    public static Vector3 ColliderSize
    {
        get { return colliderSize; }
    }


}
