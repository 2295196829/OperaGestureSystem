using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandBehaviour : HandBehaviourBase
{
    new void Awake()
    {
        Target = MainGameManager.Instance.PerspectiveTarget;
        base.Awake();
    }
    private void Update()
    {
        LeftHandDragModel();
        ScaleModel(20);
    }

}
