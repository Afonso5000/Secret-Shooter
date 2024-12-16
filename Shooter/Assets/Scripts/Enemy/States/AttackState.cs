using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
    {
        private float moveTimer;
        private float LosePlayerTimer;

        public override void Enter() { }

        public override void Exit() { }

        public override void Perform() { 

            if(enemy.CanSeePlayer()){
                LosePlayerTimer = 0;
                moveTimer += Time.deltaTime;

                if(moveTimer > Random.Range(3,7)){

                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                    moveTimer = 0;

                }
            }else{

                LosePlayerTimer += Time.deltaTime;
                if(LosePlayerTimer > 8){

                    //voltar para o estado de procura
                    stateMachine.ChangeState(new PatrolState());
                }
            }
        }

        void Start() { }

        void Update() { }
    }