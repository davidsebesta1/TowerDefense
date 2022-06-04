using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicScript : EnemyScript
{

    protected override void OnDisable()
    {
        if (op != null)
        {
            op.ReturnEnemyBasic(this.gameObject);
        }
    }
}
