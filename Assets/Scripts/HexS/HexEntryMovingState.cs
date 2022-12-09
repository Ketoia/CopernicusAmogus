using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEntryMovingState : HexBaseState
{
    private HexStateManager item;
    private List<Vector2> neighboursUp;
    private List<Vector2> neighboursRight;

    private bool rotate;
    private bool move;

    public override void EnterState(HexStateManager item)
    {
        rotate = false; move = false;
        this.item = item;

        if (item.AllFlexibleIndex.Contains(item.LastId))
        {
            Debug.Log("Yess");
            neighboursUp = GetNeighboursUpIndexs(item.LastId);
            neighboursRight = GetNeighboursRightIndexs(item.LastId);
           
        }
        else
        {
            Debug.Log("Noo");
            item.SwitchState(item.rotatingState);
        }
        //Debug.Log(AllIndexesToMoveForward(4, 6).Count);
    }
    public override void UpdateState(HexStateManager item)
    {
        
        item.transform.position = item.World.MousePosition;
        if (IsItCloser(item.LastId))
        {
            Debug.Log("Move " + move + " rot " + rotate);
            if (move)
                item.SwitchState(item.movingState);
            else if (rotate)
                item.SwitchState(item.rotatingState);
        }
    }

    private bool IsItCloser(Vector2 index)
    {
        float minDiff = (item.World.MousePosition - item.World.Hexs[index].HexPos).magnitude;
        for (int i = 0; i < neighboursUp.Count; i++)
        {
            if ((item.World.MousePosition - item.World.Hexs[neighboursUp[i]].HexPos).magnitude < minDiff)
            {
                move = true;
                return true;
            }
        }
        for (int i = 0; i < neighboursRight.Count; i++)
        {
            if ((item.World.MousePosition - item.World.Hexs[neighboursRight[i]].HexPos).magnitude < minDiff)
            {
                rotate = true;
                return true;
            }
        }
        return false;
    }

    private List<Vector2> GetNeighboursUpIndexs(Vector2 index)
    {
        List<Vector2> newList = new List<Vector2>();

        float pizzaSlices = index.y / index.x;
        //index.x > 1 ? newList.Add(new Vector2(index.x - 1, index.y - pizzaSlices)) : null ;
        if (index.x > 1)
            newList.Add(new Vector2(index.x - 1, index.y - pizzaSlices));

        if (index.x < item.World.MaxLayer)
            newList.Add(new Vector2(index.x + 1, index.y + pizzaSlices));
        //for (int i = 0; i < newList.Count; i++)
        //{
        //    Debug.Log(newList[i]);
        //}
        return newList;
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
        for (int i = 0; i < newList.Count; i++)
        {
            Debug.Log(newList[i]);
        }
        return newList;
    }

    private void SwitchState()
    {
        if (item.AllFlexibleIndex.Contains(item.LastId))
        {

        }
        else
        {
            item.SwitchState(item.rotatingState);
        }
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
