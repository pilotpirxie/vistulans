using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _movementSpeed = 2.0f;

    public float _zoomSpeed = 2.0f;

    private Vector3 _startScreenPosition = Vector3.zero;

    private Vector3 _currentScreenPosition = Vector3.zero;

    private Vector3 _cameraPosition = Vector3.zero;

    private bool _isMovingTo = false;

    private Vector3 _targetPosition;

    private bool _isTouchMove = false;

    private bool _isTouchDown = false;

    [SerializeField]
    private Vector2 _touchPosition0 = new Vector2(0, 0);

    [SerializeField]
    private Vector2 _touchPosition1 = new Vector2(0, 0);

    [SerializeField]
    private bool _isZooming = false;

    [SerializeField]
    private bool _isDragging = false;

    void FixedUpdate()
    {
        if (Input.touchCount == 1 || Input.touchCount == 2) {
            if (Input.touchCount == 1 && _isZooming == false)
            {
                Touch touch0 = Input.GetTouch(0);
                _isTouchMove = touch0.phase == TouchPhase.Moved;
                _isTouchDown = touch0.phase == TouchPhase.Began;
                _touchPosition0 = touch0.position;

                if (_isTouchDown)
                {
                    _startScreenPosition = _touchPosition0;
                    _cameraPosition = transform.position;
                }

                if (_isTouchMove == true)
                {
                    _isDragging = true;

                    _currentScreenPosition = _touchPosition0;
                    _currentScreenPosition.z = _startScreenPosition.z = _cameraPosition.y;
                    Vector3 direction = Camera.main.ScreenToWorldPoint(_currentScreenPosition) - Camera.main.ScreenToWorldPoint(_startScreenPosition);
                    direction = direction * -1;
                    _targetPosition = _cameraPosition + direction;

                    _isMovingTo = true;
                }
            }

            if (Input.touchCount == 2 && _isDragging == false)
            {
                _isZooming = true;

                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Began)
                {
                    _touchPosition0 = Input.GetTouch(0).position;
                }

                if (touch1.phase == TouchPhase.Began)
                {
                    _touchPosition1 = Input.GetTouch(1).position;
                }

                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector2 touchDragPosition0 = Input.GetTouch(0).position;
                    Vector2 touchDragPosition1 = Input.GetTouch(1).position;

                    float initialDistance = Vector2.Distance(_touchPosition0, _touchPosition1);
                    float dragDistance = Vector2.Distance(touchDragPosition0, touchDragPosition1);

                    float zoomDistance = dragDistance - initialDistance;

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
            _isDragging = false;
            _isZooming = false;
            _touchPosition0 = new Vector2(0, 0);
            _touchPosition1 = new Vector2(0, 0);

            if (Input.GetMouseButtonDown(0))
            {
                _startScreenPosition = Input.mousePosition;
                _cameraPosition = transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                _currentScreenPosition = Input.mousePosition;
                _currentScreenPosition.z = _startScreenPosition.z = _cameraPosition.y;
                Vector3 direction = Camera.main.ScreenToWorldPoint(_currentScreenPosition) - Camera.main.ScreenToWorldPoint(_startScreenPosition);
                direction = direction * -1;
                _targetPosition = _cameraPosition + direction;

                _isMovingTo = true;
            }
        }

        if (_isMovingTo)
        {
            _targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _movementSpeed);

            if (transform.position == _targetPosition)
            {
                _isMovingTo = false;
            }
        }
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
