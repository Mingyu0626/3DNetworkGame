using UnityEngine;
using UnityEngine.UI;

public class MinimapZoomController : MonoBehaviour
{
    [SerializeField]
    private Button _zoomInButton;

    [SerializeField]
    private Button _zoomOutButton;

    [SerializeField]
    private float _minOrthographicSize = 5f;

    [SerializeField]
    private float _maxOrthographicSize = 20f;

    [SerializeField]
    private float _zoomSpeed = 1f;

    private Camera _minimapCamera;

    private void Awake()
    {
        _minimapCamera = GetComponent<Camera>();
        _zoomInButton.onClick.AddListener(ZoomIn);
        _zoomOutButton.onClick.AddListener(ZoomOut);
    }

    private void OnDestroy()
    {
        _zoomInButton.onClick.RemoveListener(ZoomIn);
        _zoomOutButton.onClick.RemoveListener(ZoomOut);
    }

    private void ZoomIn()
    {
        float newSize = _minimapCamera.orthographicSize - _zoomSpeed;
        _minimapCamera.orthographicSize = Mathf.Clamp(newSize, _minOrthographicSize, _maxOrthographicSize);
    }

    private void ZoomOut()
    {
        float newSize = _minimapCamera.orthographicSize + _zoomSpeed;
        _minimapCamera.orthographicSize = Mathf.Clamp(newSize, _minOrthographicSize, _maxOrthographicSize);
    }
} 