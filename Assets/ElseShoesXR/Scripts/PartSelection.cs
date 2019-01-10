using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartSelection : MonoBehaviour
{

    #region Singleton
    //Singleton pattern implementation.
    private static PartSelection _instance;

    public static PartSelection Instance { get { return _instance; } }

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

    public Transform parts;
    public GameObject IconPrefab;
    public Transform circleMask;
    public Image scrollArrow;

    public void Start()
    {
        ListingUpdate();
    }

    public void ListingUpdate()
    {
        foreach (Transform trans in parts)
            if (trans != parts)
                Destroy(trans.gameObject);

        ConfigurablePart[] conParts = LoadedCollection.Instance
            .productSetups[SystemManager.Instance.selectedModelID]
            .configurableParts;

        for (int i = 0; i < conParts.Length; i++)
        {
            GameObject partIcon = Instantiate(IconPrefab, parts);
            InteractiveIcon ii = partIcon.GetComponent<InteractiveIcon>();

            partIcon.GetComponent<Image>().sprite = LoadedCollection.Instance
                .productSetups[SystemManager.Instance.selectedModelID]
                .configurableParts[i].partIcon;

            ii.type = InteractiveIcon.IconType.Part;
            ii.id = i;
            partIcon.GetComponentInChildren<TextMeshProUGUI>().text = conParts[i].name;

            if (LoadedCollection.Instance
                .productSetups[SystemManager.Instance.selectedModelID]
                .configurableParts[i]
                .availableShapes.Length > 1)
                ii.hasOptions = true;
        }

        if (parts.childCount > 12)
            scrollArrow.enabled = true;
        else
            scrollArrow.enabled = false;
    }
}