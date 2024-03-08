public abstract class BaseState
{

    protected Enemy CurrentEnemy;
    public abstract void OnEnterState(Enemy enemy);
    /// <summary>
    /// 逻辑判断
    /// </summary>
    public abstract void LogicUpdate();
    /// <summary>
    /// 物理判断
    /// </summary>
    public abstract void PhysicsUpdate();

    public abstract void OnExitState();
}
