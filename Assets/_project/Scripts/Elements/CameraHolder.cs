using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform mainCamera;
    private Vector3 _originalCameraPosition;

    private void Start()
    {
        _originalCameraPosition = mainCamera.localPosition;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        mainCamera.DOKill();
        mainCamera.localPosition = _originalCameraPosition;
        mainCamera.DOShakePosition(duration, magnitude, 10);
    }
}
