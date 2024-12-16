using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
    {
        private float moveTimer;
        private float LosePlayerTimer;
        private float shotTimer;

        public override void Enter() { }

        public override void Exit() { }

        public override void Perform() { 

            if(enemy.CanSeePlayer()){
                LosePlayerTimer = 0;
                moveTimer += Time.deltaTime;
                shotTimer += Time.deltaTime;
                enemy.transform.LookAt(enemy.Player.transform);

                if(shotTimer > enemy.fireRate){

                    Shoot();
                }

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

        public void Shoot(){

            //Guardar a referencia para o gunbarrel
            Transform gunbarrel = enemy.gunBarrel;

            //iniciar uma nova bullet
            GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject,gunbarrel.position,enemy.transform.rotation);
            
            //calcular a direção do Player
            Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

            //adicionar força para a bullet
            bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-1f,1f), Vector3.up) * shootDirection * 40;

            Debug.Log("Shoot");
            shotTimer = 0;

        }

        void Start() { }

        void Update() { }
    }