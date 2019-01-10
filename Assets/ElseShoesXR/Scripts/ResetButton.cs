using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public delegate void ResetEvent();
    public static event ResetEvent OnReset;

    public static bool isSwitchingCollection, isResetting = true;
#if UNITY_EDITOR && !VUFORIA
    private void Start()
    {
        //Invoke("ResetModel", 2);
    }
#endif
    public void ResetModel()
    {
        AutoRotation.Instance.StopAllCoroutines();
        AutoRotation.isRotating = false;

        for (int i = 0; i < LoadedCollection.Instance.loadedCollection.productsPrefabs.Length; i++)
        {
#if VUFORIA
            DefaultTrackableEventHandler.currentTarget = "";
            Vector3 spawnPosition = ARBtn.isAR ? new Vector3(100, 100, 100) : Vector3.zero;
#else
            Vector3 spawnPosition = Vector3.zero;
#endif
            GameObject product = Instantiate(LoadedCollection.Instance.loadedCollection.productsPrefabs[i], spawnPosition, Quaternion.Euler(0, 180, 0));

            if (LoadedCollection.Instance.productSetups[i] != null && !isSwitchingCollection)
                Destroy(LoadedCollection.Instance.productSetups[i].gameObject);

            LoadedCollection.Instance.productSetups[i] = product.GetComponent<ProductSetup>();

            if (i == 0)
                foreach (Transform ts in LoadedCollection.Instance.productSetups[i].GetComponentsInChildren<Transform>())
                    ts.gameObject.layer = 0;
            else
                foreach (Transform ts in LoadedCollection.Instance.productSetups[i].GetComponentsInChildren<Transform>())
                    ts.gameObject.layer = 1;
        }

        foreach (SelectionCircle sc in SystemManager.Instance.selectionCircles)
            for (int i = 0; i < sc.selectedIcons.Length; i++)
            {
                sc.selectedIcons[i] = 0;
            }

        if (OnReset != null)
            OnReset();

        GameObject temp = new GameObject();
        InteractiveIcon icon = temp.AddComponent<InteractiveIcon>();
        SystemManager.Instance.selectedMatID = 0;
        SystemManager.Instance.selectedModelID = 0;
        SystemManager.Instance.selectedPartID = 0;
        icon.type = InteractiveIcon.IconType.Color;
        SummaryCam.Instance.PreviewUpdate(icon);
        PartSelection.Instance.ListingUpdate();
        icon.type = InteractiveIcon.IconType.Part;
        MaterialSelection.Instance.ListingUpdate(icon);
        icon.type = InteractiveIcon.IconType.Material;
        ColorSelection.Instance.ListingUpdate(icon);
        isSwitchingCollection = false;
        isResetting = true;
        AutoRotation.Instance.OnClick();
        Summary.Instance.FillSummary();
        HighlightGlowHandler.Instance.StartGlowing();
        Destroy(temp);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
