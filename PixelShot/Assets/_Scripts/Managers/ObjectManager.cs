using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ObjectManager: AbstarctMonoSingleton<ObjectManager>
{
    #region Variables

    [SerializeField] private GameObject _map;
    [SerializeField] private Material levelBoardMaterial;
    [SerializeField] private Material playGroundMaterial;
    
    [Header("Pinyata Info")]
    [SerializeField] private List<Texture> crackTextures;
    [SerializeField] private Material crackMaterial;
    [SerializeField] private Texture hitTexture;
    [SerializeField] private Texture rainBowTexture;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Vector3 pinyataPos;
    

    [Header("Object Id's")]
    [SerializeField] private IntAsset _ballId;
    [SerializeField] private IntAsset deathParticleId;
    [SerializeField] private IntAsset deathScorePopUpId;
    [SerializeField] private IntAsset obstacleParentId;
    [SerializeField] private IntAsset animationParentId;
    [SerializeField] private IntAsset shinningId;
    [SerializeField] private IntAsset pinyataId;
    [SerializeField] private IntAsset diamondId;
    [SerializeField] private IntAsset audioObjectId;


    private GameObject _physicalSceneBall;

    private List<AnimationBehaviour> animationBehaviours;


    Vector3 _offset = new Vector3(0f, 0, 0f);

    private const float INTERVAL_COF = 0.85f;

    private Dictionary<int, Queue<GameObject>> _dictionary;
    private Queue<GameObject> loadedObjects;
    private const int PHYSICAL_SCENE_DOUBLE_OBJECT = 2;

    [System.Serializable]
    public class Pool
    {
        public IntAsset id;
        public int size;
        public GameObject prefab;
    }

    public List<Pool> pools;

    #endregion

    private void OnEnable()
    {
        SetDictForPools();

        animationBehaviours = new List<AnimationBehaviour>();
       // SeperatePhysicSceneBall();
    }
    
    public void InvokeAllAnimationBehaviours()
    {
        foreach(AnimationBehaviour behaviour in animationBehaviours)
        {
            behaviour.Invoke();
           
        }
    }


    public void FinishAllAnimationBehaviours()
    {
        foreach (AnimationBehaviour behaviour in animationBehaviours)
        {
            behaviour.CompleteAnimation();
           
        }

        animationBehaviours.Clear();
    }

    #region Object Pool



    private void SetDictForPools()
    {
        _dictionary = new Dictionary<int, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> gameObjects = new Queue<GameObject>();
            _dictionary.Add(pool.id.Value, gameObjects);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject gam = Instantiate(pool.prefab, transform);
                gam.SetActive(false);
                gameObjects.Enqueue(gam);
            }

        }

        loadedObjects = new Queue<GameObject>();
    }


    private void SeperatePhysicSceneBall()
    {
        _physicalSceneBall = GetPhysicalBallObjectFromPool();
    }
    

    public void ResetObjectPooler()
    {
        List<int> keys = new List<int>(_dictionary.Keys);

        foreach (int key in keys)
        {
            foreach (GameObject gameObject in _dictionary[key].ToArray())
            {
                if (key != _ballId.Value) // SOR ?? 
                {
                    gameObject.transform.SetParent(transform);
                }
                gameObject.SetActive(false);

            }
        }
    }


    public void ResetLoadedObjects()
    {
        while(loadedObjects.Count != 0)
        {
            GameObject gameObject = loadedObjects.Dequeue();
            gameObject.transform.SetParent(transform);
            gameObject.SetActive(false);
        }
    }




    public void IncrementPoolsOfNeed(Level level)
    {
        int[] countObjectIdMap = level.inGameObjectCountMapId;

        // id = 0 empty cell
        for(int id = 1; id < countObjectIdMap.Length; id++)
        {
            CheckPoolWithIDForDesiredAmout(id, countObjectIdMap[id]);  
        }

        CheckPoolWithIDForDesiredAmout(deathParticleId.Value, level.items.Count / 3);
        CheckPoolWithIDForDesiredAmout(deathScorePopUpId.Value, level.items.Count / 3);

    }
 
    private void CheckPoolWithIDForDesiredAmout(int id, int desiredAmount)
    {
        Pool poolToEnhance = GetPoolWithId(id);
        //int amount = PHYSICAL_SCENE_DOUBLE_OBJECT * countObjectIdMap[id] - poolToEnhance.size;
        int needToInstantiate = desiredAmount - poolToEnhance.size;
        if (needToInstantiate > 0)
        {
            EnhanceGivenPoolIndex(poolToEnhance, needToInstantiate);
        }
    }
   
    private void EnhanceGivenPoolIndex(Pool poolToEnhance, int needToInstantiate)
    {
        Queue<GameObject> gameObjects = _dictionary[poolToEnhance.id.Value];

        for (int i = 0; i < needToInstantiate; i++)
        {
            GameObject gam = Instantiate(poolToEnhance.prefab, transform);
            gam.SetActive(false);
            gameObjects.Enqueue(gam);
            poolToEnhance.size++;
        }

    }

    public void GetObjectsForLevel(Level level)
    {
        Color32[] colorPalatte = level.colorPallete;
        List<ItemData> items = level.items;
        List<ObstacleParentItem> parentItems = level.obstacleParents;
        List<AnimationData> animationDatas = level.animationDatas;
        Dictionary<int, GameObject> pixelIDToAnimationData = new Dictionary<int, GameObject>();
        SetAnimationBehaviorParent(animationDatas, pixelIDToAnimationData);

        var sequence = DOTween.Sequence().Pause();

        foreach (ItemData item in items)
        {    
            GameObject gam = ItemDataToGameObject(item);
            
            GameObject animationParent;
            if (pixelIDToAnimationData.ContainsKey(item.id))
            {
                animationParent = pixelIDToAnimationData[item.id];
                gam.transform.SetParent(animationParent.transform);
            }
            else
            {
                gam.transform.SetParent(_map.transform);
            }

            
            
            ColorizedObject(gam, colorPalatte[item._colorPalatteIndex]);

            SimplePixel sp = gam.GetComponent<SimplePixel>();

            if (sp != null)
            {
                sp.OriginalColor = colorPalatte[item._colorPalatteIndex];

            }

            gam.SetActive(true);
            _dictionary[(int)item._type].Enqueue(gam);

            gam.transform.localScale = Vector3.zero;
            sequence.Append(gam.transform.DOScale(item._scale, 0.005f));
            sequence.Join(gam.transform.DOMoveY(0.3f, 0.005f).SetLoops(2, LoopType.Yoyo));
        }


        SetObstacleParents(parentItems,sequence);

        if (level.levelType == LevelTypes.BossLevel)
        {
            GetShinEffectObjectFromPool(new Vector3(0, 0.09f, 1.56f));
        }
       
        sequence.Play();

    }

    private void SetAnimationBehaviorParent(List<AnimationData> animationDatas, Dictionary<int, GameObject> pixelIDToAnimationData)
    {
        foreach(AnimationData animationData in animationDatas)
        {
            List<int> pixelIds = animationData.pixelIDs;
            GameObject animationParent = GetAnimationParentObjectFromPool();
            
            AnimationBehaviour behavior = animationParent.GetComponent<AnimationBehaviour>();
            animationParent.transform.position = animationData.animationParentLocation;
            animationParent.transform.rotation = Quaternion.identity;
            animationParent.transform.SetParent(_map.transform);

            behavior.animationDuration = animationData.animationDuration;
            behavior.animationParentLocation = animationData.animationParentLocation;
            behavior.animationPoints = animationData.animationPoints;
            behavior.animationType = animationData.animationType;
            behavior.loopType = animationData.loopType;
            behavior.easeType = animationData.easeType;

            animationBehaviours.Add(behavior);

            for (int i = 0; i < pixelIds.Count; i++)
            {
                pixelIDToAnimationData[pixelIds[i]] = animationParent;
            }
        }
    }

    private void SetObstacleParents(List<ObstacleParentItem> parentItems, Sequence sequence)
    {
        foreach (ObstacleParentItem parent in parentItems)
        {
            List<ItemData> obstaclesChilds = parent.obstacles;

            GameObject parentObject = GetObstacleParentObjectFromPool();
            BoxCollider collider = parentObject.GetComponent<BoxCollider>();
            collider.center = parent.colliderCenter;
            collider.size = parent.colliderSize;

            parentObject.transform.position = parent.parentPosition;

            foreach (ItemData obstacle in obstaclesChilds)
            {
                GameObject gam = ItemDataToGameObject(obstacle);

                gam.transform.SetParent(parentObject.transform);

                gam.SetActive(true);
                _dictionary[(int)obstacle._type].Enqueue(gam);

                sequence.Append(gam.transform.DOScale(obstacle._scale, 0.005f));
                sequence.Join(gam.transform.DOMoveY(0.3f, 0.005f).SetLoops(2, LoopType.Yoyo));
            }


        }
    }

     

    private GameObject ItemDataToGameObject(ItemData item)
    {
        GameObject gam = GetObjectFromPool((int)item._type);
        gam.transform.position = item._position;
        gam.transform.localScale = item._scale;
        
        return gam;
    }
 
    public GameObject GetBallObjectFromPool()
    {  
        GameObject ball = ActivateObjectFromPool(_ballId.Value);
        ball.GetComponent<DragAndShootBehaviour>().enabled = true;
        return ball;
    }

    public GameObject GetDeathParticlesObjectFromPool()
    {
        GameObject particles = ActivateObjectFromPool(deathParticleId.Value);
        return particles;
    }

    public GameObject GetDeathScorePopUpObjectFromPool(int levelNumber)
    {
        GameObject popUp = ActivateObjectFromPool(deathScorePopUpId.Value);
        popUp.GetComponent<TextMeshPro>().text = "+" + levelNumber.ToString();

        return popUp;
    }

    public GameObject GetObstacleParentObjectFromPool()
    {
        GameObject obstacleParent = ActivateObjectFromPool(obstacleParentId.Value);
        return obstacleParent;
    }

    public GameObject GetAnimationParentObjectFromPool()
    {
        GameObject animationParent = ActivateObjectFromPool(animationParentId.Value);
        return animationParent;
    }

    public GameObject GetPinyataObjectFromPool()
    {
        GameObject pinyata = ActivateObjectFromPool(pinyataId.Value);
        pinyata.transform.position = pinyataPos;
        pinyata.GetComponent<Rigidbody>().velocity = Vector3.zero;
        pinyata.transform.rotation = Quaternion.Euler(90, 0, 0);
        return pinyata;
    }
    public GameObject GetDiamondObjectFromPool()
    {
        GameObject diamond = ActivateObjectFromPool(diamondId.Value);
        return diamond;
    }
    public GameObject GetShinEffectObjectFromPool(Vector3 position)
    {
        GameObject shinEffect = ActivateObjectFromPool(shinningId.Value);
        shinEffect.transform.position = position;
        return shinEffect;
    }

    public GameObject GetAudioObjectFromPool(Vector3 position)
    {
        GameObject audioObject = ActivateObjectFromPool(audioObjectId.Value);
        audioObject.transform.position = position;
        return audioObject;
    }

    public void ReturnShinEffectObjectToPool()
    {
        GameObject shinEffect = _dictionary[shinningId.Value].Peek();
        shinEffect.SetActive(false);        
    }


    private GameObject ActivateObjectFromPool(int id)
    {
        GameObject gameObject = GetObjectFromPool(id);
        gameObject.transform.SetParent(_map.transform);
        gameObject.SetActive(true);
        _dictionary[id].Enqueue(gameObject);
        loadedObjects.Enqueue(gameObject);
        return gameObject;
    }

    public Queue<GameObject> GetAnimationParentPool()
    { 
        return _dictionary[animationParentId.Value];
    }

    public void SetPinyataCrackMaterialBaseMap(int index)
    {
        crackMaterial.SetTexture("_BaseMap",crackTextures[index]);
    }

    public void SetPinyataBaseMaterialRainBow()
    {
        hitMaterial.SetTexture("_BaseMap", crackTextures[0]);
    }

    public void SetPinyataBaseMaterialHit()
    {
        hitMaterial.SetTexture("_BaseMap", hitTexture);
    }


    public GameObject GetPhysicalBallObjectFromPool()
    {
        GameObject ball = GetObjectFromPool(_ballId.Value);
        ball.transform.SetParent(null);
        ball.SetActive(true);

        return ball;
    }

    private GameObject GetObjectFromPool(int id)
    {
        return _dictionary[id].Dequeue();
    }

    public void ChangeColorOfLevelBoard(Color32 color)
    {
        levelBoardMaterial.color = color;
        float H, S, V;
        
        Color.RGBToHSV(color, out H, out S, out V);
        V *= 0.2f;

        Color playGroundColor = Color.HSVToRGB(H, S, V);
        playGroundMaterial.color = playGroundColor;
    }

    public void ColorizedObject(GameObject gam, Color32 color)
    {
        MeshRenderer meshRenderer = gam.GetComponent<MeshRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", color);
        meshRenderer.SetPropertyBlock(mpb);
    }


    #region Level Editor

