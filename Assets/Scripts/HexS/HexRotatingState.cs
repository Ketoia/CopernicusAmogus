using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexRotatingState : HexBaseState
{
    public float getDeltaAngle => startAngle - currentAngle; 
    public Vector2Int startId;
    public HexStateManager item;

    //private Vector2 lastPosIndex;
    private float startAngle = 0;
    private float currentAngle = 0;
    private List<Vector2Int> indexesToRotate;

    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        startId = item.CurrentId;
        // lastPosIndex = item.CurrentId;
        startAngle = GetAngle(new Vector2(item.transform.position.x, item.transform.position.z));
        indexesToRotate = item.GetIndexesToRotate(item.CurrentId);
        EventManager.StartAtomRotationEvent(item.CurrentId.x, item.CurrentId.y, item.rotatingState);
        //Debug.Log("Start " + item.CurrentId);

        for (int i = indexesToRotate.Count; i >= 0; i--)
        {
            if (indexesToRotate[i] == null)
                indexesToRotate.RemoveAt(i);
        }
    }


    public override void UpdateState(HexStateManager item)
    {
        item.CurrentId = item.GetNearestPoint(indexesToRotate);

        float scale = 1.5f * item.CurrentId.x;
        currentAngle = GetAngle(item.V3ToV2(item.World.MousePosition));
        Vector2 tempPos = RotateByAngle(currentAngle, scale);

        //item.transform.position = new Vector3(pos.x, 0, pos.y);
        item.transform.position = new Vector3(tempPos.x, 0, tempPos.y);

        if (Input.GetMouseButtonUp(0))
        {
            EventManager.StartAtomRotationEndEvent(item.CurrentId.y);
            //Debug.Log("Koniec " + item.CurrentId);
            item.SwitchState(item.idleState);
        }
    }

    private Vector2 RotateByAngle(float angle, float scale = 1)
    {
        float sqrt3by2 = Mathf.Sqrt(3) / 2;
        //Set up points
        Vector2[] points = new Vector2[6];
        points[0] = new Vector2(1, 0);
        points[1] = new Vector2(0.5f, sqrt3by2);
        points[2] = new Vector2(-0.5f, sqrt3by2);
        points[3] = new Vector2(-1, 0);
        points[4] = new Vector2(-0.5f, -sqrt3by2);
        points[5] = new Vector2(0.5f, -sqrt3by2);

        for (int i = 0; i < points.Length; i++)
        {
            points[i] *= scale;
        }

        //Get index by angle
        int index = (int)(angle / (Mathf.PI * 2) * 6);

        float time = (angle % (Mathf.PI / 3)) / (Mathf.PI / 3);

        Vector2 posOut = Vector2.Lerp(points[index], points[(index + 1) % 6], time);
        return posOut;
    }

    private float GetAngle(Vector2 mousePosition)
    {
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x);
        if (angle < 0) angle += 2 * Mathf.PI;
        return angle;
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

    public Vector2 GetNextNearestPoint(Vector2Int index)
    {
        List<Vector2Int> indexes = GetNeighboursRightIndexs(index);

        if ((item.World.Hexs[indexes[0]].HexPos - item.World.MousePosition).magnitude <= (item.World.Hexs[indexes[1]].HexPos - item.World.MousePosition).magnitude)
        {
            return new Vector2(item.World.Hexs[indexes[0]].HexPos.x, item.World.Hexs[indexes[0]].HexPos.z);
        }
        else
        {
            return new Vector2(item.World.Hexs[indexes[1]].HexPos.x, item.World.Hexs[indexes[1]].HexPos.z);
        }
    }

    private List<Vector2Int> GetNeighboursRightIndexs(Vector2Int index)
    {
        List<Vector2Int> newList = new List<Vector2Int>();

        if (index.y > 0)
            newList.Add(new Vector2Int(index.x, index.y - 1));
        else
            newList.Add(new Vector2Int(index.x, (item.World.MaxPizzaSlices - 1) * index.x + index.x - 1));

        if (index.y < (item.World.MaxPizzaSlices - 1) * index.x + index.x - 1)
            newList.Add(new Vector2Int(index.x, index.y + 1));
        else
            newList.Add(new Vector2Int(index.x, 0));

        return newList;
    }

    //private List<Vector2> GetIndexesToRotate(Vector2 index)
    //{
    //    List<Vector2> newList = new List<Vector2>();
    //    //float slice = index.y / index.x;
    //    for (int x = 0; x < index.x * item.World.MaxPizzaSlices; x++)
    //    {
    //        newList.Add(new Vector2(index.x, x));
    //    }

    //    return newList;
    //}
  

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
