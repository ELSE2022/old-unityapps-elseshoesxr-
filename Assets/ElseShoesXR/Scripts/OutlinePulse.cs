using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlinePulse : MonoBehaviour {

    public Outline myOutline;
    public bool isEnabled;
    private Hashtable ht = iTween.Hash("from", 0, "to", 1, "time", 1, "looptype", "pingpong", "onupdate", "HighlightPulse");

    private void Start()
    {
        myOutline = GetComponent<Outline>();
    }

    void Update ()
    {
        if (myOutline.enabled && !isEnabled)
        {
            iTween.ValueTo(gameObject, ht);

            isEnabled = true;
        }
        else if (!myOutline.enabled && isEnabled)
        {
            iTween.Stop();
            isEnabled = false;
        }
    }

    private void HighlightPulse(float newValue)
    {
        myOutline.OutlineColor = new Color(myOutline.OutlineColor.r, myOutline.OutlineColor.g, myOutline.OutlineColor.b, newValue);
    }
}
