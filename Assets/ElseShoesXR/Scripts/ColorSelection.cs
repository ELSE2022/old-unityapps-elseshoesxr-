using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ColorSelection : MonoBehaviour
{
    #region Singleton
    //Singleton pattern implementation.
    private static ColorSelection _instance;

    public static ColorSelection Instance { get { return _instance; } }

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

    public Text colorsCount;
    public Transform colors;
    public GameObject IconPrefab;
    public Transform circleMask;
    public Image scrollArrow;

    public delegate void ListUpdate();
    public static event ListUpdate OnUpdate;

    private bool isRefreshed;


    private void Start()
    {
        //Auto-selection at start
        SystemManager.Instance.selectedMatID = 0;
        Invoke("AutoSelection", .05f);
    }

    private void AutoSelection()
    {
        GameObject temp = new GameObject();
        InteractiveIcon icon = temp.AddComponent<InteractiveIcon>();
        ListingUpdate(icon);
        Destroy(temp);
    }

    private void OnEnable()
    {
        InteractiveIcon.OnIconClicked += ListingUpdate;
    }
    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= ListingUpdate;
    }

    public void ListingUpdate(InteractiveIcon sender)
    {
        switch (sender.type)
        {
            case InteractiveIcon.IconType.Part:
            case InteractiveIcon.IconType.Material:
            case InteractiveIcon.IconType.ElementPart:
                foreach (Transform trans in colors)
                    if (trans != colors)
                        Destroy(trans.gameObject);

                if (!sender.isNone)
                {
                    int matID = sender.type == InteractiveIcon.IconType.Part ? 
                        SystemManager.Instance.confirmedMatID : 
                        SystemManager.Instance.selectedMatID;

                    ConfigurablePart cPart = LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID]
                        .configurableParts[SystemManager.Instance.selectedPartID];
                    AvailableShape aShape = cPart.availableShapes[cPart.defaultShapeIndex];

                    // Prevents error if previous selected part had more materials available.
                    if (aShape.availableMaterials.Count - 1 < matID)
                        SystemManager.Instance.selectedMatID = 0;

                    Material[] mats = aShape.availableMaterials[matID].matVariations;

                    for (int i = 0; i < mats.Length; i++)
                    {
                        GameObject colorIcon = Instantiate(IconPrefab, colors);
                        InteractiveIcon ii = colorIcon.GetComponent<InteractiveIcon>();

                        colorIcon.GetComponent<Image>().sprite = GetSprite(aShape
                            .availableMaterials[matID].name);

                        colorIcon.GetComponent<Image>().color = mats[i].color;
                        ii.type = InteractiveIcon.IconType.Color;
                        ii.id = i;
                        colorIcon.GetComponentInChildren<TextMeshProUGUI>().text = mats[i].name;

                        if (mats[i] == cPart.defaultMaterial.material && sender.type != InteractiveIcon.IconType.Material)
                        {
                            ii.tag = "Finish";
                            // Wait for the icon to be correctly positioned by the UI System.
                            StartCoroutine(DelayedEvent(.02f));
                        }
                    }


                }
                break;
            case InteractiveIcon.IconType.Element:
                foreach (Transform trans in colors)
                    if (trans != colors)
                        Destroy(trans.gameObject);

                ELSEMaterial eMat = LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID]
                    .availableShapes[SystemManager.Instance.selectedOptionID]
                    .linkedObjects[SystemManager.Instance.selectedElementID]
                    .material;

                for (int i = 0; i < eMat.matVariations.Length; i++)
                {
                    GameObject colorIcon = Instantiate(IconPrefab, colors);
                    InteractiveIcon ii = colorIcon.GetComponent<InteractiveIcon>();

                    colorIcon.GetComponent<Image>().sprite = eMat.colorIcon;

                    colorIcon.GetComponent<Image>().color = eMat.matVariations[i].color == Color.black ? Color.white : eMat.matVariations[i].color;
                    ii.type = InteractiveIcon.IconType.ElementColor;
                    ii.id = i;
                    colorIcon.GetComponentInChildren<TextMeshProUGUI>().text = eMat.matVariations[i].name;
                }
                break;
            case InteractiveIcon.IconType.Option:
                if (sender.isNone)
                {
                    foreach (Transform trans in colors)
                        if (trans != colors)
                            Destroy(trans.gameObject);
                }
                else
                {
                    GameObject temp = new GameObject();
                    InteractiveIcon icon = temp.AddComponent<InteractiveIcon>();
                    ListingUpdate(icon);
                    Destroy(temp);
                }
                break;
        }

        if (colors.childCount > 12)
            scrollArrow.enabled = true;
        else
            scrollArrow.enabled = false;

            //MainScreen.Instance.Refresh();
    }

    private IEnumerator DelayedEvent(float time)
    {
        yield return new WaitForSeconds(time);
        if (OnUpdate != null)
            OnUpdate();
    }

    private Sprite GetSprite(string matName)
    {
        for (int i = 0; i < LoadedCollection.Instance.loadedCollection.materialsLibrary.Length; i++)
            if (LoadedCollection.Instance.loadedCollection.materialsLibrary[i].name == matName)
            {
                return LoadedCollection.Instance.loadedCollection.materialsLibrary[i].colorIcon;
            }

        return null;
    }
}