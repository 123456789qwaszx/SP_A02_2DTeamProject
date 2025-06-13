public class MonsterStateMachine
{
    private IMonsterState currentState;

    public void Initialize(IMonsterState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
