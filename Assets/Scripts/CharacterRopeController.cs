using UnityEngine;

public class CharacterRopeController : MonoBehaviour
{
    [SerializeField] private float _maxSwingVelocity;
    [SerializeField] private float _swingForce;

    private HingeJoint2D _ropeConnectionJoint;
    private Rigidbody2D _rigidbody;
    private Rigidbody2D _lastConnectedRopeRigidbody;
    private Vector2 _smoothedForceDirection = Vector2.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ropeConnectionJoint = GetComponent<HingeJoint2D>();
        ConnectToRope(_ropeConnectionJoint.connectedBody);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisconnectRope();
        }

        PlayerSwing();
    }

    private void PlayerSwing()
    {
        // Если игрок не на веревке - раскачка не нужна
        if (!_ropeConnectionJoint.enabled) { return; }

        if (Mathf.Abs(_rigidbody.velocity.x) > _maxSwingVelocity) { return; }

        Vector2 newForceDirection = _rigidbody.velocity.x > 0 ? Vector2.right : Vector2.left;
        _smoothedForceDirection = Vector2.Lerp(_smoothedForceDirection, newForceDirection, 0.2f);
        _rigidbody.AddForce(_smoothedForceDirection * _swingForce);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RopeEnd"))
        {
            Rigidbody2D ropeRigidbody = other.GetComponent<Rigidbody2D>();

            if (ropeRigidbody == _lastConnectedRopeRigidbody || _ropeConnectionJoint.enabled) { return; }

            ConnectToRope(ropeRigidbody);
        }
    }

    private void ConnectToRope(Rigidbody2D ropeEndRigidBody)
    {
        // Телепортируем игрока на центр сегмента веревки, что бы он не цеплялся за воздух, а держался по центру.
        Vector3 ropeEndPosition = ropeEndRigidBody.transform.position;
        Vector3 teleportedPosition = new Vector3(ropeEndPosition.x, ropeEndPosition.y, transform.position.z);
        transform.position = teleportedPosition;

        _lastConnectedRopeRigidbody = ropeEndRigidBody;
        _ropeConnectionJoint.connectedBody = ropeEndRigidBody;
        _ropeConnectionJoint.anchor = Vector2.zero;
        _ropeConnectionJoint.enabled = true;
    }

    private void DisconnectRope()
    {
        _ropeConnectionJoint.enabled = false;
    }
}