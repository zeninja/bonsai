using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchNode : MonoBehaviour
{

    public Branch branch_prefab;

    public Branch parent_branch;
    public Branch child_branch;

    int depth = 0;

    public void DoUpdate()
    {
        if (child_branch != null)
        {
            child_branch.DoUpdate();
        }
    }

    public void SpawnBranch()
    {
        if (child_branch == null)
        {
            Branch b = Instantiate(branch_prefab);
            Vector3 branch_dir = HasParent() ? parent_branch.GetRandomDirection() : Vector3.up;
            Vector3 from_dir = HasParent() ? parent_branch.direction : Vector3.zero;

            b.OnCreated(this, branch_dir, from_dir, depth + 1);

            child_branch = b;
        }
    }

    public void SetParentBranch(Branch branch) {
        transform.parent = branch.transform.Find("Nodes");
        parent_branch = branch;
    }

    public bool HasParent()
    {
        return parent_branch != null;
    }

    public void HandleParentKilled()
    {
        if (child_branch != null)
        {
            child_branch.KillBranch();
        }
        GameManager.GetInstance().HandleNodeRemoved(this);
        Destroy(gameObject);
    }
}
