using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour
{
    Branch my_branch;
    Vector3 branch_origin;
    Vector3 branch_direction;

    public Material knob_mat;

    bool selected = false;

    public Color selected_color;
    public Color deselected_color;

    float dist_from_cam;

    // List<Vector3> branch_plane = new List<Vector3>();
    Plane branch_plane;

    // Start is called before the first frame update
    void Start()
    {
        my_branch = GetComponentInParent<Branch>();
        branch_direction = my_branch.direction;
        branch_origin = my_branch.transform.position;

        InitPlane();

        // transform.position = branch_origin + branch_direction * my_branch.branch_length * 2;

        GetComponent<MeshRenderer>().material = knob_mat;
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        GetComponent<MeshRenderer>().material.color = IsSelected() ? selected_color : deselected_color;

        branch_origin = my_branch.transform.position;
        // transform.position = branch_origin + branch_direction * my_branch.branch_length * 2;
    }


    void InitPlane()
    {
        Vector3 a, b, c;
        a = branch_origin;
        b = branch_origin + branch_origin * my_branch.min_branch_length;
        c = GameManager.tree_origin;

        branch_plane.Set3Points(a, b, c);
    }

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

    public void HandleMouseHeld()
    {
        if (IsSelected())
        {
            Vector3 raw_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist_from_cam);
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(raw_mouse_pos);

            // Get closest point on the line extending from branch origin in branch direction
            Vector3 knob_pos = GetClosestPointOnLine(mouse_pos);

            // mouse_pos.z = 0;
            // transform.position = knob_pos; 
            // Debug.Draw
            // float dist_to_origin = (knob_pos - branch_origin).magnitude;
            // my_branch.SetLength(dist_to_origin);

            red_gizmo_pos = mouse_pos;
            mag_gizmo_pos = knob_pos;
        }
    }

    public void HandleMouseUp()
    {
        if (IsSelected())
        {
            selected = false;
        }
    }

    Vector3 GetClosestPointOnLine(Vector3 mouse_in)
    {
        return Vector3.ProjectOnPlane(mouse_in, branch_plane.normal);

        // return Vector3.Project(mouse_in, branch_origin + branch_direction);
    }


    bool IsSelected()
    {
        return selected;
    }

    Vector3 red_gizmo_pos;
    Vector3 mag_gizmo_pos;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(red_gizmo_pos, .3f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(mag_gizmo_pos, .3f);

        // Debug.Log("Drawing");

    }

}
