using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 mouse_curr_pos;
    Vector2 mouse_last_pos;

    public float rot_speed = 15f;
    public float auto_rot_speed = 200f;

    // float max_rotation_rate;

    float rotation_rate;

    Quaternion target_rotation;

    public void DoUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetTargetRotation();
        }

        transform.Rotate(Vector3.up * rotation_rate);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, auto_rot_speed * Time.deltaTime);
    }

    public void HandleMouseDown()
    {
        rotation_rate = 0;
    }

    public void HandleMouseHeld(Vector2 delta)
    {
        rotation_rate += delta.x * rot_speed * Time.deltaTime;
    }

    public void HandleMouseUnpressed()
    {
        rotation_rate = Mathf.Lerp(rotation_rate, 0, Time.deltaTime);
    }

    public void SetTargetRotation()
    {
        target_rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);
    }

    public void SetCameraSize(float size)
    {
        Camera.main.orthographicSize = size;
    }
}