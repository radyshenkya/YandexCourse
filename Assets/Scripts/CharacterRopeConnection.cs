using UnityEngine;

public class CharacterRopeConnection : MonoBehaviour
{
    public bool IsConnected
    {
        get { return _ropeConnectionJoint.enabled; }
    }

    private HingeJoint2D _ropeConnectionJoint;

    private void Awake()
    {
        _ropeConnectionJoint = GetComponent<HingeJoint2D>();
    }

    public void ConnectTo(Rigidbody2D ropeRigidBody)
    {
        _ropeConnectionJoint.connectedBody = ropeRigidBody;
        _ropeConnectionJoint.anchor = Vector2.zero;
        _ropeConnectionJoint.enabled = true;
    }

    public void Disconnect()
    {
        _ropeConnectionJoint.enabled = false;
    }
}
