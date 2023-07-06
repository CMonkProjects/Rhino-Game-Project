using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    //[SerializeField] float mouseCamSpeed = 10f;
    //[SerializeField] float mouseClampX = 1f;
    //[SerializeField] float mouseClampY = 1f;

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);


            //Mouse follow code clamped to the player position-----------------------------------------------------------------

            /*transform.position += new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * mouseCamSpeed, Input.GetAxis("Mouse Y") * Time.deltaTime * mouseCamSpeed, 0f);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, player.transform.position.x - mouseClampX, player.transform.position.x + mouseClampX),
                Mathf.Clamp(transform.position.y, player.transform.position.y - mouseClampY, player.transform.position.y + mouseClampY),
                transform.position.z);*/
        }
    }

    //Called by the player object
    public void FindPlayer(Transform _player)
    {
        player = _player;
    }
}
