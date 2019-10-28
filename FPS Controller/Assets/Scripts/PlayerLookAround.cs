using UnityEngine;

public class PlayerLookAround : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform player;

    private float xRotate = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //hiding and locking the mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //mouse movement from side to side
        float mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotate -= mouseInputY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        player.Rotate(Vector3.up * mouseInputX);
    }
}
