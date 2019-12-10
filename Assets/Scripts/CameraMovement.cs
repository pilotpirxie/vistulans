using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _speed = 2.0f;

    private Vector3 _startScreenPosition = Vector3.zero;

    private Vector3 _currentScreenPosition = Vector3.zero;

    private Vector3 _cameraPosition = Vector3.zero;

    private bool _isMovingTo = false;

    private Vector3 _targetPosition;

    private bool _isTouchMove = false;

    private bool _isTouchDown = false;

    private Vector2 _touchPosition = new Vector2(0, 0);

    void Update()
    {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            _isTouchMove = touch.phase == TouchPhase.Moved;
            _isTouchDown = touch.phase == TouchPhase.Began;
            _touchPosition = touch.position;

            MoveCamera(_touchPosition);
        }
        else
        {
            MoveCamera(Input.mousePosition);
        }
    }

    void MoveCamera(Vector2 inputScreenPosition)
    {
        if (Input.GetMouseButtonDown(0) || _isTouchDown)
        {
            _startScreenPosition = inputScreenPosition;
            _cameraPosition = transform.position;
        }

        if (Input.GetMouseButton(0) || _isTouchMove)
        {
            _currentScreenPosition = inputScreenPosition;
            _currentScreenPosition.z = _startScreenPosition.z = _cameraPosition.y;
            Vector3 direction = Camera.main.ScreenToWorldPoint(_currentScreenPosition) - Camera.main.ScreenToWorldPoint(_startScreenPosition);
            direction = direction * -1;
            _targetPosition = _cameraPosition + direction;

            _isMovingTo = true;
        }

        if (_isMovingTo)
        {
            _targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);

            if (transform.position == _targetPosition)
            {
                _isMovingTo = false;
            }
        }
    }
}
