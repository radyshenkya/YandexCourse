using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Slide : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody;

    private Vector2 _groundNormal;
    private Vector2 _targetVelocity;
    private bool _grounded;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private bool _isJumped = false;

    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        Vector2 alongSurface = Vector2.Perpendicular(_groundNormal);

        _targetVelocity = alongSurface * _speed;

        if (Input.GetKey(KeyCode.Space))
        {
            _isJumped = true;
        }
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        _grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = _groundNormal.x < 0 ? moveAlongGround * deltaPosition.x : moveAlongGround * -deltaPosition.x;

        ApplyMovement(move, false);

        move = Vector2.up * deltaPosition.y;

        ApplyMovement(move, true);

        Jumping();
    }

    private void Jumping()
    {
        if (!_isJumped) { return; }

        _isJumped = false;

        if (_grounded)
        {
            _velocity.y = _jumpForce;
        }
    }

    private void ApplyMovement(Vector2 move, bool yMovement)
    {
        if (move.magnitude <= MinMoveDistance) { _rigidbody.position += move; return; }


        float distance = move.magnitude;

        IEnumerable<RaycastHit2D> newHits = GetHits(move, distance);

        foreach (RaycastHit2D hit in newHits)
        {
            Vector2 currentNormal = hit.normal;

            if (currentNormal.y > _minGroundNormalY)
            {
                _grounded = true;
                if (yMovement)
                {
                    _groundNormal = currentNormal;
                    currentNormal.x = 0;
                }
            }

            float projection = Vector2.Dot(_velocity, currentNormal);
            if (projection < 0)
            {
                _velocity = _velocity - projection * currentNormal;
            }

            float modifiedDistance = hit.distance - ShellRadius;
            distance = modifiedDistance < distance ? modifiedDistance : distance;
        }

        _rigidbody.position += move.normalized * distance;
    }

    private IEnumerable<RaycastHit2D> GetHits(Vector2 move, float distance)
    {
        int count = _rigidbody.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

        return (IEnumerable<RaycastHit2D>)new ArraySegment<RaycastHit2D>(_hitBuffer, 0, count);
    }
}