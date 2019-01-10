using System.Collections;
using UnityEngine;

public class AutoRotation : MonoBehaviour {

    #region Singleton
    //Singleton pattern implementation.
    private static AutoRotation _instance;

    public static AutoRotation Instance { get { return _instance; } }

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

    public float speed;

    private GameObject[] currentProducts;
    [SerializeField]
    public static bool isRotating;
    public static Vector3 defaultRotation;

    public void OnClick()
    {
        StartCoroutine(_OnClick());
    }

    public void Start()
    {
        defaultRotation = new Vector3(0, 180, 0);
    }

    private IEnumerator _OnClick()
    {
        isRotating = !isRotating;

        if (LoadedCollection.Instance.productSetups[SystemManager.Instance.selectedModelID] != null)
        {
            currentProducts = new GameObject[LoadedCollection.Instance.productSetups.Length];

            for (int i = 0; i < currentProducts.Length; i++)
            {
                currentProducts[i] = LoadedCollection.Instance.productSetups[i].gameObject;
            }

            while (isRotating)
            {
                if (currentProducts.Length != 0)
                    foreach (GameObject go in currentProducts)
                        go.transform.localEulerAngles += new Vector3(0, speed, 0);

                yield return null;
            }


            if (currentProducts.Length != 0)
                foreach (GameObject go in currentProducts)
                    iTween.RotateTo(go, defaultRotation, .5f);

        }
    }
}
