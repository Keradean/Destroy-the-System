using UnityEngine;

public abstract class EnemySMBase : StateMachineBehaviour
{
    protected EnemyControllerBase Controller { get; private set; }
    public void Initialize(EnemyControllerBase controller)
    {
        Controller = controller;
    }

    public virtual void OnAnimationEvent(string eventName) {}
}