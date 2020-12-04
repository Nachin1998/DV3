using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image crossHair;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        crossHair.transform.position = Input.mousePosition;
    }
}
