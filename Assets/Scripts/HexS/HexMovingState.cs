using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovingState : HexBaseState
{
    private HexStateManager item;
    private Vector2 lastPosIndex;
    private Vector3 lastPosition;

    private List<Vector2> indexesToMoveForward;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        indexesToMoveForward = GetIndexesToMoveForward(item.LastId, item.World.MaxLayer);
        //for (int i = 0; i < indexesToMoveForward.Count; i++)
        //{
        //    Debug.Log(indexesToMoveForward[i]);
        //}
    }
    public override void UpdateState(HexStateManager item)
    {
        item.transform.position = item.World.MousePosition;

        if (Input.GetMouseButtonUp(0) || FindNearestPoint(1.5f))
        {
            item.SwitchState(item.idleState);
        }
    }

    private bool FindNearestPoint(float maxDistance)
    {
        for (int i = 0; i < indexesToMoveForward.Count; i++)
        {
            //Debug.Log(indexesToMoveForward[i]);
            if ((item.World.MousePosition - item.World.Hexs[indexesToMoveForward[i]].HexPos).magnitude < maxDistance)
            {
               // Debug.Log(indexesToMoveForward[i]);
                item.LastId = indexesToMoveForward[i];
                return false;
            }
        }
        return true;
    }
    private List<Vector2> GetIndexesToMoveForward(Vector2 index, int layersAmount)
    {
        List<Vector2> newList = new List<Vector2>();
        float slice = index.y / index.x;
        for (int x = 0; x < layersAmount; x++)
        {
            newList.Add(new Vector2(x + 1, slice * x + slice));
        }

        return newList;
    }

    public override void FixedUpdateState(HexStateManager item)
    {

    }
    public override void ExitState(HexStateManager item)
    {

        //Debug.Log(item.LastId);
    }
    public override void OnCollisionEnter(HexStateManager item, Collider other)
    {

    }
    public override void OnCollisionExit(HexStateManager item, Collider other)
    {

    }
}
