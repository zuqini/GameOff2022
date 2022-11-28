using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Utils
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z = 0)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static Vector3 ClipMousePosToScreen(Vector3 mousePos)
    {
        return new Vector3(Mathf.Min(Mathf.Max(mousePos.x, 0), Screen.width),
                Mathf.Min(Mathf.Max(mousePos.y, 0), Screen.height),
                mousePos.z);
    }

    public static void SetColliderEnabledRecursive(GameObject item, bool value)
    {
        var colChildren = item.GetComponentsInChildren<Collider2D>();
        foreach (var col in colChildren)
        {
            col.enabled = value;
        }
    }

    public static void SetSortingLayerRecursive(GameObject item, int sortingLayerID)
    {
            var rendChildren = item.GetComponentsInChildren<Renderer>();
            foreach (var rend in rendChildren)
            {
                rend.sortingLayerID = sortingLayerID;
            }
    }
}
