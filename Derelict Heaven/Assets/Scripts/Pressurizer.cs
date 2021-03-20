using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressurizer : Enemy
{
    public override GameManager.Form GetForm()
    {
        return GameManager.Form.pressurizer;
    }

    public override void ResetState()
    {
        // nothing
    }

    protected override void EnemyPhysicsUpdate()
    {
        // nothing
    }

    protected override void EnemyStart()
    {
        // nothing
    }

    protected override void EnemyUpdate()
    {
        // nothing
    }
}
