using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    int layers = 4;
    int maxLvls = 34;

    public static MissionManager instance;
    public UnityEngine.UI.Text text;
    public DialogSystem dialogSystem;

    private void Awake()
    {
        instance = this;
        //GenerateNewMission();
    }

    
    public List<int> GenerateNewMission()
    {
        List<int> quests = new List<int>();
        text.text = "";
        for (int i = 0; i < layers; i++)
        {
            int max = (i + 1);
            int pizza = (int)Random.Range(1, 6 * max - 1);
            //pizza = (int)Mathf.Lerp(0, max, currentLvl / maxLvls) + 1;
            pizza = Mathf.Clamp(pizza, 1, 6 * max - 1);
            text.text += "Set: " + pizza.ToString() + " atoms on " + (i + 1).ToString() + " layer \n";

            quests.Add(pizza); 
        }

        return quests;
    }

    public void FinishQuest()
    {
        dialogSystem.NextDialog();
    }
}
