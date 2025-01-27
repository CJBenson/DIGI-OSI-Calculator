using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLayout : MonoBehaviour
{
    //ordered from lowest order in nesting to highest
    [SerializeField] RectTransform[] layoutsToRebuild;
    private bool active = false;
    private int frameCount = 0;
    public void Activate()
    {
        active = true;
        frameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && frameCount >= 2)
        {
            foreach(RectTransform layout in layoutsToRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
            }
            
            active = false;
        }
        else if(active)
        {
            frameCount++;
        }
    }
}
