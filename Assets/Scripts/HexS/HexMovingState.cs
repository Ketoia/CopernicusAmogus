using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovingState : HexBaseState
{
    private HexStateManager item;
    private Vector2Int firstPosIndex;
    private Vector3 lastPosition;

    private List<Vector2Int> indexesToMoveForward;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        firstPosIndex = item.CurrentId;
        indexesToMoveForward = GetIndexesToMoveForward(item.CurrentId, item.World.MaxLayer);

        EventManager.StartAtomMoveEvent(item.CurrentId);
    }
    public override void UpdateState(HexStateManager item)
    {
        item.CurrentId = item.GetNearestPoint(indexesToMoveForward);

        Vector2 indexVec = item.V3ToV2(item.World.Hexs[item.CurrentId].HexPos) - item.V3ToV2(item.World.transform.position).normalized;

        Vector2 firstVec3 = item.V3ToV2(item.World.MousePosition);

        float firstLayerDist = item.V3ToV2(item.World.Hexs[indexesToMoveForward[0]].HexPos).magnitude;
        float LastLayerDist = item.V3ToV2(item.World.Hexs[indexesToMoveForward[indexesToMoveForward.Count - 1]].HexPos).magnitude;

        Vector2 test = indexVec.normalized * Mathf.Clamp(Vector2.Dot(firstVec3, indexVec.normalized), firstLayerDist, LastLayerDist);
       // Debug.Log("Vector3 mouse pos: " + firstVec3);
       // Debug.Log("Vector3 ball pos: " + indexVec);
      //  Debug.Log("Dot product: " + Vector2.Dot(firstVec3, indexVec));

        Vector2 pos = test; 

        item.transform.position = new Vector3(pos.x, 0, pos.y);

        if (Input.GetMouseButtonUp(0))
        {
            EventManager.StartAtomMoveEndEvent(item.CurrentId);
            item.SwitchState(item.idleState);
        }
    }

    private List<Vector2Int> GetIndexesToMoveForward(Vector2Int index, int layersAmount)
    {
        List<Vector2Int> newList = new List<Vector2Int>();
        float slice = index.y / index.x;
        for (int x = 0; x < layersAmount; x++)
        {
            newList.Add(new Vector2Int(x + 1, (int)(slice * x + slice)));
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
