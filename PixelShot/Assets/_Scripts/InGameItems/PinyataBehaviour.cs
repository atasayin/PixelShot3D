using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PinyataBehaviour : MonoBehaviour
{
    private const float DIAMOND_POSITION_OFFSET = 1;
    private const float DIAMOND_FORCE_OFFSET = 3;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private MeshRenderer pinyataMesh;
    [SerializeField] private ParticleSystem innerConfetti;

    [Header("Rope Info")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform jointTransform;
    [SerializeField] private Transform pinyataTopTransform;

    private Camera _cameraMain;

    private int _tapCount;
    private bool isExplode;

    private int totalDiamond;

    private void Awake()
    {
        _cameraMain = Camera.main;
        totalDiamond = GameManager.DIAMOND_FOR_BONUS_LEVEL;
    }

    private void OnEnable()
    {
        ResetPinayta();
        ObjectManager.Instance.SetPinyataCrackMaterialBaseMap(_tapCount++);
    }

    private void ResetPinayta()
    {
        pinyataMesh.enabled = true;
        lineRenderer.positionCount = 10;
        _tapCount = 0;
        isExplode = false;
    }

    private void Update()
    {
        UpdateRope();
        
    }

    private void OnMouseOver()
    {   
        if (!isExplode)
        {    
            if (Input.GetMouseButtonDown(0))
            {
                TapForceToPinyata();
                ChangeCrackMaterial();
            }
        }
        
    }

    private void TapForceToPinyata()
    {
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 force = hit.normal * 3;
            force.y = 0;
            rigidBody.AddForce(force, ForceMode.Impulse);
        }
    }

    private void ChangeCrackMaterial()
    {
        if (_tapCount == 4)
        {
            isExplode = true;
            lineRenderer.positionCount = 0;

            ExplosionAnimation();
           
        }
        else
        {
            StartCoroutine(HitAninationCoroutine(.02f));
            ObjectManager.Instance.SetPinyataCrackMaterialBaseMap(_tapCount++);
        }
    }

    private void ExplosionAnimation()
    {
        if (innerConfetti.isPlaying)
        {
            innerConfetti.Stop();
        }

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.5f, .3f));
        sequence.Append(transform.DOScale(0.8f, .3f));
        sequence.Append(transform.DOScale(1, .3f));
        sequence.OnComplete(() => {
            pinyataMesh.enabled = false;
            ReleaseDiamonds();
            innerConfetti.Play();
            GameManager.Instance.GoToEndGameBonus();
        }
        );
    }


    private void ReleaseDiamonds()
    {
        Vector3 startPosition = transform.position;
       for(int i = 0; i < totalDiamond; i++)
       {
            GameObject diamond = ObjectManager.Instance.GetDiamondObjectFromPool();
            diamond.transform.rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.RandomRange(0, 360), 0));
            Rigidbody rigidbody = diamond.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;

            Vector3 randomPosition = Utility.GetRandomVector3(-DIAMOND_POSITION_OFFSET, DIAMOND_POSITION_OFFSET);
            randomPosition.y = 0;
            
            Vector3 randomForce = Utility.GetRandomVector3(-DIAMOND_FORCE_OFFSET, DIAMOND_FORCE_OFFSET);
            randomForce.y = 0;
            
            diamond.transform.position = startPosition + randomPosition;
            rigidbody.AddForce(randomForce, ForceMode.Impulse);
            
        }

    }

    private void UpdateRope()
    {
        int lineSize = lineRenderer.positionCount;
        Vector3 startPosition = jointTransform.position;
        Vector3 endPosition = pinyataTopTransform.position;
        Vector3 x = (endPosition - startPosition);

        Vector3 direction = x.normalized;
        float unitMagnitude = x.magnitude / lineSize;
        Vector3 currentPoint = startPosition;

        for (int i = 0; i < lineSize; i++)
        {
            lineRenderer.SetPosition(i, currentPoint + direction * unitMagnitude * i);
        }
    }

    private IEnumerator HitAninationCoroutine(float sec)
    {
        ObjectManager.Instance.SetPinyataBaseMaterialHit();
       yield return new WaitForSeconds(sec);
        ObjectManager.Instance.SetPinyataBaseMaterialRainBow();
    }

}
