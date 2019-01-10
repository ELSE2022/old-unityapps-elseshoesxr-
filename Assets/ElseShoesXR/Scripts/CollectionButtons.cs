using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButtons : MonoBehaviour {

    public enum CollectionStyle { Brogue, Decollete }
    public CollectionStyle collectionStyle;
    public string summaryTitle;
    public ResetButton resetBtn;
    public Canvas[] canvasesToActivate;

    private Canvas catalogueScreen;

    private void Start()
    {
        catalogueScreen = transform.parent.GetComponent<Canvas>();

#if UNITY_EDITOR && !VUFORIA
        //Invoke("SwitchCollection", 1);
#endif
    }

    public void SwitchCollection()
    {
        foreach (Canvas canvas in canvasesToActivate)
            canvas.enabled = true;

        for (int i = 0; i < LoadedCollection.Instance.productSetups.Length; i++)
        {
            if (LoadedCollection.Instance.productSetups[i] != null)
                Destroy(LoadedCollection.Instance.productSetups[i].gameObject);
        }


        switch (collectionStyle)
        {
            case CollectionStyle.Brogue:
                LoadedCollection.Instance.loadedCollection = LoadedCollection.Instance.availableCollections[(int)CollectionStyle.Brogue];
                catalogueScreen.enabled = false;
                break;
            case CollectionStyle.Decollete:
                LoadedCollection.Instance.loadedCollection = LoadedCollection.Instance.availableCollections[(int)CollectionStyle.Decollete];
                catalogueScreen.enabled = false;
                break;
        }
        SwapModels();
    }

    public void SwapModels()
    {
        LoadedCollection.Instance.productSetups = new ProductSetup[LoadedCollection.Instance.loadedCollection.productsPrefabs.Length];
        ResetButton.isSwitchingCollection = true;

        if (LoadedCollection.Instance.loadedCollection.productsPrefabs.Length > 1)
        {
            foreach (Image image in ModelSelection.Instance.GetComponentsInChildren<Image>())
                image.enabled = true;
            foreach (TextMeshProUGUI text in ModelSelection.Instance.GetComponentsInChildren<TextMeshProUGUI>())
                text.enabled = true;
        }
        else
        {
            foreach (Image image in ModelSelection.Instance.GetComponentsInChildren<Image>())
                image.enabled = false;
            foreach (TextMeshProUGUI text in ModelSelection.Instance.GetComponentsInChildren<TextMeshProUGUI>())
                text.enabled = false;
        }

        Summary.Instance.title.text = summaryTitle;
    }
}
