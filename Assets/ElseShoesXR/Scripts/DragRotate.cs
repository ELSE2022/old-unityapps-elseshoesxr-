using UnityEngine;
using System.Collections;

public class DragRotate : MonoBehaviour
{
    public enum RotationAxis { X, Y, XY }
    public RotationAxis rotationAxis;
    public float rotSpeed = 300;
    public string floorName;

    private float rotX, rotY;
    private Transform floor;
    private Hashtable ht;

#if VUFORIA
    private void Start()
    {
        floor = GameObject.Find(floorName).transform;
    }

    void OnMouseDrag()
    {
        floor.SetParent(transform);

        if (rotationAxis == RotationAxis.X || rotationAxis == RotationAxis.XY)
            rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;


        if (rotationAxis == RotationAxis.Y || rotationAxis == RotationAxis.XY)
            rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;


        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);
    }

    private void OnMouseUp()
    {
        floor.SetParent(null);
        iTween.RotateTo(gameObject, new Vector3(0, 180, 0), .5f);
        iTween.RotateTo(floor.gameObject, new Vector3(0, 180, 0), .5f);
        AutoRotation.isRotating = false;
    }
#endif
}