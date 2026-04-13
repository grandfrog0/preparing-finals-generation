using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IPointMovable
{
    [SerializeField] Transform _cameraTransform;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _moveSpeed;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Quaternion rot = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
        Vector3 axis = rot * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * MoveSpeed;

        //axis.y = _rigidbody.velocity.y;
        //_rigidbody.velocity = axis;

        if (axis != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, axis, 10 * Time.deltaTime);
        }

        _agent.Move(axis);
    }

    public void MoveToPoint(Vector3 pos)
    {
        _agent.SetDestination(pos);
    }
}
