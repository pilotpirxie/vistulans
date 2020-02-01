using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Speed of zoom in/out
    /// </summary>
    public float ZoomSpeed = 2.0f;

    /// <summary>
    /// Speed of movement in 2-axis
    /// </summary>
    public float MaxMoveSpeed = 0.3f;

    /// <summary>
    /// Minimum height
    /// </summary>
    public float MinHeight = 2f;

    /// <summary>
    /// Maximum height
    /// </summary>
    public float MaxHeight = 8f;

    /// <summary>
    /// Position where camera is when player starts dragging
    /// </summary>
    private Vector3 _startScreenPosition = Vector3.zero;

    /// <summary>
    /// Current position of viewport over world when dragging
    /// </summary>
    private Vector3 _currentScreenPosition = Vector3.zero;

    /// <summary>
    /// Current position of camera in the world when dragging
    /// </summary>
    private Vector3 _cameraPosition = Vector3.zero;

    /// <summary>
    /// Flag, is actually moving to target direction or not
    /// </summary>
    private bool _isMovingTo = false;

    /// <summary>
    /// Target positon used for moving in direction
    /// </summary>
    private Vector3 _targetPosition;

    /// <summary>
    /// Position of the first touch on the screen
    /// </summary>
    [SerializeField]
    private Vector2 _touchPosition0 = new Vector2(0, 0);

    /// <summary>
    /// Position of the second touch on the screen
    /// </summary>
    [SerializeField]
    private Vector2 _touchPosition1 = new Vector2(0, 0);

    /// <summary>
    /// Flag, is zooming or not
    /// </summary>
    [SerializeField]
    private bool _isZooming = false;

    /// <summary>
    /// Flag, is dragging or not
    /// </summary>
    [SerializeField]
    private bool _isDragging = false;

    void Update()
    {
        // Check if is touch input,
        // otherwise reset positions
        // check for mouse input
        if (Input.touchCount > 0)
        {
            // Check if is touching with single finger
            // and not zooming
            if (Input.touchCount == 1 && _isZooming == false)
            {
                SingleTouch();
            }

            // Or with two fingers and not dragging
            if (Input.touchCount == 2 && _isDragging == false)
            {
                DualTouch();   
            }
        }
        else
        {
            ResetPositions();

            // Check if left button is pressed down (on start)
            if (Input.GetMouseButtonDown(0))
            {
                SetStartScreenPosition(Input.mousePosition);
            }

            // Check if left button is hold down
            if (Input.GetMouseButton(0))
            {
                SetMoveScreenPosition(Input.mousePosition);
            }
        }

        // If flag for moving to is set
        // move camera in direction of target position
        if (_isMovingTo == true)
        {
            TransformCamera();
        }
        
        // Check if mouse input is scrolling in/out
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom(false);
        }
    }

    /// <summary>
    /// Drag camera with single finger touch
    /// </summary>
    void SingleTouch()
    {
        Touch touch0 = Input.GetTouch(0);

        if (touch0.phase == TouchPhase.Stationary)
        {
            SetStartScreenPosition(touch0.position);
        }
        else if (touch0.phase == TouchPhase.Moved)
        {
            _isDragging = true;
            SetMoveScreenPosition(touch0.position);
        }
    }

    /// <summary>
    /// Zoom in/out camera with two fingers touch
    /// </summary>
    void DualTouch()
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

        if (touch1.phase == TouchPhase.Moved)
        {
            _touchPosition1 = Input.GetTouch(1).position;
        }
    }

    /// <summary>
    /// Set origin position of transformation for dragging
    /// </summary>
    /// <param name="screenPos">Old screen position</param>
    void SetStartScreenPosition(Vector3 screenPos)
    {
        _startScreenPosition = screenPos;
        _cameraPosition = transform.position;
    }

    /// <summary>
    /// Based on new and old position prepare new target position
    /// </summary>
    /// <param name="newScreenPos">New screen position</param>
    void SetMoveScreenPosition(Vector3 newScreenPos)
    {
        _currentScreenPosition = newScreenPos;
        _currentScreenPosition.z = _startScreenPosition.z = _cameraPosition.y;
        Vector3 direction = (Camera.main.ScreenToWorldPoint(_currentScreenPosition) - Camera.main.ScreenToWorldPoint(_startScreenPosition)) * -1;
        _targetPosition = _cameraPosition + direction;
        _targetPosition.y = transform.position.y;
        _isMovingTo = true;
    }

    /// <summary>
    /// Move camera to target position,
    /// prevent from screen shaking with 
    /// maximum distance to travel at once
    /// </summary>
    void TransformCamera()
    {
        _targetPosition = Vector3.MoveTowards(transform.position, _targetPosition, MaxMoveSpeed);
        float distance = Vector3.Distance(transform.position, _targetPosition);

        if (distance >= MaxMoveSpeed / 5 && distance <= MaxMoveSpeed * 2)
        {
            transform.position = _targetPosition;
            _isMovingTo = false;
        }
    }

    /// <summary>
    /// Set props to inital values
    /// </summary>
    void ResetPositions()
    {
        _isDragging = false;
        _isZooming = false;
        _touchPosition0 = new Vector2(0, 0);
        _touchPosition1 = new Vector2(0, 0);
    }

    /// <summary>
    /// Set height of camera and zoom in/out
    /// </summary>
    /// <param name="zoomIn">Zoom in</param>
    void Zoom(bool zoomIn = true)
    {
        if (zoomIn)
        {
            if (MinHeight < gameObject.transform.position.y)
            {
                gameObject.transform.Translate(new Vector3(0, -1, 2) * Time.deltaTime * ZoomSpeed);
            }
        }
        else
        {
            if (MaxHeight > gameObject.transform.position.y)
            {
                gameObject.transform.Translate(new Vector3(0, 1, -2) * Time.deltaTime * ZoomSpeed);
            }
        }
    }
}
