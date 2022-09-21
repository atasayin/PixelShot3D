using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : LevelItemMonoBase
{
    [SerializeField] private Vector3 _ballStartPosition;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private string collisionLayer;
    [SerializeField] private string unCollisionLayer;

    [SerializeField] private AudioStruct deathAudio;

    private Pool_BallDies _pool_BallDies;

    protected override void Awake()
    {
        base.Awake();
        poolables = new List<IPoolable>();
        
        originalColor = meshRenderer.sharedMaterial.color;
        poolables.Add(new Pool_NonUseRigidbody(rigidbody));
        poolables.Add(new Pool_NonUseTransform(transform,_ballStartPosition));

        _pool_BallDies = new Pool_BallDies();
        poolables.Add(_pool_BallDies);

        deathEffects.Add(new DeathVibration(this));
        deathEffects.Add(new DeathSound(deathAudio, transform.position));

    }

    public void MakeUltBall()
    {
        if (poolables.Count == 3)
        {
            poolables.RemoveAt(2);
        }
    }

    public void MakeNormalBall()
    {
        if (poolables.Count == 2)
        {
            poolables.Add(_pool_BallDies);
        }

        
    }


    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.BallTouchWallTouch();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _trailRenderer.Clear();
    }  

    public void EnableCollisonWithInGameObject() => gameObject.layer = LayerMask.NameToLayer(collisionLayer);


    public void DisableCollisonWithInGameObject() => gameObject.layer = LayerMask.NameToLayer(unCollisionLayer);



}
