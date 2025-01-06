namespace EnemyAI{

public abstract class BaseState
    {
        public StateMachine stateMachine;
        public Enemy enemy;

        /*public float searchTimer;*/

        public abstract void Enter();
        public abstract void Perform();
        public abstract void Exit();
    }
}