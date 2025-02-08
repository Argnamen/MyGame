using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraService: ISetupCamera, ICameraService
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private IGameMode _gameMode;
    private IStaticDataService _staticDataService;

    private CinemachineVirtualCamera _stealsCamera;

    private Transform _playerTransform;
    private GameObject _newLookPoint = null;

    private List<Transform> _lookAtTransforms = new List<Transform>();

    public CameraService(CinemachineVirtualCamera cinemachineVirtualCamera, IGameMode gameMode, IStaticDataService staticDataService)
    {
        _cinemachineVirtualCamera = cinemachineVirtualCamera;
        _gameMode = gameMode;
        _staticDataService = staticDataService;

        _gameMode.StealsMod += StealsCameraOn;
        _gameMode.FigthMod += FightCameraOn;
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

            SetupStealCamera();
        }
        else
        {
            _lookAtTransforms.Add(transform);
        }
    }

    private void SetupStealCamera()
    {
        GameObject cameraObject = new GameObject("StealsCamera");

        var virtualCamera = cameraObject.AddComponent<CinemachineVirtualCamera>();

        virtualCamera.enabled = false;

        virtualCamera.LookAt = _cinemachineVirtualCamera.LookAt;

        cameraObject.transform.position = new Vector3(0, 0, -_staticDataService.GetWorld(_staticDataService.CurrentRoom)[0].y * 5);

        _stealsCamera = virtualCamera;
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

    private void StealsCameraOn()
    {
        _cinemachineVirtualCamera.enabled = false;
        _stealsCamera.enabled = true;
    }

    private void FightCameraOn()
    {
        _stealsCamera.enabled = false;
        _cinemachineVirtualCamera.enabled = true;
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
