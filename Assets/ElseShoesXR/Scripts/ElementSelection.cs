using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementSelection : MonoBehaviour
{

    #region Singleton
    //Singleton pattern implementation.
    private static ElementSelection _instance;

    public static ElementSelection Instance { get { return _instance; } }

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

    public LayoutElement layoutElement;
    public Transform elements;
    public GameObject iconPrefab;
    public GameObject selectionCircle;
    public Transform circleMask;

    private bool hasElements;

    public static bool isPanelShown;

    [Header("Temporary Fix")]
    public GameObject circleIcon;

    private void OnEnable()
    {
        InteractiveIcon.OnIconClicked += ListingUpdate;
    }
    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= ListingUpdate;
    }

    private void ListingUpdate(InteractiveIcon sender)
    {
        if (sender.type == InteractiveIcon.IconType.Option)
        {
            SystemManager.Instance.selectedOptionID = sender.id;

            foreach (Transform trans in elements)
                if (trans != elements)
                    Destroy(trans.gameObject);

            LinkedObject[] los = LoadedCollection.Instance
                            .productSetups[SystemManager.Instance.selectedModelID]
                            .configurableParts[SystemManager.Instance.selectedPartID]
                            .availableShapes[sender.id].linkedObjects;

            for (int i = 0; i < los.Length; i++)
            {
                if (los[i].isElement)
                {
                    GameObject elementIcon = Instantiate(iconPrefab, elements);
                    InteractiveIcon ii = elementIcon.GetComponent<InteractiveIcon>();

                    elementIcon.GetComponent<Image>().sprite = los[i].elementIcon;

                    ii.type = InteractiveIcon.IconType.Element;
                    ii.id = i;

                    //elementIcon.GetComponentInChildren<TextMeshProUGUI>().text = los[i].linkedGameObject.name;
                    hasElements = true;
                }
            }

            if (hasElements == true)
            {
                AddMainIcon();
                ShowPanel(true);
            }
            else
                ShowPanel(false);

            hasElements = false;
        }
        else if (sender.type == InteractiveIcon.IconType.Part)
            ShowPanel(false);
    }

    private void AddMainIcon()
    {
        GameObject elementIcon = Instantiate(iconPrefab, elements);
        InteractiveIcon ii = elementIcon.GetComponent<InteractiveIcon>();
        elementIcon.transform.SetAsFirstSibling();

        elementIcon.GetComponent<Image>().sprite = LoadedCollection.Instance
                .productSetups[SystemManager.Instance.selectedModelID]
                .configurableParts[SystemManager.Instance.selectedPartID]
                .availableShapes[SystemManager.Instance.selectedOptionID].menuIcon;

        ii.type = InteractiveIcon.IconType.ElementPart;
        ii.id = SystemManager.Instance.selectedPartID;
    }

    private void ShowPanel(bool condition)
    {
        isPanelShown = condition;
        circleIcon.SetActive(condition);

        layoutElement.ignoreLayout = !condition;

        foreach (Image img in transform.GetComponentsInChildren<Image>())
            if(img.transform.GetComponent<Mask>() == null)
                img.enabled = condition;
    }
}
