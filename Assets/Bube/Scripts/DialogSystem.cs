using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public Image fadeout;
    public Image backgroundText;

    public Text textName;
    public Text textDialog;
    public float textWait = 1;
    public float textWaitBetween = 1;

    public Actor actor_1;
    public Actor actor_2;

    private bool switcher = false;
    private bool isFadeout = false;

    public Dialog dialogDebug;

    public void StartDialogDebug()
    {
        StartDialog(dialogDebug);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartDialog(dialogDebug);
    }

    public void StartDialog(Dialog dialog)
    {
        switcher = true;
        textName.text = "";
        textDialog.text = "";
        StartCoroutine(LerpFadeout(0, 1, 1, fadeout));
        StartCoroutine(LerpFadeout(0, 1, 1, backgroundText));
        StartCoroutine(ShowDialog(dialog));
    }

    public void StopAllDialog()
    {
        StopAllCoroutines();
    }

    private IEnumerator LerpFadeout(float start, float stop, float time, Image image)
    {
        float dTime = 0;
        Color color = image.color;
        while(dTime < time)
        {
            float alpha = Mathf.Lerp(start, stop, dTime / time);
            color.a = alpha;
            image.color = color;

            dTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        color.a = stop;
        image.color = color;

        isFadeout = true;
    }

    private IEnumerator ShowTextName(Actor actor)
    {
        textName.text = "";
        for (int i = 0; i < actor.name.Length; i++)
        {
            textName.text += actor.name[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < dialog.Dialogs.Count; i++)
        {
            if (switcher)
            {
                StartCoroutine(ShowTextName(actor_1));
                actor_1.Show();
                actor_2.Hide();
            }
            else 
            {
                StartCoroutine(ShowTextName(actor_2));
                actor_1.Hide();
                actor_2.Show();
            }

            textDialog.text = "";
            yield return new WaitForSeconds(0.5f); //Wait for player to come
            for (int j = 0; j < dialog.Dialogs[i].Length; j++)
            {
                textDialog.text += dialog.Dialogs[i][j];
                yield return new WaitForSeconds(textWait);
            }

            switcher = !switcher;
            yield return new WaitForSeconds(textWaitBetween);
            
        }

        actor_1.Hide();
        actor_2.Hide();
        textDialog.text = "";
        textName.text = "";

        StartCoroutine(LerpFadeout(1, 0, 1, fadeout));
        StartCoroutine(LerpFadeout(1, 0, 1, backgroundText));
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(DialogSystem))]
public class DialogSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Debug start dialog"))
        {
            DialogSystem dialogSystem = (DialogSystem)target;
            dialogSystem.StartDialogDebug();
        }

        if (GUILayout.Button("Debug stop all dialog"))
        {
            DialogSystem dialogSystem = (DialogSystem)target;
            dialogSystem.StopAllDialog();
        }
    }
}

#endif