using UnityEngine;
using System.Collections;

public class InputHack : MonoBehaviour
{

    // Use this for initialization
    public static RaycastHit hit;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            
            if(hit.collider.tag=="BG")
                Debug.Log(hit.point.x);
        }
    }
}
