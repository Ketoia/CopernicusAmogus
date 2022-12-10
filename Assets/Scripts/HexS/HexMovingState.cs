using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovingState : HexBaseState
{
    private HexStateManager item;
    private Vector2 fristPosIndex;
    private Vector3 lastPosition;

    private List<Vector2> indexesToMoveForward;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        fristPosIndex = item.CurrentId;
        indexesToMoveForward = GetIndexesToMoveForward(item.CurrentId, item.World.MaxLayer);
        //for (int i = 0; i < indexesToMoveForward.Count; i++)
        //{
        //    Debug.Log(indexesToMoveForward[i]);
        //}
    }
    public override void UpdateState(HexStateManager item)
    {
        //item.transform.position = item.World.MousePosition;
        //
        //Vector2 mouseVec = item.V3ToV2(item.World.MousePosition) - item.V3ToV2(item.World.transform.position).normalized;
        item.CurrentId = item.GetNearestPoint(indexesToMoveForward);

        Vector2 indexVec = item.V3ToV2(item.World.Hexs[item.CurrentId].HexPos) - item.V3ToV2(item.World.transform.position).normalized;

        Vector2 firstVec = item.V3ToV2(item.World.transform.position);
        Vector2 firstVec2 = item.V3ToV2(item.World.Hexs[item.CurrentId].HexPos);

        Vector2 firstVec3 = item.V3ToV2(item.World.MousePosition);
        Vector2 perVector = Vector2.Perpendicular(indexVec) + item.V3ToV2(item.World.MousePosition);

        Vector2 firstLayerPos = item.V3ToV2(item.World.Hexs[indexesToMoveForward[0]].HexPos);
        Vector2 LastLayerPos = item.V3ToV2(item.World.Hexs[indexesToMoveForward[indexesToMoveForward.Count - 1]].HexPos);
        Vector2 pos = item.GetIntersectionPointCoordinates(firstVec,firstVec2,firstVec3,perVector);
        //float signX = Mathf.Sign(pos.x - firstLayerPos.x);
        //float signY = Mathf.Sign(pos.y - firstLayerPos.y);
        //if (IsVectorNegative(pos, firstLayerPos))
        //{
        //    pos = item.V3ToV2(item.World.Hexs[indexesToMoveForward[0]].HexPos);
        //}
        //else  if (IsVectorNegative(LastLayerPos, pos))
        //{
        //    pos = item.V3ToV2(item.World.Hexs[indexesToMoveForward[indexesToMoveForward.Count - 1]].HexPos);
        //}
       // float a = CalcMultiplayer(pos, Vector2.Dot(mouseVec, indexVec));
       // Debug.DrawRay(item.World.transform.position, new Vector3(a * pos.x, 0, a * pos.y), Color.green);
        item.transform.position = new Vector3(pos.x, 0, pos.y);

        if (Input.GetMouseButtonUp(0))
        {
            item.SwitchState(item.idleState);
        }
    }

    private bool IsVectorNegative(Vector2 vec1, Vector2 vec2)
    {
        float signX = Mathf.Sign(vec1.x - vec2.x);
        float signY = Mathf.Sign(vec1.y - vec2.y);

        if (signX < 0 && signY < 0)
            return true;
        else
            return false;
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
    public Vector2 GetNextNearestPoint(Vector2 index)
    {
        List<Vector2> indexes = GetNeighboursUpIndexs(index);
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

    private List<Vector2> GetNeighboursUpIndexs(Vector2 index)
    {
        List<Vector2> newList = new List<Vector2>();

        float pizzaSlices = index.y / index.x;
        if (index.x > 1)
            newList.Add(new Vector2(index.x - 1, index.y - pizzaSlices));

        if (index.x < item.World.MaxLayer)
            newList.Add(new Vector2(index.x + 1, index.y + pizzaSlices));
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
