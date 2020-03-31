using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchKnob : MonoBehaviour
{
    public Branch my_branch;
    public bool selected = false;

    MeshRenderer renderer;

    public Color selected_color, deselected_color;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void DoUpdate()
    {
        renderer.material.color = IsSelected() ? selected_color : deselected_color;
    }

    bool IsSelected()
    {
        return selected;
    }

    public void HandleMouseDown()
    {
        // Kill branch
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
            Vector3 final_point = Vector3.zero;

            // #region old
            // #endregion
            if (!my_branch.HasParent())
            {
                // Simple raycast
                // first branch has no parent
                float dist_from_cam = (Camera.main.transform.position - transform.position).magnitude;
                Vector3 raw_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist_from_cam);
                Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(raw_mouse_pos);

                final_point = mouse_pos;
            }
            else
            {
                // Raycast onto a plane
                // Plane is drawn from Knob - Branch Origin - Tree Origin

                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay(cam_ray.origin, cam_ray.direction * 40, Color.cyan, .5f);
                UpdatePlane();

                Vector3 plane_point = Vector3.zero;
                float enter = 0.0f;

                if (branch_plane.Raycast(cam_ray, out enter))
                {
                    plane_point = cam_ray.GetPoint(enter);
                    Debug.Log("hit plane");
                }
                DrawPlane(transform.position, branch_plane.normal, 1000f);

                // Vector3 plane_vector = plane_point - my_branch.origin;

                // Get closest point on the line extending from branch origin in branch direction
                // Vector3 projected_pos = Vector3.Project(plane_point, my_branch.origin + my_branch.direction);
                Vector3 projected_pos = FindNearestPointOnLine(my_branch.origin, my_branch.direction, plane_point);

                final_point = projected_pos;

                // debugs
                red_gizmo_pos = projected_pos;
                blu_gizmo_pos = plane_point;
            }

            float branch_length = (final_point - my_branch.origin).magnitude;
            my_branch.SetLength(branch_length);

            #region extra
            Debug.DrawRay(my_branch.transform.position, my_branch.direction * 100, Color.yellow);
            #endregion
        }
    }
    // Vector3 knob_pos = Vector3.ProjectOnPlane(mouse_pos, branch_plane.normal);



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
        if (IsSelected())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(red_gizmo_pos, .5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(mag_gizmo_pos, .2f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(blu_gizmo_pos, .1f);
        }
    }

    public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;

        float dotP = Vector3.Dot(lhs, direction);
        return origin + direction * dotP;
    }

    Plane branch_plane;

    void UpdatePlane()
    {
        branch_plane.Set3Points(transform.position, my_branch.origin, Vector3.zero);
    }

    void DrawPlane(Vector3 position, Vector3 normal, float size = 1)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        Vector3 corner0 = position + v3 * size;
        Vector3 corner2 = position - v3 * size;
        Quaternion q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        Vector3 corner1 = position + v3 * size;
        Vector3 corner3 = position - v3 * size;

        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);

    }
}
