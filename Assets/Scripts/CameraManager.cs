using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        //camera.GetComponent<Transform>().localPosition = player.transform.position;
        Camera.main.transform.localPosition = new Vector3(player.transform.position.x,player.transform.position.y + 265f,player.transform.position.z - 50f);
    }
}
