using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    float h;
    float v;
    float moveLimiter = 0.7f;

    Rigidbody2D rb;

    public Camera cam;
    public Transform camFollow;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        cam = Camera.main;
        camFollow = cam.transform.parent;

        SetCameraToFollow();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            Vector2 moveDirection = rb.velocity;

            if (moveDirection != Vector2.zero)  //if we're moving
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameController.gameIsActive)
        {
            if (h != 0 && v != 0)   //check for diagonal movement
            {
                //limit moving speed diagonally, at 70% speed
                h *= moveLimiter;
                v *= moveLimiter;
            }

            rb.velocity = new Vector2(h * speed, v * speed) * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.Sleep();
        }
    }

    void SetCameraToFollow()
    {
        CameraController cameraController = camFollow.GetComponent<CameraController>();

        cameraController.FindPlayer(gameObject.transform);
    }
}
