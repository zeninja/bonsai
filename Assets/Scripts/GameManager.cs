using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    public CameraController cam;
    public List<BranchNode> nodes = new List<BranchNode>();

    Vector3 screen_mouse_down;
    Vector3 screen_mouse_current;
    Vector3 screen_mouse_delta;

    public enum Selection { None, Knob };
    public Selection selection;

    BranchKnob selected_knob;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateObjects();
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
            HandleSpace();
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     foreach (Branch b in branches)
        //     {
        //         b.SpawnChildBranch();
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     foreach (Branch b in branches)
        //     {
        //         b.SpawnBranch();
        //     }
        // }
    }

    void UpdateObjects()
    {
        foreach (BranchNode n in nodes)
        {
            n.DoUpdate();
        }

        cam.DoUpdate();
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
            if (hit.collider.GetComponent<BranchKnob>() != null)
            {
                selection = Selection.Knob;
                selected_knob = hit.collider.GetComponent<BranchKnob>();
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
            case Selection.None:
                cam.HandleMouseDown();
                break;
        }
    }

    void HandleMouseHeld()
    {
        screen_mouse_current = Input.mousePosition;
        screen_mouse_delta = (screen_mouse_current - screen_mouse_down) / (Screen.width / 2);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        switch (selection)
        {
            case Selection.Knob:
                selected_knob.HandleMouseHeld();
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
            case Selection.None:
                break;
        }

        screen_mouse_down = Vector2.zero;
    }

    void HandleSpace()
    {
        SpawnBranches();
    }

    void SpawnBranches()
    {
        BranchNode[] current_node_list = new BranchNode[nodes.Count];

        nodes.CopyTo(current_node_list);
        
        foreach (BranchNode n in current_node_list)
        {
            n.SpawnBranch();
        }
    }

    public void HandleNodeAdded(BranchNode new_node)
    {
        nodes.Add(new_node);
    }
}
