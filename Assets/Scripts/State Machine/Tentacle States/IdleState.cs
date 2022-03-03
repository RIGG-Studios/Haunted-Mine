using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(TentacleController controller) : base("Idle", controller) => this.controller = controller;

    public override void EnterState()
    {
        controller.InitializeTentacles();
    }

    public override void UpdateLogic()
    {
        controller.SetAgentPosition(Vector3.zero);
    }

}