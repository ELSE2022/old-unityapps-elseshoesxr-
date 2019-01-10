using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class MaterialSelection : MonoBehaviour
{
    #region Singleton
    //Singleton pattern implementation.
    private static MaterialSelection _instance;

    public static MaterialSelection Instance { get { return _instance; } }

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

    public Text materialCount;
    public Transform materials;
    public GameObject IconPrefab;
    public Transform circleMask;
    public Image scrollArrow;

    public delegate void ListUpdate();
    public static event ListUpdate OnUpdate;

    private void Start()
    {
        //Auto-selection at start
        SystemManager.Instance.selectedPartID = 0;
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
            case InteractiveIcon.IconType.ElementPart:
                foreach (Transform trans in materials)
                    if (trans != materials)
                        Destroy(trans.gameObject);
                if (!sender.isNone)
                {
                    ConfigurablePart cPart = LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID]
                        .configurableParts[SystemManager.Instance.selectedPartID];

                    List<ELSEMaterial> am = cPart.availableShapes[cPart.defaultShapeIndex].availableMaterials;

                    for (int i = 0; i < am.Count; i++)
                    {
                        GameObject matIcon1 = Instantiate(IconPrefab, materials);
                        InteractiveIcon ii1 = matIcon1.GetComponent<InteractiveIcon>();
                        matIcon1.GetComponent<Image>().sprite = GetSprite(am[i].name);
                        ii1.type = InteractiveIcon.IconType.Material;
                        ii1.id = i;
                        matIcon1.GetComponentInChildren<TextMeshProUGUI>().text = am[i].name;
                    }
                    // Wait for the icon to be correctly positioned by the UI System.
                    StartCoroutine(DelayedEvent(.02f));
                }
                break;
            case InteractiveIcon.IconType.Element:
                foreach (Transform trans in materials)
                    if (trans != materials)
                        Destroy(trans.gameObject);

                ELSEMaterial eMat = LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID]
                    .availableShapes[SystemManager.Instance.selectedOptionID]
                    .linkedObjects[SystemManager.Instance.selectedElementID]
                    .material;

                GameObject matIcon2 = Instantiate(IconPrefab, materials);

                InteractiveIcon ii2 = matIcon2.GetComponent<InteractiveIcon>();

                matIcon2.GetComponent<Image>().sprite = eMat.icon;
                ii2.type = InteractiveIcon.IconType.Material;
                ii2.id = 0;
                matIcon2.GetComponentInChildren<TextMeshProUGUI>().text = eMat.name;
                break;
            case InteractiveIcon.IconType.Option:
                if (sender.isNone)
                {
                    foreach (Transform trans in materials)
                        if (trans != materials)
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

        if (materials.childCount > 6)
            scrollArrow.enabled = true;
        else
            scrollArrow.enabled = false;
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
                return LoadedCollection.Instance.loadedCollection.materialsLibrary[i].icon;
            }

        return null;
    }
}