using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Summary : MonoBehaviour, IPointerDownHandler
{
    #region Singleton
    //Singleton pattern implementation.
    private static Summary _instance;

    public static Summary Instance { get { return _instance; } }

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

    public float from;
    public float to;
    public float time;
    public RectTransform top;
    public RectTransform center;
    public RectTransform layoutGroup;
    public Text title;
    public Transform summaryElements;
    public GameObject elementPrefab;


    public List<SummaryElement> elementsList;

    private float panelHeight;
    private Hashtable ht;
    private float temp;
    private RectTransform thisTransform;
    private bool isAnimating;

    //private Dictionary<string, Queue<GameObject>> poolDictionary;

    void OnEnable()
    {

        thisTransform = GetComponent<RectTransform>();
        ht = iTween.Hash("from", from, "to", to, "time", time, "onupdate", "AnimatePanel", "oncomplete", "SwitchValues");
    }

    void Start()
    {   //FillSummary();
        //poolDictionary = new Dictionary<string, Queue<GameObject>>();
        GeneratePools();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isAnimating)
        {
            iTween.ValueTo(gameObject, ht);
            isAnimating = true;
        }
    }

    public void GeneratePools()
    {

    }

    public void FillSummary()
    {
        if (elementsList.Count > 0)
        {
            elementsList.Clear();
            foreach (Transform se in summaryElements)
                if (se != summaryElements)
                    Destroy(se.gameObject);
        }

        for (int j = 0; j < LoadedCollection.Instance.productSetups[0].configurableParts.Length; j++)
        {
            SummaryElement newElement = Instantiate(elementPrefab, summaryElements).GetComponent<SummaryElement>();
            newElement.part.text = LoadedCollection.Instance.productSetups[0].configurableParts[j].name;
            newElement.mat.text = LoadedCollection.Instance.productSetups[0].configurableParts[j].defaultMaterial.typeName;
            newElement.col.text = LoadedCollection.Instance.productSetups[0].configurableParts[j].defaultMaterial.material.name;
            newElement.price.text = "0€"; //LoadedCollection.Instance.productSetups[i].configurableParts[j].name;
            elementsList.Add(newElement);
        }
    }

    #region iTween Callbacks
    void AnimatePanel(float newValue)
    {
        panelHeight = newValue;
        thisTransform.sizeDelta = new Vector2(center.sizeDelta.x, panelHeight + top.sizeDelta.y);
        center.sizeDelta = new Vector2(center.sizeDelta.x, panelHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }

    void SwitchValues()
    {
        isAnimating = false;
        temp = from;
        from = to;
        to = temp;
        ht = iTween.Hash("from", from, "to", to, "time", time, "onupdate", "AnimatePanel", "oncomplete", "SwitchValues");
    }
    #endregion
}