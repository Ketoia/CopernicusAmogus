using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    int layers = 4;
    int maxLvls = 34;

    public List<int> GenerateNewMission(int currentLvl)
    {
        List<int> quests = new List<int>();
        for (int i = 0; i < layers; i++)
        {
            int max = (i + 1);
            int pizza = (int)Random.Range(1, 6 * max - 1);
            //pizza = (int)Mathf.Lerp(0, max, currentLvl / maxLvls) + 1;
            pizza = Mathf.Clamp(pizza, 1, 6 * max - 1);
            quests.Add(pizza); 
        }

        return quests;
    }
}
