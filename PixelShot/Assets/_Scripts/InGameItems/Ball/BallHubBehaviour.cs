using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BallHubBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject staticHub;
    [SerializeField] private GameObject dynamicHub;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Material hubMaterial;
    [SerializeField] private Material dragBackgroundMaterial;
    [SerializeField] private Transform dragBackgroundTransform;


    private const float STRETCHING_COEF = 0.05f;

    private Vector3 _dragDirection;
    
    private Transform _staticHubTransform;
    private Transform _dynamicHubTransform;
    private Vector3 _staticHubStartScale;
    private DragAndShootBehaviour _dragNShoot;
    private float _maxDragMagnitude;
    private float _dragMagnitude;
    private Vector3 _startPosition;
    
    private GameObject _activeBall;
    private Transform ballTransform;
    private Vector3 dragVector;
    private Vector3 _offsetFromBall;
    private int lineSize;
    private Gradient lineRendererColor;
    private float _middleKeyValue;
    private const float RELEASE_ANIMATION_DURATION = 0.35f;
    private bool _isTouch;
    

    private void Awake()
    {
        _staticHubTransform = staticHub.transform;
        _staticHubStartScale = _staticHubTransform.localScale;
        _dynamicHubTransform = dynamicHub.transform;
        
        lineSize = _lineRenderer.positionCount;
        _offsetFromBall = new Vector3(0, -0.05f, 0);

        
    }

    private void OnEnable()
    {
        _isTouch = false;
        _lineRenderer.positionCount = 0;
        _lineRenderer.positionCount = 10;
        DragAndShootBehaviour.OnBallThrow += ReleaseAnimations;
    }

    private void OnDisable()
    {
        DragAndShootBehaviour.OnBallThrow -= ReleaseAnimations;
    }

    public void SetUpBallHub(Color color)
    {
        _startPosition = _staticHubTransform.position;
        ColorHub(color);
        _middleKeyValue = 0.5f;
        lineSize = _lineRenderer.positionCount;
        _lineRenderer.enabled = true;
    }

    private void ColorHub(Color color)
    {
        hubMaterial.color = color;

        lineRendererColor = new Gradient();
        Utility.CreateMainColorGradient(lineRendererColor, color, 1, 1);
        _lineRenderer.colorGradient = lineRendererColor;
        
    }

    public void ArrangeBall(GameObject activeBall)
    {
        _activeBall = activeBall;
        _dragNShoot = _activeBall.GetComponent<DragAndShootBehaviour>();
        _maxDragMagnitude = _dragNShoot.MaxDragMag;

        _lineRenderer.enabled = true;
    }



    public void ReleaseAnimations()
    {
        FadeOutLineRenderer();
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (_dragNShoot.IsThrew)
        {
            return;
        }

        PlayFirstTouchSound();

        _dragMagnitude = _dragNShoot.DragVector.magnitude;
        ballTransform = _activeBall.transform;

        UpdateHubsTransformsAccordingToDragPower();
        UpdateLineRendererPoisitions();
        UpdateAnimationCurveLineRenderer();
        UpdateDragBackground();
    }

    private void PlayFirstTouchSound()
    {
        if (!_isTouch && _dragNShoot.DragVector.magnitude > float.Epsilon)
        {
            _isTouch = true;
            AudioManager.Instance.BallHubFirstTouch();
        }

        if (_isTouch && _dragNShoot.DragVector.magnitude <= 0.5f)
        {
            _isTouch = false;
        }
    }

    private void UpdateHubsTransformsAccordingToDragPower()
    {
        _dynamicHubTransform.position = ballTransform.position + _offsetFromBall;

        float ratio = _dragMagnitude / _maxDragMagnitude;
        float scaler = ratio >= 0.5f ? 1.5f - (_dragMagnitude / _maxDragMagnitude) : 1;
        _staticHubTransform.localScale = (_staticHubStartScale) * scaler ;

        // shake      
        if ((_dragMagnitude / _maxDragMagnitude) > 0.9f)
        {
          //  transform.DOShakePosition(0.5f, new Vector3(0.01f ,0, 0.01f), 1, 1, false, true).SetLoops(1,LoopType.Yoyo).
            //    OnComplete(() => transform.position = _startPosition);
            
            

        }
        

      

    }

    private void UpdateLineRendererPoisitions()
    {
        _lineRenderer.positionCount = lineSize;

        Vector3 start = new Vector3(0, 0.2f, 0);
        Vector3 dest = _dynamicHubTransform.position;
        dest.y = 0.2f;
        Vector3 x = (dest - start);

        Vector3 direction = x.normalized;
        float unitMagnitude = x.magnitude / lineSize;
        Vector3 currentPoint = start;

        for(int i = 0; i < lineSize; i++)
        {
            _lineRenderer.SetPosition(i, currentPoint + direction * unitMagnitude * i);
        }
    }



    private void UpdateAnimationCurveLineRenderer()
    {
        AnimationCurve myAnimationCurve = _lineRenderer.widthCurve;

        Keyframe[] myKeys = new Keyframe[myAnimationCurve.keys.Length];

        float middleRatio = (2f - (_dragMagnitude / _maxDragMagnitude)) * _middleKeyValue;
        
        if (middleRatio > 2.5f)
        {
            middleRatio = 2.5f;
        }


        myKeys[0] = new Keyframe(0f, 1);
        myKeys[1] = new Keyframe(0.5f, middleRatio);
        myKeys[2] = new Keyframe(1f, 1);

        myAnimationCurve.keys = myKeys;

        _lineRenderer.widthCurve = myAnimationCurve;
    }

    private void UpdateDragBackground()
    {
        if (_dragMagnitude > 5f)
        {
            dragBackgroundTransform.localScale = Vector3.one * (_dragMagnitude / _maxDragMagnitude) * 1.25f;

        }
    }

    private async void FadeOutLineRenderer()
    {
        List<Task> tasks = new List<Task>();

       
        //for (int i = 0; i < lineSize; i++)
        //{
        //    tasks.Add(
        //        DOTween.To(() => _lineRenderer.GetPosition(i), (x)
        //                        => _lineRenderer.SetPosition(i, x),
        //                        new Vector3(0, 0.2f, 0),
        //                        RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart());
        ///*
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(0), (x)
                              => _lineRenderer.SetPosition(0, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );

            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(1), (x)
                              => _lineRenderer.SetPosition(1, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(2), (x)
                              => _lineRenderer.SetPosition(2, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(3), (x)
                              => _lineRenderer.SetPosition(3, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(4), (x)
                              => _lineRenderer.SetPosition(4, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(5), (x)
                              => _lineRenderer.SetPosition(5, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(6), (x)
                              => _lineRenderer.SetPosition(6, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(7), (x)
                              => _lineRenderer.SetPosition(7, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(8), (x)
                              => _lineRenderer.SetPosition(8, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
            tasks.Add(
                DOTween.To(() => _lineRenderer.GetPosition(9), (x)
                              => _lineRenderer.SetPosition(9, x),
                              new Vector3(0, 0.2f, 0),
                              RELEASE_ANIMATION_DURATION).Play().AsyncWaitForStart()
                      );
        //*/

        //}


        tasks.Add(
            _staticHubTransform.DOScale(_staticHubStartScale, RELEASE_ANIMATION_DURATION / 2).SetEase(Ease.OutBounce).AsyncWaitForStart()
            );

        tasks.Add(
            _dynamicHubTransform.DOMove(_startPosition, RELEASE_ANIMATION_DURATION / 2).SetEase(Ease.OutBounce).AsyncWaitForStart()
            );


        
        await Task.WhenAll(tasks);
    }


    public GameObject ActiveBall
    {
        get { return _activeBall; }
        set { _activeBall = value; }
    }

    public DragAndShootBehaviour DragNShoot
    {
        get { return _dragNShoot; }
        set { _dragNShoot = value; }
    }

    public float DragMagnitude
    {
        get { return _dragMagnitude; }
        set { _dragMagnitude = value; }
    }


}
