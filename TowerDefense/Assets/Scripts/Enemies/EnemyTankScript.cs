using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankScript : EnemyScript
{
    protected override void OnDisable()
    {
        if (op != null)
        {
            op.ReturnEnemyTank(this.gameObject);
        }
    }
}
