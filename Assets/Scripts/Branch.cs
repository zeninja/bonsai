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

    float min_length = 2;

    public BranchNode node_prefab;
    List<BranchNode> child_nodes = new List<BranchNode>();
    int node_count = 1;

    public void OnCreated(BranchNode node, Vector3 new_direction, Vector3 old_direction)
    {
        origin_node = node;
        origin = origin_node.transform.position;
        direction = new_direction;

        length = min_length;

        InitBranch();
        SetRotation(old_direction);
        SetLength(length);
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
    }

    public void DoUpdate()
    {
        SetPosition();
        SetNodePositions();
        SetKnobPosition();

        // FindMyPlane();
    }

    public void SetLength(float new_length)
    {
        length = new_length;
        length = Mathf.Max(length, min_length);

        gfx.transform.localScale = new Vector3(1, 1, length / 2);
    }

    void SetPosition()
    {
        origin = origin_node.transform.position;
        transform.position = origin;
    }

    void SetRotation(Vector3 old)
    {
        gfx.transform.rotation = Quaternion.LookRotation(direction, old);
    }

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

    void SetKnobPosition()
    {
        knob.transform.position = origin + direction * length;
    }

    public Vector3 GetRandomDirection()
    {
        List<Vector3> directions = new List<Vector3>() { gfx.transform.up, -gfx.transform.up };
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
        Destroy(gameObject);
    }
}
