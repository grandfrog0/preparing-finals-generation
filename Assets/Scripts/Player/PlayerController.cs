using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform _cameraTransform;
    [SerializeField] float _movementSpeed = 5;
    [SerializeField] Rigidbody _rigidbody;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Quaternion rot = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
        Vector3 axis = rot * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _movementSpeed;

        _rigidbody.velocity = axis;

        if (axis != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, axis, 10 * Time.deltaTime);
        }
    }
}
