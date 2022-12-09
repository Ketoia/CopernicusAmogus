using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRotatingState : HexBaseState
{
    private HexStateManager item;
    private Vector2 lastPosIndex;
    private Vector3 lastPosition;

    private List<Vector2> indexesToRotate;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        lastPosIndex = item.LastId;
        indexesToRotate = GetIndexesToRotate(item.LastId);
    }
    public override void UpdateState(HexStateManager item)
    {

        //if ((item.World.MousePosition - item.World.transform.position).magnitude < 0.05f)
        //{
        //    return;
        //}
        SetNearestPoint(1.5f, 0.5f);
        Vector2 pos = GetIntersectionPointCoordinates(V3ToV2(item.World.MousePosition), V3ToV2(item.World.transform.position), V3ToV2(item.World.Hexs[item.LastId].HexPos), GetNextNearestPoint(item.LastId));
        //Debug.DrawRay(Vector3.zero, item.World.MousePosition - item.World.transform.position * 10f, Color.red);
        //Debug.DrawRay(item.World.Hexs[item.LastId].HexPos, item.World.Hexs[item.LastId].HexPos - new Vector3(GetNextNearestPoint(item.LastId).x,0, GetNextNearestPoint(item.LastId).y) * 10f, Color.red);
        //float diff = (new Vector3(pos.x,0,pos.y) - item.transform.position).magnitude;

        item.transform.position = new Vector3(pos.x, 0, pos.y);
        
        //else
        //{
        //    //Debug.Log(item.World.Hexs[item.LastId].HexPos);
        //    item.transform.position = new Vector3(item.World.Hexs[item.LastId].HexPos.x, 0, item.World.Hexs[item.LastId].HexPos.z);
        //}
        
        if (Input.GetMouseButtonUp(0) /*|| FindNearestPoint(1.5f)*/)
        {
            item.SwitchState(item.idleState);
        }
    }
    public void SetNearestPoint(float maxDistance, float coolFactor)
    {
        for (int i = 0; i < indexesToRotate.Count; i++)
        {
            //Debug.Log(indexesToMoveForward[i]);
            if ((item.transform.position - item.World.Hexs[indexesToRotate[i]].HexPos).magnitude < maxDistance)
            {
                item.LastId = indexesToRotate[i];

            }
        }
    }



    public bool FindNearestPoint(float maxDistance)
    {
        
        for (int i = 0; i < indexesToRotate.Count; i++)
        {
            //Debug.Log(indexesToMoveForward[i]);
            if ((item.World.MousePosition - item.World.Hexs[indexesToRotate[i]].HexPos).magnitude < maxDistance)
            {
                // Debug.Log(indexesToMoveForward[i]);
                item.LastId = indexesToRotate[i];
                return false;
            }
        }
        return true;
        //foreach (var hex in item.World.Hexs)
        //{
        //    if (hex.Key.x == layer)
        //    {
        //        //Debug.Log((item.World.MousePosition - hex.Value.HexPos).magnitude);
        //        if ((item.World.MousePosition - hex.Value.HexPos).magnitude < maxDistance)
        //        {
        //            //Debug.Log(item.LastId);
        //            item.LastId = hex.Key;
        //            return false;
        //        }
        //    }
        //}
        //return true;
    }

    public Vector2 V3ToV2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public Vector2 GetNextNearestPoint(Vector2 index)
    {
        List<Vector2> indexes = GetNeighboursRightIndexs(index);
        //if (true)
        //{

        //}
        if ((item.World.Hexs[indexes[0]].HexPos - item.World.MousePosition).magnitude <= (item.World.Hexs[indexes[1]].HexPos - item.World.MousePosition).magnitude)
        {
            return new Vector2(item.World.Hexs[indexes[0]].HexPos.x, item.World.Hexs[indexes[0]].HexPos.z);
        }
        else
        {
            return new Vector2(item.World.Hexs[indexes[1]].HexPos.x, item.World.Hexs[indexes[1]].HexPos.z);
        }
    }

    private List<Vector2> GetNeighboursRightIndexs(Vector2 index)
    {
        List<Vector2> newList = new List<Vector2>();

        if (index.y > 0)
            newList.Add(new Vector2(index.x, index.y - 1));
        else
            newList.Add(new Vector2(index.x, (item.World.MaxPizzaSlices - 1) * index.x + index.x - 1));

        if (index.y < (item.World.MaxPizzaSlices - 1) * index.x + index.x - 1)
            newList.Add(new Vector2(index.x, index.y + 1));
        else
            newList.Add(new Vector2(index.x, 0));

        return newList;
    }

    private List<Vector2> GetIndexesToRotate(Vector2 index)
    {
        List<Vector2> newList = new List<Vector2>();
        float slice = index.y / index.x;
        for (int x = 0; x < index.x * item.World.MaxPizzaSlices; x++)
        {
            newList.Add(new Vector2(index.x, x));
        }

        return newList;
    }
    //Vector3 intersect(Line& line1, Line& line2)
    //{
    //    Vector3 p = line1.point;           // P1
    //    Vector3 v = line1.direction;       // v
    //    Vector3 q = line2.point;           // Q1
    //    Vector3 u = line2.direction;       // u

    //    // find a = v x u
    //    Vector3 a = v.cross(u);            // cross product

    //    // find dot product = (v x u) . (v x u)
    //    float dot = a.dot(a);              // inner product

    //    // if v and u are parallel (v x u = 0), then no intersection, return NaN point
    //    if (dot == 0)
    //        return Vector3(NAN, NAN, NAN);

    //    // find b = (Q1-P1) x u
    //    Vector3 b = (q - p).cross(u);      // cross product

    //    // find t = (b.a)/(a.a) = ((Q1-P1) x u) .(v x u) / (v x u) . (v x u)
    //    float t = b.dot(a) / dot;

    //    // find intersection point
    //    Vector3 point = p + (t * v);       // substitute t to line1
    //    return point;
    //}

    public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2/*, out bool found*/)
    {
        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

        if (tmp == 0)
        {
            // No solution!
            //found = false;
            return Vector2.zero;
        }

        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

        //found = true;

        return new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );
    }
    public override void FixedUpdateState(HexStateManager item)
    {

    }
    public override void ExitState(HexStateManager item)
    {

    }
    public override void OnCollisionEnter(HexStateManager item, Collider other)
    {

    }
    public override void OnCollisionExit(HexStateManager item, Collider other)
    {

    }
}
