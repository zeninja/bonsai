using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // other classes
    // public class TreeInfo
    // {
    //     public Vector3 origin;
    //     public Extents extents = new Extents();
    // }

    // public class Extents
    // {
    //     public float top;
    //     public float bottom;
    //     public float left;
    //     public float right;

    //     public void GetExtents()
    //     {

    //     }
    // }

    class InputData
    {
        public Vector3 screen_mouse_down;
        public Vector3 screen_mouse_current;
        public Vector3 screen_mouse_delta;
    }


    // variables
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    public UIManager ui;
    public CameraController cam;
    public List<BranchNode> nodes = new List<BranchNode>();
    public List<Branch> branches  = new List<Branch>();

    public enum Selection { None, Knob };
    public Selection selection;

    public CameraControl zoom_control;

    InputData input = new InputData();
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

    // This is the only update used in the game
    void Update()
    {
        HandleInput();
        UpdateObjects();
    }

    #region input
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            HandleGrow();
        }
    }

    void HandleMouseUnpressed()
    {
        cam.HandleMouseUnpressed();
    }

    void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        input.screen_mouse_down = Input.mousePosition;

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
                ui.HandleMouseDown();
                break;
            case Selection.None:
                cam.HandleMouseDown();
                break;
        }
    }

    void HandleMouseHeld()
    {
        input.screen_mouse_current = Input.mousePosition;
        input.screen_mouse_delta = (input.screen_mouse_current - input.screen_mouse_down) / (Screen.width / 2);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        switch (selection)
        {
            case Selection.Knob:
                selected_knob.HandleMouseHeld();
                ui.HandleMouseHeld();
                break;
            case Selection.None:
                cam.HandleMouseHeld(input.screen_mouse_delta);
                break;
        }
    }

    void HandleMouseUp()
    {
        switch (selection)
        {
            case Selection.Knob:
                ui.HandleMouseUp();
                selected_knob.HandleMouseUp();
                selected_knob = null;
                break;
            case Selection.None:
                break;
        }

        input.screen_mouse_down = Vector2.zero;
    }

    void HandleSpace()
    {
        SpawnBranches();
    }

    void HandleGrow()
    {
        GrowAllBranches();
    }

    void HandleShrink()
    {
        ShrinkAllBranches();
    }
    #endregion

    #region functions
    void UpdateObjects()
    {
        foreach (BranchNode n in nodes)
        {
            n.DoUpdate();
        }

        cam.DoUpdate();
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

    public void HandleNodeRemoved(BranchNode removed)
    {
        nodes.Remove(removed);
    }
    #endregion


    void GrowAllBranches()
    {
        float grow_speed = 1;

        foreach ( Branch b in branches )
        {
            b.Grow(grow_speed);
        }
    }

    void ShrinkAllBranches()
    {
        float shrink_speed = 1 * -1f;

        foreach ( Branch b in branches )
        {
            b.Grow(shrink_speed);
        }
    }


    public void HandleBranchAdded(Branch new_branch)
    {
        branches.Add(new_branch);
    }

    public void HandleBranchRemoved(Branch removed)
    {
        branches.Remove(removed);
    }

}
