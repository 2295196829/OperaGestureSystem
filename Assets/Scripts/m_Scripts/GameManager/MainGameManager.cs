using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : GameManagerBase
{
    public static MainGameManager Instance=null;

    private MainHandBehaviour handBehaviour;
    public MainHandBehaviour HandBehaviour
    {
        get { return handBehaviour; }
    }

    private Transform perspectiveTarget;
    public Transform PerspectiveTarget
    {
        get { return perspectiveTarget; }

    }
    new void Awake()
    {
        Instance = this;
        base.Awake();
    }
    protected override void InitGameObject()
    {
        perspectiveTarget = GameObject.FindGameObjectWithTag("PerspectiveTarget").transform;
    } 
    protected override void InitManager()
    {
        handBehaviour = new GameObject("HandBehaviour").AddComponent<MainHandBehaviour>();
    }

  
}
