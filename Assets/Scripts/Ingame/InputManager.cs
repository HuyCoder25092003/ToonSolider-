using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : BYSingletonMono<InputManager>
{
    public static Vector3 delta_mouse;
    private Vector3 ogrinal;
    void Update()
    {
        delta_mouse = Vector3.zero;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                ogrinal = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                delta_mouse = Input.mousePosition - ogrinal;
                ogrinal = Input.mousePosition;
            }
            else
            {
                delta_mouse = Vector3.zero;
                ogrinal = Vector3.zero;
            }
        }
    }
}
