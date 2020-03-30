using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchKnob : MonoBehaviour
{
    public Branch my_branch;
    public bool selected = false;

    bool IsSelected()
    {
        return selected;
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
            float dist_from_cam = (Camera.main.transform.position - transform.position).magnitude;
            Vector3 raw_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist_from_cam);
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(raw_mouse_pos);

            // Get closest point on the line extending from branch origin in branch direction
            // Vector3 plane_pos = branch_plane.ClosestPointOnPlane(mouse_pos);

            Vector3 knob_pos = Vector3.Project(mouse_pos, my_branch.origin + my_branch.direction);
            float branch_length = (knob_pos - my_branch.origin).magnitude;

            my_branch.SetLength(branch_length);



            red_gizmo_pos = raw_mouse_pos;
            mag_gizmo_pos = mouse_pos;
            // blu_gizmo_pos = 

        }
    }

    public void HandleMouseUp()
    {
        if (IsSelected())
        {
            selected = false;
        }
    }

    Vector3 red_gizmo_pos;
    Vector3 mag_gizmo_pos;
    Vector3 blu_gizmo_pos;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(red_gizmo_pos, .5f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(mag_gizmo_pos, .2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(blu_gizmo_pos, .1f);
    }

}
