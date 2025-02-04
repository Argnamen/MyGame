using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraService: ISetupCamera, ICameraService
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private Transform _playerTransform;
    private GameObject _newLookPoint = null;

    private List<Transform> _lookAtTransforms = new List<Transform>();

    public CameraService(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        _cinemachineVirtualCamera = cinemachineVirtualCamera;
    }

    public void SetupVirtualCamera(Transform transform, bool isMain)
    {
        if (isMain)
        {
            if (_cinemachineVirtualCamera != null)
            {
                _cinemachineVirtualCamera.Follow = transform;
                _cinemachineVirtualCamera.LookAt = transform;
            }

            _playerTransform = transform;
        }
        else
        {
            _lookAtTransforms.Add(transform);
        }
    }

    public void LookAt(Vector2 target)
    {
        if (_newLookPoint != null)
        {
            MonoBehaviour.Destroy(_newLookPoint);
        }

        _newLookPoint = new GameObject("LookAtTarget");
        _newLookPoint.transform.position = new Vector3(target.x, target.y, _playerTransform.position.z);

        _cinemachineVirtualCamera.LookAt = _newLookPoint.transform;
    }

    public void CancelLookAt()
    {
        if (_newLookPoint != null)
        {
            MonoBehaviour.Destroy(_newLookPoint);
        }

        ResetCameraRotation();
    }

    private void ResetCameraRotation()
    {
        _cinemachineVirtualCamera.LookAt = _playerTransform;
        _cinemachineVirtualCamera.Follow = _playerTransform;
    }
}

public interface ISetupCamera
{
    public void SetupVirtualCamera(Transform playerTransform, bool isMain);
}

public interface ICameraService
{
    public void LookAt(Vector2 target);

    public void CancelLookAt();
}
