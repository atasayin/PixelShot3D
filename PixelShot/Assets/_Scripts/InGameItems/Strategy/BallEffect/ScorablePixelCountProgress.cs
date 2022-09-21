using UnityEngine;

public class ScorablePixelCountProgress : IBallEffect
{
    public delegate void PixelDies();
    public static event PixelDies OnPixelDies;


    public void CollisionEffect(Collider collider)
    {
        LevelManager.Instance.ScoreLevelNumberAmountOnHit();
        OnPixelDies?.Invoke(); 

    }
}
