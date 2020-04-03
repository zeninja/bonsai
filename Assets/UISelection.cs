using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{

    public void OnCreated()
    {
        SetPosition();
        transform.localScale = Vector3.one;
        Debug.Log(transform.eulerAngles);
    }

    public void OnUpdate()
    {
        SetPosition();
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void SetPosition()
    {
        Vector2 screen_pos = Input.mousePosition;
        Vector2 half = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector2 pos = screen_pos - half;

        transform.localPosition = pos;
    }

    public void OnDestroyed()
    {
        Destroy(gameObject);
    }
}
