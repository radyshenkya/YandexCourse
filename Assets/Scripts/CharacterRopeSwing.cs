using UnityEngine;

public class CharacterRopeSwing : MonoBehaviour
{
    [SerializeField] private float _maxSwingVelocity;
    [SerializeField] private float _swingForce;

    private HingeJoint2D _ropeConnectionJoint;
    private Rigidbody2D _rigidbody;
    private Vector2 _smoothedForceDirection = Vector2.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ropeConnectionJoint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        PlayerSwing();
    }

    private void PlayerSwing()
    {
        // Если игрок не на веревке - раскачка не нужна
        if (!_ropeConnectionJoint.enabled) { return; }

        if (Mathf.Abs(_rigidbody.velocity.x) > _maxSwingVelocity) { return; }

        Vector2 newForceDirection = _rigidbody.velocity.x > 0 ? Vector2.right : Vector2.left;
        _smoothedForceDirection = Vector2.Lerp(_smoothedForceDirection, newForceDirection, 0.1f);
        _rigidbody.AddForce(_smoothedForceDirection * _swingForce);
    }
}
