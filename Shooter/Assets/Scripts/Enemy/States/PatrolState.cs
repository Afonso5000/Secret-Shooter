using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float WaitTimer;

    public override void Enter(){

    }
    public override void Perform(){
        PatrolCycle();
        if(enemy.CanSeePlayer()){

            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit(){

    }

    public void PatrolCycle(){

        WaitTimer += Time.deltaTime;
        if (WaitTimer > 3){

        if(enemy.Agent.remainingDistance < 0.2f){
            if(waypointIndex < enemy.path.waypoints.Count -1){
                waypointIndex++ ;
            }else{
                waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                WaitTimer = 0;
            }

        }
    }
  }
}
