using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhysicalSceneManager
{
    public GameObject _physicalBall;
    public Rigidbody _physicalBallRigidbody;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    public void CreateProjectionLineScene(Transform levelBoard)
    {
        if (_simulationScene.IsValid())
        {
            return;
        }
        
        _simulationScene = SceneManager.CreateScene("PhysicslLevel", new CreateSceneParameters(LocalPhysicsMode.Physics3D));        
        _physicsScene = _simulationScene.GetPhysicsScene();
        Physics.autoSimulation = false;

        foreach (Transform obj in levelBoard)
        {
            GameObject levelPart = MonoBehaviour.Instantiate(obj.gameObject, obj.position, obj.rotation);
            Renderer rd = levelPart.GetComponent<Renderer>();
            if (rd != null) { rd.enabled = false; }
            SceneManager.MoveGameObjectToScene(levelPart, _simulationScene);

        }

    }

    public void AddBallToPhysicalScene()
    {
        _physicalBall = ObjectManager.Instance.PhysicalSceneBall;
        _physicalBall.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(_physicalBall, _simulationScene);
        _physicalBallRigidbody = _physicalBall.GetComponent<Rigidbody>();
    }

    public void SendObjectToPhysicalScene(GameObject physicSceneObject)
    {
        physicSceneObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(physicSceneObject, _simulationScene);
        physicSceneObject.SetActive(true);
        
        physicSceneObject.GetComponent<Renderer>().enabled = false;

        //if (!physicSceneObject.isStatic)
        //{
        //_spawnedObjects.Add(physicSceneObject.transform, physicSceneObject.transform);

        //}
    }

    public Scene SimulationScene
    {
        get { return _simulationScene; }
    }

    public PhysicsScene PhysicsScene
    {
        get { return _physicsScene; }
    }

    public GameObject PhysicalBall
    {
        get { return _physicalBall; }
    }

    public Rigidbody PhysicalBallRigidbody
    {
        get { return _physicalBallRigidbody; }
    }



}
