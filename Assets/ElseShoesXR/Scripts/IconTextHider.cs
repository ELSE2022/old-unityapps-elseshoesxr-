using UnityEngine;
using TMPro;

public class IconTextHider : MonoBehaviour {

	void OnEnable() {
        InteractiveIcon.OnIconClicked += TextSwitch;
    }
	
	void OnDisable () {
        InteractiveIcon.OnIconClicked += TextSwitch;
    }

    void TextSwitch(InteractiveIcon sender)
    {
        InteractiveIcon[] iIcons = FindObjectsOfType<InteractiveIcon>();
        {
            foreach (InteractiveIcon iIcon in iIcons)
                if (iIcon.type == sender.type)
                {
                    if (iIcon.id == sender.id)
                        iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                    else
                        iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    switch(sender.type)
                    {
                        case InteractiveIcon.IconType.Part:
                            if (sender.id != SystemManager.Instance.selectedPartID)
                                iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            break;
                        case InteractiveIcon.IconType.Material:
                            if (sender.id != SystemManager.Instance.selectedMatID)
                                iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            break;
                        case InteractiveIcon.IconType.Color:
                            if (sender.id != SystemManager.Instance.selectedColID)
                                iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            break;
                        case InteractiveIcon.IconType.Model:
                            if (sender.id != SystemManager.Instance.selectedModelID)
                                iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            break;
                        case InteractiveIcon.IconType.Element:
                            if (sender.id != SystemManager.Instance.selectedElementID)
                                iIcon.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            break;
                    }
                }
        }
    }
}
