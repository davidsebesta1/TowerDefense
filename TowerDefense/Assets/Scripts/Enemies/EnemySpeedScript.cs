using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedScript : EnemyScript
{
    protected override void OnDisable()
    {
        if (op != null)
        {
            op.ReturnEnemySpeed(this.gameObject);
        }
    }
}
