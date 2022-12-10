using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRotatingState : HexBaseState
{
    private HexStateManager item;
    //private Vector2 lastPosIndex;
    private Vector3 lastPosition;

    private List<Vector2> indexesToRotate;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
       // lastPosIndex = item.CurrentId;
        indexesToRotate = GetIndexesToRotate(item.CurrentId);
    }
    public override void UpdateState(HexStateManager item)
    {
        item.CurrentId = item.GetNearestPoint(indexesToRotate);

        Vector2 pos = item.GetIntersectionPointCoordinates(item.V3ToV2(item.World.MousePosition), item.V3ToV2(item.World.transform.position), item.V3ToV2(item.World.Hexs[item.CurrentId].HexPos), GetNextNearestPoint(item.CurrentId));

        item.transform.position = new Vector3(pos.x, 0, pos.y);
        
 
        
        if (Input.GetMouseButtonUp(0))
        {
            item.SwitchState(item.idleState);
        }
    }

    //public Vector2 GetNearestPoint()
    //{
    //    Vector2 index = item.CurrentId;
    //    float diff = (item.World.MousePosition - item.World.Hexs[index].HexPos).magnitude;
    //    for (int i = 0; i < indexesToRotate.Count; i++)
    //    {
    //        float newDiff = (item.World.MousePosition - item.World.Hexs[indexesToRotate[i]].HexPos).magnitude;
    //        if (newDiff < diff)
    //        {
    //            diff = newDiff;
    //            index = indexesToRotate[i];
    //        }
    //    }
    //    return index;
    //}


    //public Vector2 V3ToV2(Vector3 vector)
    //{
    //    return new Vector2(vector.x, vector.z);
    //}

    public Vector2 GetNextNearestPoint(Vector2 index)
    {
        List<Vector2> indexes = GetNeighboursRightIndexs(index);

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
  

    //public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2/*, out bool found*/)
    //{
    //    float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

    //    if (tmp == 0)
    //    {
    //        // No solution!
    //        //found = false;
    //        return Vector2.zero;
    //    }

    //    float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

    //    //found = true;

    //    return new Vector2(
    //        B1.x + (B2.x - B1.x) * mu,
    //        B1.y + (B2.y - B1.y) * mu
    //    );
    //}
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
