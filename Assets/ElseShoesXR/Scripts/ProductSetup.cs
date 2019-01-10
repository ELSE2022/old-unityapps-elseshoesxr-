using UnityEngine;

public class ProductSetup : MonoBehaviour
{
    public string targetName;
    public ConfigurablePart[] configurableParts;
    public Outline[] outlines;

    private DragRotate dragRotate;
    private BoxCollider bCollider;

    private void OnEnable()
    {
#if VUFORIA
        DefaultTrackableEventHandler.OnSwitch += TargetParenting;
        ARBtn.OnSwitch += ARSwitch;
#endif

    }

    private void Start()
    {
        outlines = new Outline[configurableParts.Length];
        dragRotate = GetComponent<DragRotate>();
        bCollider = GetComponent<BoxCollider>();

        if (outlines[0] == null && tag != "Finish")
        {
            for (int i = 0; i < configurableParts.Length; i++)
            {
                outlines[i] = configurableParts[i].renderer.gameObject.GetComponent<Outline>();
                outlines[i].OutlineWidth = 3;
                outlines[i].OutlineColor = new Color32(75, 170, 202, 255);
            }
        }
    }

    private void OnDisable()
    {
#if VUFORIA
        DefaultTrackableEventHandler.OnSwitch -= TargetParenting;
        ARBtn.OnSwitch -= ARSwitch;
#endif
    }

    public void TargetParenting(Transform sender)
    {
        if (transform.parent == null || transform.parent.name == targetName)
        {
            transform.SetParent(sender);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(0, 180, 0);
            GetComponent<DragRotate>().enabled = false;
        }
    }

    public void ARSwitch(bool isAR)
    {
        if (transform.parent == null || transform.parent.name == targetName)
        {
            if (!isAR)
            {
                transform.SetParent(null);
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = new Vector3(0, 180, 0);
                dragRotate.enabled = true;
                bCollider.enabled = true;

                // Makes sure the renderers are enabled when going back to No AR mode.
                foreach (ProductSetup ps in LoadedCollection.Instance.productSetups)
                    foreach (Renderer rend in ps.GetComponentsInChildren<Renderer>())
                        rend.enabled = true;
            }
            else
            {
                transform.localPosition = new Vector3(100,100,100); // Moves it temporarily out of the camera FOV.
                transform.SetParent(GameObject.Find(targetName).transform);
                dragRotate.enabled = false;
                bCollider.enabled = false;

#if VUFORIA
                DefaultTrackableEventHandler.currentTarget = null;
#endif
            }
        }
    }
}
