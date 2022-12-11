using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenButtons
{
    public int layerSize = 4;

    public Vector2 CheckIndex(Vector2 mousePosition)
    {
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x);
        if (angle < 0) angle += Mathf.PI * 2;

        angle = -angle;
        angle += Mathf.PI * 2;

        int index = (int)((angle / (Mathf.PI * 2) * 6 * layerSize + 0.5f) % (6 * layerSize));

        return new Vector2(layerSize, index);
    }

    public Vector2 CheckDirection(Vector2 startPos, Vector2 mousePosition)
    {
        //Add some special direction blockers!

        Vector2 localPos = startPos - mousePosition;

        float angle = Mathf.Atan2(localPos.y, localPos.x);
        if (angle < 0) angle += Mathf.PI * 2;

        int directionIndex = (int)(((angle / (Mathf.PI * 2)) * 6 + 0.5f) % (6));

        float sqrt3by2 = Mathf.Sqrt(3) / 2;
        //Set up points
        Vector2[] points = new Vector2[6];
        points[0] = new Vector2(1, 0);
        points[1] = new Vector2(0.5f, sqrt3by2);
        points[2] = new Vector2(-0.5f, sqrt3by2);
        points[3] = new Vector2(-1, 0);
        points[4] = new Vector2(-0.5f, -sqrt3by2);
        points[5] = new Vector2(0.5f, -sqrt3by2);

        Debug.DrawLine(new Vector3(mousePosition.x, 0, mousePosition.y), new Vector3(startPos.x, 0, startPos.y));
        Debug.LogError(mousePosition + ", " + startPos);

        return points[directionIndex].normalized;
    }

    public List<Vector2Int> CheckOnDirection(Vector2 startPos, Vector2 direction, Dictionary<Vector2, HexInfo> hexagons)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        List<GameObject> SpanedGameobjects = new List<GameObject>();
        foreach (var item in hexagons.Keys)
        {
            Vector3 hexPos = hexagons[item].HexPos;
            Vector2 localDirection = startPos - new Vector2(hexPos.x, hexPos.z);
            localDirection = localDirection.normalized;
            float dotLength = Vector2.Dot(direction, localDirection);

            if (dotLength <= -0.995f)
            {
                Debug.Log("Good: " + item);
                Debug.DrawLine(new Vector3(startPos.x, 1, startPos.y), new Vector3(hexPos.x, 1, hexPos.z));
                positions.Add(new Vector2Int((int)item.x, (int)item.y));
                //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //obj.transform.position = hexPos;
            }
        }
        return positions;
    }
}
