using UnityEngine;

public class CharacterRopeAutoConnection : MonoBehaviour
{
    [SerializeField] private float _centerizingSmoothCoefficient = 0.01f;

    private CharacterRopeConnection _ropeConnection;
    private Rigidbody2D _lastConnectedRopeRigidbody;

    private void Start()
    {
        _ropeConnection = GetComponent<CharacterRopeConnection>();
        HingeJoint2D ropeConnectionJoint = GetComponent<HingeJoint2D>();

        _lastConnectedRopeRigidbody = ropeConnectionJoint.connectedBody;
        _ropeConnection.ConnectTo(_lastConnectedRopeRigidbody);
    }

    private void Update()
    {
        MoveToRopeCenter();
    }

    private void MoveToRopeCenter()
    {
        if (!_ropeConnection.IsConnected) { return; }

        Vector3 connectedRopePosition = _lastConnectedRopeRigidbody.transform.position;
        Vector3 desiredPosition = new Vector3(connectedRopePosition.x, connectedRopePosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _centerizingSmoothCoefficient);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Rope>(out Rope rope))
        {
            Rigidbody2D ropeRigidbody = other.GetComponent<Rigidbody2D>();
            if (ropeRigidbody == _lastConnectedRopeRigidbody || _ropeConnection.IsConnected) { return; }

            _lastConnectedRopeRigidbody = ropeRigidbody;
            _ropeConnection.ConnectTo(ropeRigidbody);
        }
    }
}
