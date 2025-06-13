using UnityEngine;

public interface IMonsterState
{
    void Enter();
    void Update();
    void Exit();
}

public abstract class MonsterStateBase : IMonsterState
{
    protected MonsterBase monster;
    protected Animator animator;

    public MonsterStateBase(MonsterBase monster, Animator animator)
    {
        this.monster = monster;
        this.animator = animator;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
