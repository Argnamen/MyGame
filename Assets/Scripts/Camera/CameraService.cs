using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        _gameMode.StealthMod += StealsCameraOn;
        _gameMode.FightMod += FightCameraOn;
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

    public void ForceCamera(Vector3 newPos)
    {
        Transform look = _cinemachineVirtualCamera.LookAt;
        Transform follow = _cinemachineVirtualCamera.Follow;

        _cinemachineVirtualCamera.Follow = null;
        _cinemachineVirtualCamera.LookAt = null;

        _cinemachineVirtualCamera.transform.localPosition = new Vector3(newPos.x, newPos.y, _cinemachineVirtualCamera.transform.localPosition.z);

        _cinemachineVirtualCamera.Follow = follow;
        _cinemachineVirtualCamera.LookAt = look;
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

    public void ForceCamera(Vector3 newPos);
}
