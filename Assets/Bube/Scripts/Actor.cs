using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Actor : MonoBehaviour
{
    [HideInInspector()] public Vector3 from;
    [HideInInspector()] public Vector3 to;

    public string name;

    public float showTime = 0.5f;
    public float hideTime = 0.3f;

    private Coroutine showingCoroutine;
    private Coroutine hidingCoroutine;

    private bool isShow = false;
    private bool isHide = true;

    private bool isShowing = false;
    private bool isHiding = false;

    public AudioClip audioClip;

    public void Show()
    {
        if (isShow) return;
        if (isHiding) StopCoroutine(hidingCoroutine);

        showingCoroutine = StartCoroutine(MoveCharacter(showTime, State.Show));
    }

    public void Hide()
    {
        if (isHide) return;
        if (isShowing) StopCoroutine(showingCoroutine);

        hidingCoroutine = StartCoroutine(MoveCharacter(hideTime, State.Hide));
    }

    private IEnumerator MoveCharacter(float time, State state )
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector3 fromPos = Vector3.zero, toPos = Vector3.zero;

        isHide = false;
        isShow = false;

        if (state == State.Show)
        {
            isShowing = true;
            fromPos = from;
            toPos = to;
        }
        else if (state == State.Hide)
        {
            isHiding = true;
            fromPos = to;
            toPos = from;
        }

        float dtime = 0;
        Vector3 currentPos = fromPos;
        while ((currentPos - toPos).magnitude > 0.01f)
        {
            currentPos = Vector3.Lerp(fromPos, toPos, dtime / time);
            rectTransform.localPosition = currentPos;
            dtime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (state == State.Show)
        {
            isShowing = false;
            isShow = true;
        }
        else if (state == State.Hide)
        {
            isHiding = false;
            isHide = true;
        }
    }

    private enum State
    {
        Show,
        Hide
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save postion from"))
        {
            Actor actor = (Actor)target;
            actor.from = actor.transform.localPosition;
        }


        if (GUILayout.Button("Save postion to"))
        {
            Actor actor = (Actor)target;
            actor.to = actor.transform.localPosition;
        }
    }
}
#endif