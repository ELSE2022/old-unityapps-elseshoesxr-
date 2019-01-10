using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectionCircle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public enum SelectionType { Part, Element, Material, Color, Model }
    public SelectionType type;
    public GameObject IconPrefab;
    public Image icon;
    public TextMeshProUGUI text;
    public RectTransform optionsFlag;
    public Sprite[] flagSizes;
    public float optionsDelay;
    public float time;

    [Header("Temporary Fix for Part Selector")]
    public Sprite[] defaultPartIcons;
    public string[] defaultPartText;
    public Color[] defaultColors;

    //[HideInInspector]
    public int[] selectedIcons; /////

    [HideInInspector]
    public bool isDragged;

    private Image flag;
    private float from;
    private float to;
    private bool isHeld;
    private bool areOptionShown;
    private float timer;
    private Hashtable ht;
    private float temp;
    private float optionScaleTo;

    private bool hasOptions;
    private Transform _sender;
    private bool isNone;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private static int tempMatID;
    private int materialCircleId;

    public Vector3 startingPosition;

    #region Unity Methods
    private void OnEnable()
    {
        InteractiveIcon.OnIconClicked += MoveSelection;
        InteractiveIcon.OnIconUp += Unheld;
        ColorSelection.OnUpdate += SelectDefault;
        ResetButton.OnReset += OnResetEvent;
        CatalogueBtn.OnShowing += KillOptions;
        ht = iTween.Hash("from", from, "to", to, "time", time, "onupdate", "AnimateFlag", "oncomplete", "SwitchValues");
    }

    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= MoveSelection;
        InteractiveIcon.OnIconUp -= Unheld;
        ColorSelection.OnUpdate -= SelectDefault;
        ResetButton.OnReset -= OnResetEvent;
        CatalogueBtn.OnShowing -= KillOptions;
    }

    void Start()
    {
        startingPosition = transform.localPosition;
        flag = optionsFlag.GetComponent<Image>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        GeneratePools();

        for (int i = 0; i < SystemManager.Instance.selectionCircles.Length; i++)
        {
            if (SystemManager.Instance.selectionCircles[i].type == SelectionType.Material)
                materialCircleId = i;
        }
    }

    void Update()
    {
        if (_sender && !isNone)
        {
            transform.position = _sender.position;
        }
    }
    #endregion

    #region Icon Pooling

    public void GeneratePools()
    {
        ConfigurablePart[] cParts = LoadedCollection.Instance
            .productSetups[SystemManager.Instance.selectedModelID]
            .configurableParts;

        selectedIcons = new int[cParts.Length];

        foreach (ConfigurablePart cPart in cParts)
        {
            if (!poolDictionary.ContainsKey(cPart.name))
            {
                AvailableShape[] shapes = cPart.availableShapes;

                if (shapes.Length > 1)
                {
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int i = 0; i < shapes.Length; i++)
                    {
                        GameObject optionIcon = Instantiate(IconPrefab, optionsFlag);
                        InteractiveIcon ii = optionIcon.GetComponent<InteractiveIcon>();
                        optionIcon.SetActive(false);

                        optionIcon.GetComponent<Image>().sprite = cPart.availableShapes[i].menuIcon;

                        optionIcon.GetComponent<Image>().SetNativeSize();

                        if (optionIcon.GetComponent<Image>().sprite.name == "None")
                            ii.isNone = true;

                        ii.type = InteractiveIcon.IconType.Option;
                        ii.id = i;

                        if (cPart.defaultShapeIndex == i)
                            optionIcon.tag = "Finish";

                        objectPool.Enqueue(optionIcon);
                    }

                    poolDictionary.Add(cPart.name, objectPool);
                }
            }
        }
    }

    private void GenerateOptions()
    {
        foreach (Transform ts in optionsFlag)
            if (ts != optionsFlag)
                ts.gameObject.SetActive(false);

        string tag = LoadedCollection.Instance
            .productSetups[SystemManager.Instance.selectedModelID]
            .configurableParts[SystemManager.Instance.selectedPartID].name;

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        SetFlagSprite(poolDictionary[tag].Count);
        to = GetFlagWidth();
        ht = iTween.Hash("from", from, "to", to, "time", time, "onupdate", "AnimateFlag", "oncomplete", "SwitchValues");

        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            InteractiveIcon ii = objectToSpawn.GetComponent<InteractiveIcon>();

            if (objectToSpawn.tag == "Finish")
            {
                objectToSpawn.transform.SetParent(icon.transform);
                objectToSpawn.transform.localPosition = Vector3.zero;
                objectToSpawn.GetComponent<Image>().raycastTarget = false;
                objectToSpawn.tag = "Player";
            }
            if (objectToSpawn.tag == "Player")
            {
                objectToSpawn.transform.localScale = new Vector3(1, 1, 1);
                objectToSpawn.transform.SetParent(icon.transform);
                objectToSpawn.GetComponent<Image>().raycastTarget = false;
            }
            else
            {
                objectToSpawn.transform.SetParent(optionsFlag);
                objectToSpawn.transform.SetSiblingIndex(ii.id);
                objectToSpawn.GetComponent<Image>().raycastTarget = true;
            }

            objectToSpawn.SetActive(true);

            poolDictionary[tag].Enqueue(objectToSpawn);
        }
    }
    #endregion

    // Handles the circle positioning and triggers general interactions
    private void MoveSelection(InteractiveIcon sender)
    {
        isHeld = false;
        timer = 0;
        text.transform.localEulerAngles = Vector3.zero;

        if (sender.type.ToString() == type.ToString())
        {
            _sender = sender.transform;
            hasOptions = sender.hasOptions;
            Image senderImg = sender.GetComponent<Image>();
            icon.sprite = senderImg.sprite;
            icon.color = senderImg.color;
            text.text = sender.GetComponentInChildren<TextMeshProUGUI>().text;
            icon.SetNativeSize();
            transform.position = sender.transform.position;

            //selectedIcons[SystemManager.Instance.selectedPartID] = sender.id;

            switch (type.ToString())
            {
                case "Part":
                    tempMatID = SystemManager.Instance.selectedMatID = SystemManager.Instance.selectionCircles[materialCircleId].selectedIcons[SystemManager.Instance.selectedPartID];
                    foreach (Transform trans in icon.transform)
                        trans.gameObject.SetActive(false);

                    icon.enabled = true;
                    transform.SetParent(PartSelection.Instance.circleMask);
                    if (areOptionShown)
                    {
                        HideOptions(sender.transform, false);
                    }
                    if (hasOptions)
                    {
                        isHeld = true;
                        StartCoroutine(Held());
                    }
                    break;
                case "Material":
                    tempMatID = sender.id;
                    //transform.SetParent(MaterialSelection.Instance.circleMask);
                    break;
                case "Color":
                    //transform.SetParent(ColorSelection.Instance.circleMask);
                    break;
                case "Model":
                    //transform.SetParent(ModelSelection.Instance.circleMask);
                    break;
            }

        }
        else if (sender.type.ToString() == "Option") //Hide options upon selection.
        {
            sender.transform.tag = "Player";
            sender.GetComponent<Image>().raycastTarget = false;

            foreach (Transform trans in icon.transform)
                if (trans != icon && trans.tag != "Player" && trans != transform)
                    trans.gameObject.SetActive(false);

            HideOptions(sender.transform, true);

            if (sender.isNone)
            {
                if (type == SelectionType.Color || type == SelectionType.Material)
                {
                    isNone = true;
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                isNone = false;
            }
        }
        else if (type == SelectionType.Element && sender.type.ToString() == "Element")
        {
            print("STOCAZZO");
            icon.SetNativeSize();
            icon.transform.localScale = new Vector3(2, 2, 2);
        }
        else if (type == SelectionType.Element && sender.type.ToString() == "ElementPart")
        {
            _sender = sender.transform;
            hasOptions = sender.hasOptions;
            Image senderImg = sender.GetComponent<Image>();
            icon.sprite = senderImg.sprite;
            icon.color = senderImg.color;
            icon.SetNativeSize();
            icon.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            transform.position = sender.transform.position;
            tempMatID = SystemManager.Instance.selectedMatID = selectedIcons[SystemManager.Instance.selectedPartID];

            foreach (Transform trans in icon.transform)
                trans.gameObject.SetActive(false);

            //print(icon.sprite.name + "is " + ElementSelection.isPanelShown);
            //icon.enabled = ElementSelection.isPanelShown;
            icon.enabled = true;
            transform.SetParent(ElementSelection.Instance.circleMask);
        }
        else if (type == SelectionType.Color && sender.type.ToString() == "ElementColor")
        {
            _sender = sender.transform;
            hasOptions = sender.hasOptions;
            Image senderImg = sender.GetComponent<Image>();
            icon.sprite = senderImg.sprite;
            icon.color = senderImg.color;
            icon.SetNativeSize();
            transform.position = sender.transform.position;

            foreach (Transform trans in icon.transform)
                trans.gameObject.SetActive(false);

            icon.enabled = true;
            transform.SetParent(ColorSelection.Instance.circleMask);
        }
        else
        {
            if (type == SelectionType.Color || type == SelectionType.Material)
            {
                StartCoroutine(DelayedDefault(.02f));
            }

        }

        if (sender.type.ToString() == "Color" && type == SelectionType.Material)
        {
            selectedIcons[SystemManager.Instance.selectedPartID] = tempMatID;
            SystemManager.Instance.confirmedMatID = tempMatID;
        }
        else
        {
            SystemManager.Instance.confirmedMatID = SystemManager.Instance.selectionCircles[materialCircleId].selectedIcons[SystemManager.Instance.selectedPartID];
        }

        if (type == SelectionType.Element && sender.type.ToString() == "Element")
        {
            icon.SetNativeSize();
            icon.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
        }

        if (sender.type.ToString() == "Part" && type != SelectionType.Model) // Makes sure the icon is active if switching selection to another part.
            icon.enabled = true;

        if (icon.IsActive())
            text.enabled = true;
    }

    private IEnumerator DelayedDefault(float delay)
    {
        yield return new WaitForSeconds(delay);
        SelectDefault();
    }

    private void OnResetEvent()
    {
        GeneratePools();

        switch (type)
        {
            case SelectionType.Part:
                _sender = PartSelection.Instance.parts.GetChild(0);
                LoadDefaultValues(LoadedCollection.Instance.loadedCollection.name);
                break;
            case SelectionType.Material:
                _sender = MaterialSelection.Instance.materials.GetChild(0);
                LoadDefaultValues(LoadedCollection.Instance.loadedCollection.name);
                break;
            case SelectionType.Color:
                _sender = ColorSelection.Instance.colors.GetChild(0);
                LoadDefaultValues(LoadedCollection.Instance.loadedCollection.name);
                break;

        }
    }

    private void LoadDefaultValues(string colName)
    {
        switch (colName)
        {
            case "MaleCollection":
                icon.sprite = defaultPartIcons[0];
                text.text = defaultPartText[0];
                icon.color = defaultColors[0];
                break;
            case "WomanCollection":
                icon.sprite = defaultPartIcons[1];
                text.text = defaultPartText[1];
                icon.color = defaultColors[1];
                break;
        }
    }


    private void SelectDefault()
    {
        switch (type.ToString())
        {
            case "Part":
                if (ResetButton.isResetting)
                {
                    GeneratePools();

                    //_sender = PartSelection.Instance.parts.GetChild(0);

                    icon.sprite = LoadedCollection.Instance
                        .loadedCollection
                        .productsPrefabs[SystemManager.Instance.selectedModelID]
                        .GetComponent<ProductSetup>()
                        .configurableParts[0].partIcon;

                    ResetButton.isResetting = false;
                }
                break;
            case "Material":
                if (MaterialSelection.Instance.materials.childCount - 1 < SystemManager.Instance.selectedMatID)
                {
                    print("Zeroing out selectedMatID");
                    SystemManager.Instance.selectedMatID = 0;
                }

                if (MaterialSelection.Instance.materials.childCount > 0)
                {
                    int id = MaterialSelection.Instance.materials.childCount == 1 ? selectedIcons.Length - 1 : SystemManager.Instance.selectedPartID; // Protection for Elements.
                    _sender = MaterialSelection.Instance.materials.GetChild(selectedIcons[id]);
                    Image matImg = MaterialSelection.Instance.materials.GetChild(selectedIcons[id]).GetComponent<Image>();
                    SystemManager.Instance.selectedMatID = selectedIcons[SystemManager.Instance.selectedPartID];
                    icon.sprite = matImg.sprite;
                    icon.color = matImg.color;
                    text.text = _sender.GetComponentInChildren<TextMeshProUGUI>().text;
                    icon.SetNativeSize();
                }
                else
                {
                    transform.position = Vector3.zero;
                }

                break;
            case "Color":
                InteractiveIcon[] colors = ColorSelection.Instance.colors.GetComponentsInChildren<InteractiveIcon>();

                if (colors.Length > 0)
                {
                    foreach (InteractiveIcon ii in ColorSelection.Instance.colors.GetComponentsInChildren<InteractiveIcon>())
                    {
                        if (ii.tag == "Finish")
                        {
                            _sender = ii.transform;

                            Image senderImg = ii.GetComponent<Image>();
                            icon.sprite = senderImg.sprite;
                            icon.color = senderImg.color;
                            text.text = _sender.GetComponentInChildren<TextMeshProUGUI>().text;
                            icon.SetNativeSize();
                            break;
                        }
                        else if (ii.type == InteractiveIcon.IconType.ElementColor)
                        {
                            LinkedObject lObject = LoadedCollection.Instance
                                .productSetups[SystemManager.Instance.selectedModelID]
                                .configurableParts[SystemManager.Instance.selectedPartID]
                                .availableShapes[SystemManager.Instance.selectedOptionID]
                                .linkedObjects[SystemManager.Instance.selectedElementID];

                            if (ii.id == lObject.defaultColorID)
                            {
                                _sender = ii.transform;

                                Image senderImg = ii.GetComponent<Image>();
                                icon.sprite = senderImg.sprite;
                                icon.color = senderImg.color;
                                text.text = _sender.GetComponentInChildren<TextMeshProUGUI>().text;
                                icon.SetNativeSize();
                                break;
                            }
                        }
                        else
                        {
                            transform.position = Vector3.zero;
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.zero;
                }
                break;
            case "Element":
                if (ElementSelection.Instance.elements.childCount > 0)
                {
                    InteractiveIcon ii = ElementSelection.Instance.elements.GetChild(0).GetComponent<InteractiveIcon>();
                    transform.position = ii.transform.position;
                    Image senderImg = ii.GetComponent<Image>();
                    icon.sprite = senderImg.sprite;
                    icon.color = senderImg.color;
                    icon.SetNativeSize();
                }
                break;
        }
    }

    // Handles the flag sprite and sizes switching
    private int GetFlagWidth()
    {
        flag.SetNativeSize();
        RectTransform rt = flag.GetComponent<RectTransform>();
        int nativeWidth = (int)rt.sizeDelta.x;
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
        return nativeWidth;
    }
    private void SetFlagSprite(int count)
    {
        if (count == 2)
            flag.sprite = flagSizes[0];
        else
            flag.sprite = flagSizes[1];
    }

    #region Click & Hold Mechanism
    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasOptions)
        {
            isHeld = true;
            StartCoroutine(Held());
        }
    }

    IEnumerator Held()
    {
        while (isHeld && !areOptionShown)
        {
            timer += Time.deltaTime;
            if (timer > optionsDelay)
                ShowOptions();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
        timer = 0;
    }

    private void Unheld(InteractiveIcon sender)
    {
        timer = 0;
        isHeld = false;
    }
    #endregion

    #region Options Hide/Show
    private void ShowOptions()
    {
        icon.enabled = false;
        text.enabled = false;
        areOptionShown = true;
        optionScaleTo = 1;
        GenerateOptions();
        iTween.ValueTo(gameObject, ht);
    }

    private void HideOptions(Transform sender, bool animating)
    {
        optionScaleTo = 0;
        areOptionShown = false;
        if (animating)
            iTween.ValueTo(gameObject, ht);
        else
        {
            optionsFlag.sizeDelta = new Vector2(to, optionsFlag.sizeDelta.y);
            foreach (InteractiveIcon ii in GetComponentsInChildren<InteractiveIcon>())
                if (ii.transform.tag != "Player")
                    ii.transform.localScale = new Vector3(optionScaleTo, 1, 1);
            SwitchValues();
        }
    }

    private void KillOptions()
    {
        optionsFlag.sizeDelta = new Vector2(to, optionsFlag.sizeDelta.y);
        foreach (InteractiveIcon ii in GetComponentsInChildren<InteractiveIcon>())
                ii.transform.localScale = new Vector3(0, 1, 1);
        icon.enabled = true;
    }
    #endregion

    #region iTween Callbacks
    void AnimateFlag(float newValue)
    {
        optionsFlag.sizeDelta = new Vector2(newValue, optionsFlag.sizeDelta.y);

        foreach (InteractiveIcon ii in optionsFlag.GetComponentsInChildren<InteractiveIcon>())
            if (ii.transform.tag != "Player")
                iTween.ScaleTo(ii.gameObject, new Vector3(optionScaleTo, 1, 1), time);
    }

    void SwitchValues()
    {
        temp = from;
        from = to;
        to = temp;
        ht = iTween.Hash("from", from, "to", to, "time", time, "onupdate", "AnimateFlag", "oncomplete", "SwitchValues");
    }
    #endregion
}
