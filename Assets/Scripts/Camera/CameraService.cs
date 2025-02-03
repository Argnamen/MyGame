using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CameraService
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private Transform _playerTransform;

    private GameObject _newLookPoint;
    public CameraService(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        _cinemachineVirtualCamera = cinemachineVirtualCamera;
    }

    public void LookAt(Vector2 target)
    {
        if(_playerTransform == null)
        {
            _playerTransform = _cinemachineVirtualCamera.LookAt;
        }

        if(_newLookPoint != null)
        {
            MonoBehaviour.Destroy(_newLookPoint);
        }

        _newLookPoint = new GameObject("CameraTarget");
        _newLookPoint.transform.position = new Vector3(target.x, target.y, 0);

        _cinemachineVirtualCamera.LookAt = _newLookPoint.transform;
    }

    public void CancleLookAt()
    {
        MonoBehaviour.Destroy(_newLookPoint);

        _cinemachineVirtualCamera.LookAt = _playerTransform;
    }
}
