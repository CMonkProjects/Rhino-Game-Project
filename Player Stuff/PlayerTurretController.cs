using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    public Camera cam;

    Vector3 mousePos;

    float rotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused && GameController.gameIsActive)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 lookDir = transform.position - mousePos;

            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg * rotationSpeed;    //We convert Atan2's radians to degrees by multiplying it by Rad2Deg
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
