using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    [SerializeField] Transform _cameraTransform;

    [SerializeField] Vector3 _offset;
    [SerializeField] float _moveSpeed;

    [SerializeField] Vector2 _distanceRange;
    [SerializeField] float _scrollSensitivity;

    [SerializeField] float _rotateAngle;

    private float _currentDistance;
    private float _mouseScroll;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, _moveSpeed * Time.fixedDeltaTime);
        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _offset.normalized * _currentDistance, _moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        
        if ((_mouseScroll = Input.GetAxis("Mouse ScrollWheel")) != 0)
        {
            _currentDistance = Mathf.Clamp(_currentDistance + _scrollSensitivity * _mouseScroll, _distanceRange.x, _distanceRange.y);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0, _rotateAngle, 0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0, -_rotateAngle, 0);
        }
    }

    private void Start()
    {
        _currentDistance = _distanceRange.x;
    }
}
