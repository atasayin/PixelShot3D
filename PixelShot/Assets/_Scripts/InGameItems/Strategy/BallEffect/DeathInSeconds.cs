using System.Collections;
using UnityEngine;


public class DeathInSeconds : IBallEffect
{
    private GameObject _gameObject;
    private MonoBehaviour _monoBehaviour;
    private LevelItemMonoBase _levelItemMonoBase;

    public DeathInSeconds(GameObject gameObject, MonoBehaviour monoBehaviour)
    {
        _gameObject = gameObject;
        _monoBehaviour = monoBehaviour;
        _levelItemMonoBase = gameObject.GetComponent<LevelItemMonoBase>();
    }

    public void CollisionEffect(Collider collider)
    {
        _monoBehaviour.StartCoroutine(WaitForDeath(5.0f));
    }


    private IEnumerator WaitForDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (_levelItemMonoBase != null)
        {
            _levelItemMonoBase.RunDeathEffectBehaviours();
        }

        _gameObject.SetActive(false);

    }


}
