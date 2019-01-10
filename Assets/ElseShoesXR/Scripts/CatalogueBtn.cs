using UnityEngine;

public class CatalogueBtn : MonoBehaviour {

    public Canvas catalogueScreen;
    public Canvas[] canvasesToDeactivate;

    public delegate void Show();
    public static event Show OnShowing;

    public void ShowCatalogue()
    {
        if (OnShowing != null)
            OnShowing();

        foreach (Canvas canvas in canvasesToDeactivate)
            canvas.enabled = false;

        for (int i = 0; i < LoadedCollection.Instance.productSetups.Length; i++)
        {
            if (LoadedCollection.Instance.productSetups[i] != null)
                LoadedCollection.Instance.productSetups[i].transform.position = new Vector3(1000, 1000, 1000);
        }

        AutoRotation.isRotating = false;
        catalogueScreen.enabled = true;
    }
}
