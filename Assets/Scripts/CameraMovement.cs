using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _zoomSpeed = 2.0f;

    public float _maxMoveSpeed = 0.3f;

    private Vector3 _startScreenPosition = Vector3.zero;

    private Vector3 _currentScreenPosition = Vector3.zero;

    private Vector3 _cameraPosition = Vector3.zero;

    private bool _isMovingTo = false;

    private Vector3 _targetPosition;

    [SerializeField]
    private Vector2 _touchPosition0 = new Vector2(0, 0);

    [SerializeField]
    private Vector2 _touchPosition1 = new Vector2(0, 0);

    [SerializeField]
    private bool _isZooming = false;

    [SerializeField]
    private bool _isDragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && _isZooming == false)
            {
                Touch touch0 = Input.GetTouch(0);

                if (touch0.phase == TouchPhase.Stationary)
                {
                    SetStartScreenPosition(touch0.position);
                }

                if (touch0.phase == TouchPhase.Moved)
                {
                    _isDragging = true;
                    SetMoveScreenPosition(Input.mousePosition);
                }
            }

            if (Input.touchCount == 2 && _isDragging == false)
            {
                _isZooming = true;

                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Stationary && touch1.phase == TouchPhase.Began)
                {
                    _touchPosition0 = Input.GetTouch(0).position;
                    _touchPosition1 = Input.GetTouch(1).position;
                }

                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector2 touchDragPosition0 = Input.GetTouch(0).position;
                    Vector2 touchDragPosition1 = Input.GetTouch(1).position;

                    float zoomDistance = Vector2.Distance(touchDragPosition0, touchDragPosition1) - Vector2.Distance(_touchPosition0, _touchPosition1);

                    if (zoomDistance > 5f)
                    {
                        Zoom(true);
                    }
                    else if (zoomDistance < -5f)
                    {
                        Zoom(false);
                    }
                }

                if (touch0.phase == TouchPhase.Moved)
                {
                    _touchPosition0 = Input.GetTouch(0).position;
                }

                if (touch0.phase == TouchPhase.Moved)
                {
                    _touchPosition1 = Input.GetTouch(1).position;
                }
            }
        }
        else
        {
            ResetPositions();

            if (Input.GetMouseButtonDown(0))
            {
                SetStartScreenPosition(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                SetMoveScreenPosition(Input.mousePosition);
            }
        }

        if (_isMovingTo == true)
        {
            TransformCamera();
        }
        

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom(false);
        }
    }

    void SetStartScreenPosition(Vector3 screenPos)
    {
        _startScreenPosition = screenPos;
        _cameraPosition = transform.position;
    }

    void SetMoveScreenPosition(Vector3 newScreenPos)
    {
        _currentScreenPosition = newScreenPos;
        _currentScreenPosition.z = _startScreenPosition.z = _cameraPosition.y;
        Vector3 direction = (Camera.main.ScreenToWorldPoint(_currentScreenPosition) - Camera.main.ScreenToWorldPoint(_startScreenPosition)) * -1;
        _targetPosition = _cameraPosition + direction;
        _targetPosition.y = transform.position.y;
        _isMovingTo = true;
    }

    void TransformCamera()
    {
        _targetPosition = Vector3.MoveTowards(transform.position, _targetPosition, _maxMoveSpeed);
        float distance = Vector3.Distance(transform.position, _targetPosition);

        if (distance >= _maxMoveSpeed / 5 && distance <= _maxMoveSpeed * 2)
        {
            transform.position = _targetPosition;
            _isMovingTo = false;
        }
        else
        {
            Debug.Log($"Too long {distance}");
        }
    }

    void ResetPositions()
    {
        _isDragging = false;
        _isZooming = false;
        _touchPosition0 = new Vector2(0, 0);
        _touchPosition1 = new Vector2(0, 0);
    }

    void Zoom(bool zoomIn = true)
    {
        if (zoomIn)
        {
            gameObject.transform.Translate(new Vector3(0, -1, 2) * Time.deltaTime * _zoomSpeed);
        }
        else
        {
            gameObject.transform.Translate(new Vector3(0, 1, -2) * Time.deltaTime * _zoomSpeed);
        }
    }
}
