using UnityEngine;

public class CharacterRopeConnection : MonoBehaviour
{
    private HingeJoint2D _ropeConnectionJoint;
    private Rigidbody2D _rigidbody;
    private Rigidbody2D _lastConnectedRopeRigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ropeConnectionJoint = GetComponent<HingeJoint2D>();
        ConnectTo(_ropeConnectionJoint.connectedBody);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<RopeEnd>(out RopeEnd ropeEnd))
        {
            Rigidbody2D ropeRigidbody = other.GetComponent<Rigidbody2D>();

            if (ropeRigidbody == _lastConnectedRopeRigidbody || _ropeConnectionJoint.enabled) { return; }

            ConnectTo(ropeRigidbody);
        }
    }

    public void ConnectTo(Rigidbody2D ropeEndRigidBody)
    {
        // Телепортируем игрока на центр сегмента веревки, что бы он не цеплялся за воздух, а держался по центру.
        // Vector3 ropeEndPosition = ropeEndRigidBody.transform.position;
        // Vector3 teleportedPosition = new Vector3(ropeEndPosition.x, ropeEndPosition.y, transform.position.z);
        // transform.position = teleportedPosition;

        _lastConnectedRopeRigidbody = ropeEndRigidBody;
        _ropeConnectionJoint.connectedBody = ropeEndRigidBody;
        _ropeConnectionJoint.anchor = Vector2.zero;
        _ropeConnectionJoint.enabled = true;
    }

    public void Disconnect()
    {
        _ropeConnectionJoint.enabled = false;
    }
}
