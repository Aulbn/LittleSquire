using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField]
    private float cameraSensitivity = 1.0f;
    private float cameraYaw = 1.0f;
    private float cameraPitch = 1.0f;

    [SerializeField]
    private float cameraSmoothing = 0.2f;

    [SerializeField]
    private float cameraDistance = 10f;
    private float cameraHeight = 1.0f;

    [SerializeField] [Range(-30f, 30f)]
    private float minCameraPitch = 1f;
    [SerializeField] [Range(60f, 90f)]
    private float maxCameraPitch = 80f;

    [SerializeField]
    private Transform cameraTarget;
    [SerializeField] private LayerMask collisionLayer;

    
    Vector3 cameraCollisionOffset;
    [Range(0f, 1f)] public float cameraCollisionOffsetX;
    [Range(0f, 1f)] public float cameraCollisionOffsetY = 0.5f;
    [Range(0f, 1f)] public float cameraCollisionOffsetZ;
    public Vector3 cameraLineTraceOffset;
    
    private Transform cameraTransform;
    private Vector2 cameraReferenceSmoothing = Vector3.zero;
    private Vector2 currentCameraRotation = Vector3.zero;
    private Vector3 targetPosition;

    private void Awake()
    {
        if (Camera.main != null) 
            cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraCollisionOffset = new Vector3(cameraCollisionOffsetX, cameraCollisionOffsetY, cameraCollisionOffsetZ);
    }

    public void UpdateCamera(Vector2 mouseDelta)
    {
        mouseDelta *= cameraSensitivity;
        cameraYaw += mouseDelta.x;
        cameraPitch += -mouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        currentCameraRotation = Vector2.SmoothDamp(currentCameraRotation,
            new Vector2(cameraPitch, cameraYaw), ref cameraReferenceSmoothing, cameraSmoothing);

        cameraTransform.eulerAngles = new Vector3(currentCameraRotation.x, currentCameraRotation.y, 0.0f);

        targetPosition = cameraTarget.position - (cameraTransform.forward * cameraDistance) + (cameraTransform.up * cameraHeight);
        
        CompensateForWalls(cameraTarget.position, ref targetPosition);
        cameraTransform.position = targetPosition;
    }

    public void SetSensitivity(float value)
    {
        cameraSensitivity = value;
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget) {
        if (Physics.Linecast(fromObject, toTarget + cameraLineTraceOffset, out RaycastHit hit, collisionLayer)) {
            toTarget = new Vector3(hit.point.x, hit.point.y, hit.point.z) + cameraCollisionOffset;
        }
    }
}
