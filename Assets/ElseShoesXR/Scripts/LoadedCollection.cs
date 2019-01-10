using System.Collections;
using UnityEngine;


public class LoadedCollection : MonoBehaviour
{

    #region Singleton
    //Singleton pattern implementation.
    private static LoadedCollection _instance;

    public static LoadedCollection Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;

            productSetups = new ProductSetup[loadedCollection.productsPrefabs.Length];

            for (int i = 0; i < loadedCollection.productsPrefabs.Length; i++)
            {
                GameObject product = Instantiate(loadedCollection.productsPrefabs[i], new Vector3(1000,1000,1000), Quaternion.Euler(0, 180, 0));

                if (productSetups[i] != null)
                    Destroy(productSetups[i].gameObject);

                productSetups[i] = product.GetComponent<ProductSetup>();

                if (i == 0)
                    foreach (Transform ts in productSetups[i].GetComponentsInChildren<Transform>())
                        ts.gameObject.layer = 0;
                else
                    foreach (Transform ts in productSetups[i].GetComponentsInChildren<Transform>())
                        ts.gameObject.layer = 1;
            }
        }
    }
    #endregion

    [HideInInspector]
    public ProductSetup[] productSetups;
    public ELSECollection loadedCollection;
    public ELSECollection[] availableCollections;

    private void OnEnable()
    {
        InteractiveIcon.OnIconClicked += ModelSelection;
    }
    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= ModelSelection;
    }

    private void Start()
    {
        FillDefaultMaterials();
    }

    void ModelSelection(InteractiveIcon sender)
    {
        if(sender.type == InteractiveIcon.IconType.Model)
        {
            for (int i = 0; i < productSetups.Length; i++)
            {
                if (i == SystemManager.Instance.selectedModelID)
                    foreach(Transform ts in productSetups[i].GetComponentsInChildren<Transform>())
                        ts.gameObject.layer = 0;
                else
                    foreach (Transform ts in productSetups[i].GetComponentsInChildren<Transform>())
                        ts.gameObject.layer = 1;
            }
        }
    }

    private void FillDefaultMaterials()
    {
        foreach(ELSECollection ec in availableCollections)
        {
            foreach(GameObject pp in ec.productsPrefabs)
            {
                ProductSetup ps = pp.GetComponent<ProductSetup>();
                
                foreach(ConfigurablePart cp in ps.configurableParts)
                {
                    cp.defaultMaterial.material = cp.renderer.sharedMaterial;
                }
            }
        }
    }
}

