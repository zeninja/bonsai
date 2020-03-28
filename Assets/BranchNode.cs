using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchNode : MonoBehaviour
{
    public Branch branch_prefab;

    Vector3 node_position;
    Branch parent_branch;
    Branch child_branch;
    float spawn_percent;

    public void SpawnBranch()
    {
        if (child_branch == null)
        {
            child_branch = Instantiate(branch_prefab);
            child_branch.SetOrigin(this, parent_branch.direction, ChooseRandomBranchDirection());
        }
    }

    public void SetNodeBranch(Branch b)
    {
        parent_branch = b;
    }

    Vector3 ChooseRandomBranchDirection()
    {
        List<Vector3> directions = parent_branch.GetDirectionList();
        return (directions[Random.Range(0, directions.Count)]);
    }

    public void HandleParentBranchRemoved()
    {
        if (child_branch != null)
        {
            child_branch.KillBranch();
        }
        Destroy(gameObject);
    }
}
