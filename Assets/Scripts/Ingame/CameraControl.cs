using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform limit_left;
    [SerializeField] Transform limit_Right;
    [SerializeField] Transform trans_cam;
    private float pos_x;
    [SerializeField] float sensity = 1;
    [SerializeField] float speed;
    private void LateUpdate()
    {
        Vector3 delta_move = InputManager.delta_mouse;
        pos_x = trans_cam.localPosition.x;
        pos_x = Mathf.Lerp(pos_x, pos_x - delta_move.x * sensity, Time.deltaTime * speed);
        pos_x = Mathf.Clamp(pos_x, limit_left.localPosition.x, limit_Right.localPosition.x);
        trans_cam.localPosition = new Vector3(pos_x, trans_cam.localPosition.y, trans_cam.localPosition.z);
    }
}
