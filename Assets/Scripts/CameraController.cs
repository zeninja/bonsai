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

    float rot;
    float tilt;

    public float tilt_rate = 1f;

    float tilt_affect = 0;
    Extensions.Property tilt_range = new Extensions.Property(5, 70f);

    public void DoUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetTargetRotation();
        }

        // transform.Rotate(y_tilt, x_rot, 0, Space.World);

        // tilt = tilt_range.start + ((Mathf.Sin(Time.time) + 1) / 2) * (tilt_range.end - tilt_range.start);

        tilt += tilt_affect;
        tilt = Mathf.Clamp(tilt, tilt_range.start, tilt_range.end);

        // Debug.Log(tilt);

        rot += rotation_rate;

        transform.eulerAngles = new Vector3(tilt, rot, 0);

        // transform.Rotate(transform.up * rotation_rate);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, auto_rot_speed * Time.deltaTime);
    }

    public void HandleMouseDown()
    {
        rotation_rate = 0;
    }

    public void HandleMouseHeld(Vector2 delta)
    {
        rotation_rate += delta.x * rot_speed * Time.deltaTime;
        tilt_affect +=  -delta.y * tilt_rate * Time.deltaTime;
    }

    public float tilt_decay_rate = 10f;
    public float rot_decay_rate = 10f;

    public void HandleMouseUnpressed()
    {
        rotation_rate = Mathf.Lerp(rotation_rate, 0, Time.deltaTime * rot_decay_rate);
        tilt_affect   = Mathf.Lerp(  tilt_affect, 0, Time.deltaTime * tilt_decay_rate);
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