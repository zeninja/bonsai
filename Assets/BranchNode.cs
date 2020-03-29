using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchNode : MonoBehaviour
{
    // public Branch branch_prefab;

    // Vector3 node_position;
    Branch parent_branch;
    Branch child_branch;

    public void DoUpdate()
    {
        if (child_branch != null)
        {
            child_branch.DoUpdate();
        }
    }

    public void SetParentBranch(Branch b)
    {
        parent_branch = b;
    }

    public void SetChildBranch(Branch branch)
    {
        child_branch = branch;
    }

    public void HandleParentBranchRemoved()
    {
        if (child_branch != null)
        {
            child_branch.KillBranch();
        }
        Destroy(gameObject);
    }

    public bool HasParent()
    {
        return parent_branch != null;
    }
}
