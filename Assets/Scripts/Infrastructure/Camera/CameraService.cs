using UnityEngine;
using Cinemachine;

public class CameraService
{
    private Camera _mainCamera;
    private CinemachineVirtualCamera _activeVirtualCamera;

    public CameraService(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }

    public void SetActiveCamera(CinemachineVirtualCamera virtualCamera)
    {
        if (_activeVirtualCamera != null)
        {
            _activeVirtualCamera.Priority = 10;
        }

        _activeVirtualCamera = virtualCamera;
        _activeVirtualCamera.Priority = 10;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        var impulseSource = _activeVirtualCamera.GetComponent<CinemachineImpulseSource>();
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }

    public void SetFieldOfView(float fieldOfView)
    {
        _mainCamera.fieldOfView = fieldOfView;
    }
}


