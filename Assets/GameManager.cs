using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Branch> branches;
    public List<Knob> knobs;
    public CameraController cam;

    Vector2 screen_mouse_down;
    Vector2 screen_mouse_current;
    public Vector2 screen_mouse_delta;

    public enum Selection { Branch, Knob, None }
    public Selection selection = Selection.None;

    Knob selected_knob;

    public static Vector3 tree_origin;

    void Start()
    {
        Debug.Log("Set tree origin!");
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        HandleInput();

        // Updates
        cam.DoUpdate();

        foreach (Branch b in branches)
        {
            b.DoUpdate();
        }

        foreach (Knob k in knobs)
        {
            k.DoUpdate();
        }
    }

    void HandleInput()
    {
        if (!Input.GetMouseButton(0))
        {
            HandleMouseUnpressed();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            HandleMouseHeld();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Branch b in branches)
            {
                b.SpawnBranch();
            }
        }
    }

    void HandleMouseUnpressed()
    {
        cam.HandleMouseUnpressed();
    }

    public void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        screen_mouse_down = Input.mousePosition;
        int layer = 1 << LayerMask.NameToLayer("Knob");

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 40, layer))
        {
            if (hit.collider.GetComponent<Knob>() != null)
            {
                selection = Selection.Knob;
                selected_knob = hit.collider.GetComponent<Knob>();
            }
        }
        else
        {
            selection = Selection.None;
        }

        switch (selection)
        {
            case Selection.Knob:
                selected_knob.HandleMouseDown();
                break;
            case Selection.Branch:
                break;
            case Selection.None:
                cam.HandleMouseDown();
                break;
        }
    }

    void HandleMouseHeld()
    {
        screen_mouse_current = Input.mousePosition;
        screen_mouse_delta = (screen_mouse_current - screen_mouse_down) / (Screen.width / 2);

        switch (selection)
        {
            case Selection.Knob:
                selected_knob.HandleMouseHeld();
                break;
            case Selection.Branch:
                break;
            case Selection.None:
                cam.HandleMouseHeld(screen_mouse_delta);
                break;
        }
    }

    void HandleMouseUp()
    {
        switch (selection)
        {
            case Selection.Knob:
                selected_knob.HandleMouseUp();
                selected_knob = null;
                break;
            case Selection.Branch:
                break;
            case Selection.None:
                break;
        }

        screen_mouse_down = Vector2.zero;
    }
}