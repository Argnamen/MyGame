using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private CameraService _cameraService;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineImpulseSource _impulseSource;

    [Inject]
    public void Construct(CameraService cameraService)
    {
        _cameraService = cameraService;
    }

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        _cameraService.SetActiveCamera(_virtualCamera);
        _cameraService.SetFieldOfView(60f);
    }

    public void SetFollowTarget(Transform target)
    {
        _virtualCamera.Follow = target;
        _virtualCamera.LookAt = target;
    }

    public void AdjustFieldOfView(float fov)
    {
        _virtualCamera.m_Lens.FieldOfView = fov;
    }

    public void Shake(float intensity)
    {
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulse(intensity);
        }
    }

    private void OnSomeEvent()
    {
        Shake(1.0f);
    }
}

