#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LoopType = DG.Tweening.LoopType;
using EaseType = DG.Tweening.Ease;

namespace LevelEditor
{
    [ExecuteInEditMode]
    public class AnimationManager : MonoBehaviour
    {
        private const float POSITION_Y = 0.2f;

        [SerializeField] private GameObject map;
        [SerializeField] private GameObject animationParentPrefab;

        [Header("Animation Info")]
        [SerializeField] private bool isTurnMouseTrackForParent = false;
        [SerializeField] public AnimationType animationType;
        [SerializeField] public List<Vector3> animationPoints;
        [Range(0, 5f)] public float animationDuration = 3;
        [SerializeField] private Vector3 animationParentLocation;
        [SerializeField] public LoopType loopType = LoopType.Yoyo;
        [SerializeField] public EaseType easeType = EaseType.Flash;

        private Transform[] _transforms;
        private List<int> pixelIDs;

        public List<AnimationData> animationDatas;

        private void Start()
        {
            animationDatas = new List<AnimationData>();
            DeclareNewAnimationParentVariables();
        }

        void Update()
        {
            _transforms = Selection.transforms;
            if (_transforms.Length == 0)
                return;

            if (isTurnMouseTrackForParent)
            {
                SetAnimationPositions();
            }
        }

        private void SetAnimationPositions()
        {
            Vector3 mousePos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 position = hit.point;
                position.y = POSITION_Y;

                if (Input.GetMouseButtonDown(0))
                {
                    animationParentLocation = position;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    animationPoints.Add(position);
                }
            }
        }

        public void CreateAnimationParent()
        {
            GameObject parent = Instantiate(animationParentPrefab);
            AnimationBehaviour behaviour = parent.GetComponent<AnimationBehaviour>();

            behaviour.animationType = animationType;
            behaviour.animationPoints = animationPoints;
            behaviour.animationDuration = animationDuration;
            behaviour.animationParentLocation = animationParentLocation;
            behaviour.loopType = loopType;
            behaviour.easeType = easeType;

            parent.transform.position = animationParentLocation;
            parent.transform.SetParent(map.transform);

            foreach (Transform animatedObject in _transforms)
            {
                animatedObject.SetParent(parent.transform);
                pixelIDs.Add(animatedObject.GetComponent<IDStore>().ID);

            }
            CreateAnimationData();
            DeclareNewAnimationParentVariables();
            behaviour.Invoke();
        }

        private void CreateAnimationData()
        {
            AnimationData animationData = new AnimationData();

            animationData.animationType = animationType;
            animationData.animationPoints = animationPoints;
            animationData.animationDuration = animationDuration;
            animationData.animationParentLocation = animationParentLocation;
            animationData.loopType = loopType;
            animationData.easeType = easeType;
            animationData.pixelIDs = pixelIDs;

            animationDatas.Add(animationData);
        }

        private void DeclareNewAnimationParentVariables()
        {
            animationPoints = new List<Vector3>();
            pixelIDs = new List<int>();
        }

    }

}
#endif