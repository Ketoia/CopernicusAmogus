using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AtomRotationState : AtomBaseState
{
    public float startAngle;

    private AtomStateManager item;

    public override void EnterState(AtomStateManager item)
    {
        this.item = item;
        startAngle = GetAngle(new Vector2(item.transform.position.x, item.transform.position.z));
        //if (startAngle < 0) startAngle += 2 * Mathf.PI;
        Debug.Log("Rotation enter");
        EventManager.AtomRotationEndEvent += SwitchState;
    }

    private void SwitchState(int hexIndexY)
    {
        Debug.Log("Rotation enter2");

        item.UpdateCurrentRotId(hexIndexY);
        item.SwitchState(item.idleState);
    }

    public override void UpdateState(AtomStateManager item)
    {
        float currentAngle;
        currentAngle = startAngle - item.hexRotatingState.getDeltaAngle;

        float scale = 1.5f * item.CurrentId.x;
        Vector2 pos = RotateByAngle(currentAngle, scale);
        item.transform.position = new Vector3(pos.x, 0, pos.y);
    }

    public override void FixedUpdateState(AtomStateManager item)
    {

    }
 
    public override void OnCollisionEnter(AtomStateManager item, Collider other)
    {

    }
    public override void OnCollisionExit(AtomStateManager item, Collider other)
    {

    }
    public override void ExitState(AtomStateManager item)
    {
        EventManager.AtomRotationEndEvent -= SwitchState;
    }

    private float GetAngle(Vector2 mousePosition)
    {
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x);
        if (angle < 0) angle += 2 * Mathf.PI;
        return angle;
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
        if (angle < 0) angle += Mathf.PI * 2;
        angle = angle % (Mathf.PI * 2);
        int index = (int)(angle / (Mathf.PI * 2) * 6);

        float time = (angle % (Mathf.PI / 3)) / (Mathf.PI / 3);
        if (index < 0 || index > 5) Debug.LogError(index);
        Vector2 posOut = Vector2.Lerp(points[index], points[(index + 1) % 6], time);
        return posOut;
    }

}
