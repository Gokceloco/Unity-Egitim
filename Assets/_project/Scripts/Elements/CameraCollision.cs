using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    public float smooth;
    private Vector3 _dollyDir;
    private float _distance;
    public LayerMask layerMask;

    public bool isActive;

    private void Awake()
    {
        _dollyDir = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        RaycastHit hit;

        if (Physics.Raycast(transform.parent.position, (transform.position - transform.parent.position), out hit, 3f, layerMask))
        {
            _distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            _distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _dollyDir * _distance, Time.deltaTime * smooth);            
    }
}
