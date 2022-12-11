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
    public List<Vector4> AdditionalLayers;
    private int counter = 0;

    private void Awake()
    {
        instance = this;
        //GenerateNewMission();
    }

    
    public List<int> GenerateNewMission()
    {
        List<int> quests = new List<int>();
        text.text = "";

        if(counter < AdditionalLayers.Count)
        {
            quests.Add((int)AdditionalLayers[counter].x);
            text.text += "Set: " + quests[0] + " atoms on " + (1).ToString() + " layer \n";
            quests.Add((int)AdditionalLayers[counter].y);
            text.text += "Set: " + quests[1] + " atoms on " + (2).ToString() + " layer \n";
            quests.Add((int)AdditionalLayers[counter].z);
            text.text += "Set: " + quests[2] + " atoms on " + (3).ToString() + " layer \n";
            quests.Add((int)AdditionalLayers[counter].w);
            text.text += "Set: " + quests[3] + " atoms on " + (4).ToString() + " layer \n";
        }
        else
        {
            for (int i = 0; i < layers; i++)
            {
                int max = (i + 1);
                int pizza = (int)Random.Range(1, 6 * max - 1);
                //pizza = (int)Mathf.Lerp(0, max, currentLvl / maxLvls) + 1;
                pizza = Mathf.Clamp(pizza, 1, 6 * max - 1);
                text.text += "Set: " + pizza.ToString() + " atoms on " + (i + 1).ToString() + " layer \n";

                quests.Add(pizza);
            }
        }
        counter++;

        return quests;
    }

    public void FinishQuest()
    {
        dialogSystem.NextDialog();
    }
}
