using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum IconType { Part, Material, Color, Option, Model, Element, ElementPart, ElementColor }
    public IconType type;
    public int id;

    [HideInInspector]
    public bool hasOptions;

    [HideInInspector]
    public bool isNone;

    public delegate void IconClick(InteractiveIcon sender);
    public static event IconClick OnIconClicked;
    public static event IconClick OnIconUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (type)
        {
            case IconType.Part:
                SystemManager.Instance.selectedPartID = id;
                ProductSetup ps = LoadedCollection.Instance.productSetups[SystemManager.Instance.selectedModelID];
                ConfigurablePart cp0 = ps.configurableParts[SystemManager.Instance.selectedPartID];
                if (cp0.customRotatedPosition != Vector3.zero)
                {
                    AutoRotation.isRotating = false;
                    AutoRotation.defaultRotation = cp0.customRotatedPosition;
                }
                else
                {
                    if (AutoRotation.isRotating == false)
                    {
                        AutoRotation.defaultRotation = new Vector3(0, 180, 0);
                        AutoRotation.Instance.OnClick();
                    }
                }

                for (int i = 0; i < ps.outlines.Length; i++)
                {
                    if (i == id)
                        ps.outlines[i].enabled = true;
                    else
                        ps.outlines[i].enabled = false;
                }
                break;
            case IconType.Material:
                ConfigurablePart cp1 = LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID];

                cp1.selectedMatID = id;
                SystemManager.Instance.selectedMatID = id;
                break;
            case IconType.Color:
                if (LoadedCollection.Instance.productSetups != null)
                {
                    for (int i = 0; i < LoadedCollection.Instance.productSetups.Length; i++)
                    {
                        ConfigurablePart cpi = LoadedCollection.Instance
                        .productSetups[i]
                        .configurableParts[SystemManager.Instance.selectedPartID];

                        Material matToApply = cpi.availableShapes[cpi.defaultShapeIndex]
                        .availableMaterials[SystemManager.Instance.selectedMatID]
                        .matVariations[id];

                        SummaryElement summaryElement = Summary.Instance.elementsList[SystemManager.Instance.selectedPartID];
                        summaryElement.mat.text = cpi.availableShapes[cpi.defaultShapeIndex]
                            .availableMaterials[SystemManager.Instance.selectedMatID].name;
                        summaryElement.col.text = matToApply.name;
                        summaryElement.price.text = "0€";


                        cpi.defaultMaterial.material = cpi.selectedMaterial = cpi.renderer.material = matToApply;

                        SystemManager.Instance.selectedColID = id;
                    }
                }
                break;
            case IconType.Option:
                if (tag != "Player")
                {
                    LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID].meshFilter.mesh =
                    LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID].availableShapes[id].shape;

                    foreach (AvailableShape shape in LoadedCollection.Instance
                        .productSetups[SystemManager.Instance.selectedModelID]
                        .configurableParts[SystemManager.Instance.selectedPartID].availableShapes)
                    {
                        foreach (LinkedObject lo in shape.linkedObjects)
                        {
                            if (shape == LoadedCollection.Instance
                                .productSetups[SystemManager.Instance.selectedModelID]
                                .configurableParts[SystemManager.Instance.selectedPartID].availableShapes[id])
                            {
                                lo.linkedGameObject.SetActive(true);
                            }
                            else
                            {
                                lo.linkedGameObject.SetActive(false);
                            }
                        }
                    }

                    foreach (SelectionCircle sCircle in SystemManager.Instance.selectionCircles)
                        if (sCircle.type == SelectionCircle.SelectionType.Part)
                        {
                            transform.SetParent(sCircle.icon.transform);
                            transform.localPosition = Vector3.zero;
                            transform.localScale = Vector3.one;
                        }

                    InteractiveIcon partIcon = PartSelection.Instance.parts
                        .GetChild(SystemManager.Instance.selectedPartID)
                        .GetComponent<InteractiveIcon>();

                    if (isNone)
                        partIcon.isNone = true;
                    else
                        partIcon.isNone = false;
                }
                break;
            case IconType.Model:
                SystemManager.Instance.selectedModelID = id;
                break;
            case IconType.Element:
                SystemManager.Instance.selectedElementID = id;
                break;
            case IconType.ElementColor:
                LinkedObject lObject = LoadedCollection.Instance
                    .productSetups[SystemManager.Instance.selectedModelID]
                    .configurableParts[SystemManager.Instance.selectedPartID]
                    .availableShapes[SystemManager.Instance.selectedOptionID]
                    .linkedObjects[SystemManager.Instance.selectedElementID];

                lObject.defaultColorID = id;
                lObject.linkedGameObject.GetComponent<Renderer>().material = lObject.material.matVariations[id];
                break;
        }
        SetSelectionTag();

        if (OnIconClicked != null)
            OnIconClicked(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnIconUp != null)
            OnIconUp(this);
    }

    private void SetSelectionTag()
    {
        GameObject[] selectedIcons = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in selectedIcons)
        {
            if (go.GetComponent<InteractiveIcon>().type == type)
                go.tag = "Untagged";
        }

        tag = "Player";
    }
}
