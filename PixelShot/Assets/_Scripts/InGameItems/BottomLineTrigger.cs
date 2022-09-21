using UnityEngine;

public class BottomLineTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        LevelItemMonoBase levelItemMonoBase = gameObject.GetComponent<LevelItemMonoBase>();

        if (levelItemMonoBase != null)
        {
            levelItemMonoBase.RunDeathEffectBehaviours();
        }
        
        other.gameObject.SetActive(false);

      
    }
}