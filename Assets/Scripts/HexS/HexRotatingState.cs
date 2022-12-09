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
        indexesToRotate = GetIndexesToRotate(item.LastId);
    }
    public override void UpdateState(HexStateManager item)
    {
        item.transform.position = item.World.MousePosition;

        if (Input.GetMouseButtonUp(0) || FindNearestPoint(1.5f))
        {
            item.SwitchState(item.idleState);
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
