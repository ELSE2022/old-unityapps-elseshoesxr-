using UnityEngine;

public class SummaryCam : MonoBehaviour {

    #region Singleton
    //Singleton pattern implementation.
    private static SummaryCam _instance;

    public static SummaryCam Instance { get { return _instance; } }

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

    public Transform loc1, loc2;

    private Vector3 p1, p2;
    private Quaternion q1, q2;
    private Vector3 s1, s2;

    private void OnEnable()
    {
        p1 = loc1.position;
        p2 = loc2.position;
        q1 = loc1.rotation;
        q2 = loc2.rotation;
        s1 = loc1.localScale;
        s2 = loc2.localScale;
        InteractiveIcon.OnIconClicked += PreviewUpdate;
    }
    private void OnDisable()
    {
        InteractiveIcon.OnIconClicked -= PreviewUpdate;
    }

    public void PreviewUpdate(InteractiveIcon sender)
    {
        if(sender.type == InteractiveIcon.IconType.Color || sender.type == InteractiveIcon.IconType.Model || sender.type == InteractiveIcon.IconType.Option)
        {
            foreach (Transform ts in transform)
                if (ts != transform)
                    Destroy(ts.gameObject);

            GameObject productInstance = LoadedCollection.Instance.productSetups[SystemManager.Instance.selectedModelID].gameObject;

            GameObject shoe1 = Instantiate(productInstance, p1, q1, transform);
            GameObject shoe2 = Instantiate(productInstance, p2, q2, transform);
            shoe1.tag = shoe2.tag = "Finish";
            Destroy(shoe1.GetComponent<BoxCollider>());
            Destroy(shoe2.GetComponent<BoxCollider>());
            Destroy(shoe1.GetComponent<DragRotate>());
            Destroy(shoe2.GetComponent<DragRotate>());
            shoe1.transform.localScale = s1;
            shoe2.transform.localScale = s2;


            shoe1.SetActive(true);
            shoe2.SetActive(true);
        }
    }
}
