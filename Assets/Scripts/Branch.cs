using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    BranchNode origin_node;
    public BranchKnob knob;
    public BranchGFX gfx;

    public Vector3 origin;
    public Vector3 direction;
    public float length;
    Vector3 extent;

    float min_length = 2;
    float max_length = 15f;

    public BranchNode node_prefab;
    List<BranchNode> child_nodes = new List<BranchNode>();
    int node_count = 3;

    public int depth;


    List<float> tilt_amount = new List<float> { 0f, -15f, -30f, -45f, -60f };
    public float tilt;

    // List<float> lean_amount = new List<float> { 0, 15, 30, 45, 60 };
    // public float lean;


    #region start
    public void OnCreated(BranchNode node, Vector3 new_direction, Vector3 old_direction, int branch_depth)
    {
        origin_node = node;
        origin = origin_node.transform.position;
        direction = new_direction;

        length = min_length;

        depth = branch_depth;

        InitBranch();
        SetRotation(old_direction);
        SetLength(length);
        SetTilt();
    }

    void InitBranch()
    {
        for (int i = 0; i < node_count; i++)
        {
            BranchNode new_node = Instantiate(node_prefab);

            child_nodes.Add(new_node);
            new_node.SetParentBranch(this);
            GameManager.GetInstance().HandleNodeAdded(new_node);
        }

        GameManager.GetInstance().HandleBranchAdded(this);
    }
    #endregion

    public void DoUpdate()
    {
        SetPosition();
        SetNodePositions();
        SetKnobPosition();
    }


    // Branch
    // [external]
    public void SetLength(float new_length)
    {

        length = new_length;
        length = Mathf.Clamp(length, min_length, max_length);

        gfx.transform.localScale = new Vector3(1, 1, length / 2);
    }

    // [internal]
    void SetPosition()
    {
        origin = origin_node.transform.position;
        transform.position = origin;
    }

    void SetRotation(Vector3 old)
    {
        gfx.transform.rotation = Quaternion.LookRotation(direction, old);
    }

    void SetTilt()
    {
        if (HasParent())
        {
            // Vector3 rot = gfx.transform.eulerAngles;
            // rot.x = tilt_amount[Random.Range(0, tilt_amount.Count)];
            // gfx.transform.eulerAngles += rot;

            tilt = tilt_amount[Random.Range(0, tilt_amount.Count)];
            Vector3 rot = new Vector3(tilt, 0, 0);

            gfx.transform.Rotate(rot);

            direction = gfx.transform.forward;
            tilt = rot.x;
        }
    }

    // Nodes
    void SetNodePositions()
    {
        float node_spread_distance = length / (node_count + 1);

        for (int i = 0; i < child_nodes.Count; i++)
        {
            Vector3 node_vector = direction.normalized * node_spread_distance * (i + 1);
            child_nodes[i].transform.position = origin + node_vector;
            // Debug.Log("Got node vector: " + node_vector);
        }
    }


    // Knob
    void SetKnobPosition()
    {
        knob.transform.position = origin + direction * length;
        knob.DoUpdate();
    }


    // Children and cleanup
    public Vector3 GetRandomDirection()
    {
        List<Vector3> directions = new List<Vector3>() { gfx.transform.right, -gfx.transform.right };
        Vector3 direction = (directions[Random.Range(0, directions.Count)]);

        // Debug.Log("New branch direction: " + direction);
        return direction;
    }

    public void KillBranch()
    {
        foreach (BranchNode n in child_nodes)
        {
            n.HandleParentKilled();
        }
        GameManager.GetInstance().HandleBranchRemoved(this);
        Destroy(gameObject);
    }


    // external info
    public Vector3 GetExtent()
    {
        extent = origin + direction * length;
        return extent;
    }

    public bool HasParent()
    {
        return origin_node.HasParent();
    }


    public void Grow(float speed = 1)
    {
        length += speed * Time.fixedDeltaTime;
        SetLength(length);
    }

    public void Shrink(float speed = 1)
    {
        // tbh, unnecessary
        length -= speed * Time.fixedDeltaTime;
        SetLength(length);
    }

}
