public class Pool_BallDies : IPoolable
{
    public delegate void BallDies();
    public static event BallDies OnBallDie;

    public void GetPooledObject()
    {
        
    }

    public void ReturnToPool()
    {
        OnBallDie?.Invoke();
    }
}
