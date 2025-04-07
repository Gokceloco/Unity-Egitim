using System;
using UnityEngine;

public class CameraDistanceRaycaster : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform cameraTargetTransform;

    public LayerMask layerMask = Physics.AllLayers;
    public float minimumDistanceFromObstacles = .1f;
    public float smoothingFactor = 25f;

    private Transform tr;
    float currentDistance;

    private void Awake()
    {
        tr = transform;
        
        currentDistance = (cameraTargetTransform.position - cameraTransform.position).magnitude;
    }

    private void LateUpdate()
    {
        Vector3 castDirection = cameraTargetTransform.position - cameraTransform.position;

        float distance = GetCameraDistance(castDirection);

        currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * smoothingFactor);
        cameraTransform.position = tr.position + castDirection.normalized * currentDistance;
    }

    private float GetCameraDistance(Vector3 castDirection)
    {
        float distance = castDirection.magnitude + minimumDistanceFromObstacles;
        if (Physics.Raycast(new Ray(tr.position, castDirection), out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Ignore))
        {
            print(hit.collider.gameObject.name);
            return Mathf.Max(0, hit.distance - minimumDistanceFromObstacles);
        }

        return castDirection.magnitude;
    }
}
