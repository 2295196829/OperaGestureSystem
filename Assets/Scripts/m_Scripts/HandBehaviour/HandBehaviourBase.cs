using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using Leap.Unity.HandsModule;

public enum HandState
{
    Normal,
    Pinch
}
public class HandBehaviourBase : MonoBehaviour
{
    protected HandState handstate=HandState.Normal;
    public HandState HandStateIndexer
    {
        get { return handstate; }
    }

    //左手
    protected Transform leftHand;
    public Transform LeftHand
    {
        get { return leftHand; }
    }
    protected Transform leftHandPalm;
    protected InteractionHand leftHandManager;
    protected bool isLeftHandPinched = false;

    //右手
    protected Transform rightHand;
    public Transform RightHand
    { 
        get { return rightHand; } 
    }
    protected Transform rightHandPalm;
    protected InteractionHand rightHandManager; 
    protected bool isRightHandPinched = false;

    protected Transform Target;

    protected void Awake()
    {
        
        Transform father = GameObject.Find("Interaction Manager").transform;
        leftHand=father.GetChild(0);
        rightHand = father.GetChild(1);

        leftHandManager = leftHand.GetComponent<InteractionHand>();
        rightHandManager = rightHand.GetComponent<InteractionHand>();
        
        InitPinchDectector();
    }

    private void SetHandPinched(ref bool handPinched,bool state)
    {
        handPinched= state;
    }

    //private void Update()
    //{
    //    if (isLeftHandPinched) { print(leftHandManager.velocity); }
    //}
    #region 控制模型
    protected void LeftHandDragModel( float speed=850)
    {
        if (isLeftHandPinched && !isRightHandPinched)
            Target.Rotate(Vector3.down, leftHandManager.velocity.x * speed * Time.deltaTime);
    }

    private float oldDistance = 0;
    protected void ScaleModel(float speed,float minSize=0.6f,float maxSize=1.2f)
    {
        if(!isLeftHandPinched || ! isRightHandPinched)
            return;
        //float scaleTmp = 0;
        //取得双手在x轴上的距离
        float newDistance=Mathf.Abs(leftHandManager.position.x-rightHandManager.position.x);
        float dValue = newDistance - oldDistance;
        //如果新旧距离大于某个阈值
        if(Mathf.Abs(newDistance-oldDistance)>=0.02f)
        {
            int direction=dValue>=0? 1 : -1;
            float finalScaleFactor = direction * Time.deltaTime * speed+Target.localScale.x;
            finalScaleFactor=Mathf.Clamp(finalScaleFactor, minSize, maxSize);
            Target.localScale = finalScaleFactor*Vector3.one;
            oldDistance = newDistance;
        }
    }

    #endregion


    #region 初始化PinchedDetector
    protected Transform leftHandModel=null;
    protected Transform righHandModel=null;
    /// <summary>
    /// 初始化PinchedDetector
    /// </summary>
    protected void InitPinchDectector()
    {
        HandModelManager handModelManager=FindObjectOfType<HandModelManager>();

        leftHandModel=handModelManager.transform.GetChild(0);
        righHandModel=handModelManager.transform.GetChild(1);

        PinchDetector leftPinchDetector=leftHandModel.gameObject.AddComponent<PinchDetector>();
        PinchDetector rightPinchDetector=righHandModel.gameObject.AddComponent<PinchDetector>();

        //左手
        leftPinchDetector.OnActivate = new UnityEngine.Events.UnityEvent();
        leftPinchDetector.OnActivate.AddListener(() => { SetHandPinched(ref isLeftHandPinched, true); });
        leftPinchDetector.OnDeactivate = new UnityEngine.Events.UnityEvent();
        leftPinchDetector.OnDeactivate.AddListener(() => { SetHandPinched(ref isLeftHandPinched, false); });

        //右手
        rightPinchDetector.OnActivate = new UnityEngine.Events.UnityEvent();
        rightPinchDetector.OnActivate.AddListener(() => { SetHandPinched(ref isRightHandPinched, true); });
        rightPinchDetector.OnDeactivate = new UnityEngine.Events.UnityEvent();
        rightPinchDetector.OnDeactivate.AddListener(() => { SetHandPinched(ref isRightHandPinched, false); });
    }
    #endregion

}
