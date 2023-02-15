using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    private CharacterRopeConnection _characterRopeConnection;

    private void Start()
    {
        _characterRopeConnection = GetComponent<CharacterRopeConnection>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _characterRopeConnection.Disconnect();
        }
    }
}
