using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public BranchNode branch_node_prefab;
    public float branch_length;
    public Vector3 direction;
    public float min_branch_length;

    BranchNode origin_node;
    BranchKnob knob;

    List<BranchNode> child_nodes = new List<BranchNode>();
    Transform graphics;
    int node_count = 1;

    public Branch branch_prefab;

    // void Awake()
    // {

    // }

    void Start()
    {
        InitBranch();
        InitNodes();
        InitKnob();
        SetNodePositions();
    }

    public void DoUpdate()
    {
        UpdateBranch();
        SetNodePositions();
        UpdateNodes();

        direction = graphics.forward;
    }

    void InitBranch()
    {
        branch_length = transform.localScale.z;
        graphics = transform.Find("GFX");
    }

    void InitKnob() {
        knob = GetComponentInChildren<BranchKnob>();
        knob.InitKnob();
    }

    public void SpawnBranch()
    {
        foreach (BranchNode node in child_nodes)
        {
            Branch new_branch = Instantiate(branch_prefab);
            new_branch.SetOrigin(node, direction, ChooseRandomBranchDirection());
            node.SetChildBranch(new_branch);
        }
    }

    void InitNodes()
    {
        Debug.Log("Initting branch");

        Transform node_parent = transform.Find("Nodes");
        if (!node_parent.gameObject.activeSelf) { return; }

        for (int i = 0; i < node_count; i++)
        {
            Debug.Log(node_count);
            BranchNode n = Instantiate(branch_node_prefab);
            n.SetParentBranch(this);

            child_nodes.Add(n);

            // n.transform.SetParent(node_parent);
        }
    }

    void SetNodePositions()
    {
        float node_spread_distance = branch_length / (node_count + 1);

        for (int i = 0; i < child_nodes.Count; i++)
        {
            child_nodes[i].transform.position = transform.position + direction * branch_length / 2 + direction.normalized * node_spread_distance * (i + 1);
        }
    }

    void UpdateNodes()
    {
        foreach (BranchNode n in child_nodes)
        {
            n.DoUpdate();
        }
    }

    public void SetOrigin(BranchNode node, Vector3 old_branch_direction, Vector3 new_branch_direction)
    {
        origin_node = node;
        transform.position = node.transform.position;
        graphics.rotation = Quaternion.LookRotation(new_branch_direction, old_branch_direction);
        direction = new_branch_direction;
    }

    public void SetLength(float new_length)
    {
        branch_length = new_length / 2;
        branch_length = Mathf.Max(branch_length, min_branch_length);

        graphics.localScale = new Vector3(1, 1, branch_length);
    }

    void UpdateBranch()
    {
        if (origin_node == null) { return; }
        transform.position = origin_node.transform.position;

    }

    Vector3 ChooseRandomBranchDirection()
    {
        List<Vector3> directions = new List<Vector3>() { graphics.transform.up, -graphics.transform.up };
        return (directions[Random.Range(0, directions.Count)]);
    }

    public void KillBranch()
    {
        foreach (BranchNode n in child_nodes)
        {
            n.HandleParentBranchRemoved();
        }
        Destroy(gameObject);
    }

    public bool HasParent()
    {

        if (origin_node != null)
        {
            return origin_node.HasParent();
        }
        else
        {
            return false;
        }
    }
}
