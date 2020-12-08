using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public Camera miniMapCam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        miniMapCam.transform.position = new Vector3(player.transform.position.x, miniMapCam.transform.position.y, player.transform.position.z);
    }
}