#if UNITY_EDITOR

    public GameObject GetObjectFromPoolEditor(int id)
    {
        GameObject gameObject = GetObjectFromPool(id);
        gameObject.SetActive(true);

        if (gameObject == null)
        {
            gameObject = Instantiate(GetPoolWithId(id).prefab);
        }
        else
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null) { rb.isKinematic = true; }
        }


        return gameObject;
    }

    public void ReturnToPool(LevelEditor.GridCell cell)
    {
        GameObject gameObject = cell.GameObject;

        if (gameObject == null)
        {
            return;
        }

        _dictionary[(int)cell.Type].Enqueue(gameObject);
        gameObject.SetActive(false);
    }


    public void PlacePixelToMapOutlineCell(GameObject gam, float scale, int i, int j)
    {
        PlacePixelToMapTransformRotation(gam, scale, i, j);
        gam.transform.localScale = Vector3.one * scale * INTERVAL_COF; 
    }

    public void PlacePixelToMapOutlineCellScaleVector(GameObject gam, Vector3 scale, int i, int j)
    {
        PlacePixelToMapTransformRotation(gam, scale.x, i, j);
        gam.transform.localScale = scale * INTERVAL_COF;
        
    }

    public void PlacePixelToMapFullCell(GameObject gam, float scale, int i, int j)
    {
        PlacePixelToMapTransformRotation(gam, scale, i, j);
        gam.transform.localScale = Vector3.one * scale;
    }

    private void PlacePixelToMapTransformRotation(GameObject gam, float scale, int i, int j)
    {
        gam.transform.position = _map.transform.position + _offset + new Vector3(j, 0, -i) * scale;
        gam.transform.rotation = Quaternion.identity;
        gam.transform.SetParent(_map.transform);
    }

    public void Destroy3DObject(GameObject go)
    {
        Destroy(go);
    }

#endif

#endregion

    private Pool GetPoolWithId(int id)
    {
        foreach(Pool pool in pools)
        {
            if (pool.id.Value == id)
            {
                return pool;
            }
        }
        return null;
    }

    public Vector3 Offset
    {
        get { return _offset; }
        set { _offset = value; }
    }

    public GameObject PhysicalSceneBall
    {
        get { return _physicalSceneBall; }
    }


    #endregion
}

