using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightGlowHandler : MonoBehaviour {

    #region Singleton
    //Singleton pattern implementation.
    private static HighlightGlowHandler _instance;

    public static HighlightGlowHandler Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private Outline myOutline;
    private Hashtable ht = iTween.Hash("from", 0, "to", 1, "time", 1, "looptype", "pingpong", "onupdate", "HighlightPulse");

    private void OnEnable()
    {
        InteractiveIcon.OnIconClicked += HighlightUpdate;
    }
    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= HighlightUpdate;
    }

    public void StartGlowing()
    {
        Invoke("Glow", .5f);
    }

    private void Glow()
    {
        myOutline = LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID].outlines[0];

        iTween.ValueTo(gameObject, ht);
    }
    private void HighlightUpdate(InteractiveIcon sender)
    {
        if(sender.type == InteractiveIcon.IconType.Part || sender.type == InteractiveIcon.IconType.Model)
        {
            for (int i = 0; i < LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID].outlines.Length; i++)
            {
                if(i == sender.id)
                {
                    myOutline = LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID].outlines[i];
                    break;
                }
            } 
        }
    }
    private void HighlightPulse(float newValue)
    {
        myOutline.OutlineColor = new Color(myOutline.OutlineColor.r, myOutline.OutlineColor.g, myOutline.OutlineColor.b, newValue);
    }
}
