using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramesPerSecond : MonoBehaviour
{
    string label2 = "A* Pathfinding Example Project";
    string label = "";
    
    float count;

    IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            if(Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                label = "FPS: " + (Mathf.Round(count));
                
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                label = "Pause";
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 140, 100, 25), label);
        GUI.Label(new Rect(5, 160, 200, 25), label2);
    }
}
