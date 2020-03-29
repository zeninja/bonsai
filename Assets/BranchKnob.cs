using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchKnob : MonoBehaviour
{
    Branch  my_branch;
    Vector3 branch_origin;
    Vector3 branch_direction;

    public Material knob_mat;
    MeshRenderer renderer;

    public Color selected_color;
    public Color deselected_color;

    Plane branch_plane;

    bool selected = false;

    void Start() {
        renderer = GetComponent<MeshRenderer>();
    }

    public void InitKnob()
    {
        GetComponent<MeshRenderer>().material = knob_mat;
        my_branch = GetComponentInParent<Branch>();
        branch_direction = my_branch.direction;
        branch_origin = my_branch.transform.position;

        GameManager.GetInstance().knobs.Add(this);
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        renderer.material.color = IsSelected() ? selected_color : deselected_color;
        SetPosition();
    }

    void SetPosition()
    {
        transform.position = branch_origin + branch_direction * my_branch.branch_length * 2;
    }

    // void FindMyPlane()
    // {
    //     Vector3 cam_pos = Camera.main.transform.position;
    //     Vector3 forward = Camera.main.transform.forward;
    //     Vector3 a, b, c;

    //     Vector3 origin = GameManager.tree_origin;

    //     if (my_branch.HasParent())
    //     {
    //         a = branch_origin;
    //         b = branch_origin + branch_origin * my_branch.min_branch_length;
    //         c = GameManager.tree_origin;
    //     }
    //     else
    //     {
    //         a = origin + Camera.main.transform.right * 5;
    //         b = origin - Camera.main.transform.right * 5;
    //         c = origin + Vector3.up * 5 + Vector3.forward * plane_spacing;
    //     }

    //     Debug.DrawLine(a, b, Color.green, 1f);
    //     Debug.DrawLine(b, c, Color.green, 1f);
    //     Debug.DrawLine(c, a, Color.green, 1f);

    //     branch_plane.Set3Points(a, b, c);
    // }

    public void HandleMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // If holding shift, kill the branch and its children
            my_branch.KillBranch();
        }
        else
        {
            selected = true;
        }
    }

    // public float plane_spacing = 1f;

    public void HandleMouseHeld()
    {
        if (IsSelected())
        {
            float dist_from_cam = (Camera.main.transform.position - transform.position).magnitude;
            Vector3 raw_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist_from_cam);
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(raw_mouse_pos);

            // Get closest point on the line extending from branch origin in branch direction
            // Vector3 plane_pos = branch_plane.ClosestPointOnPlane(mouse_pos);


            Vector3 knob_pos = Vector3.Project(mouse_pos, branch_origin + branch_direction);
            float branch_length = (knob_pos - branch_origin).magnitude;
            my_branch.SetLength(branch_length);

            red_gizmo_pos = mouse_pos;
            mag_gizmo_pos = knob_pos;
            // blu_gizmo_pos = plane_pos;
        }
    }

    public void HandleMouseUp()
    {
        if (IsSelected())
        {
            selected = false;
        }
    }

    // Vector3 GetClosestPointOnLine(Vector3 mouse_in)
    // {
    //     return Vector3.ProjectOnPlane(mouse_in, branch_plane.normal);

    //     // return Vector3.Project(mouse_in, branch_origin + branch_direction);
    // }

    bool IsSelected()
    {
        return selected;
    }

    Vector3 red_gizmo_pos;
    Vector3 mag_gizmo_pos;
    Vector3 blu_gizmo_pos;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(red_gizmo_pos, .3f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(mag_gizmo_pos, .2f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(blu_gizmo_pos, .1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(branch_origin, .5f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(GameManager.tree_origin, .5f);
    }

}