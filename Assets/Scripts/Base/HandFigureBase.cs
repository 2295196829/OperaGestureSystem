using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//引用
using Leap;
using Leap.Unity;
 
public class HandFigureBase : MonoBehaviour
{
    //Service Provider
    LeapProvider provider;

    //Controller controller;
    public HandModelBase leftHandModel;//左手
    public HandModelBase rightHandModel;//右手

    //Fingers
    private Finger thumbFinger;//大拇指
    private Finger indexFinger;//食指
    private Finger middleFinger;//中指
    private Finger ringFinger;//无名指
    private Finger pinkyFinger;//小拇指
    public bool fingersUpdated = false;//记录手指更新状态

 
    ///判断条件
    const float smallestVelocity = 0.1f;
    const float deltaVelocity = 0.000001f;
    const float deltaCloseFinger = 0.06f;
 
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    #region MoveDiraction  移动方向
    public bool isStationary(Hand hand)// 固定不动:判断hand是否禁止
    {
        //位移小于判定值
        return hand.PalmVelocity.magnitude < smallestVelocity;
    }
    public bool isMoveUp(Hand hand)     // 手划向上
    {
        return hand.PalmVelocity.y > deltaVelocity && !isStationary(hand);
    }
    public bool isMoveDown(Hand hand)   // 手划向下
    {
        return hand.PalmVelocity.y < -deltaVelocity && !isStationary(hand);
    }
    public bool isMoveLeft(Hand hand)   // 手划向左
    { 
        return hand.PalmVelocity.x < -deltaVelocity && !isStationary(hand);
    }
    public bool isMoveRight(Hand hand)  // 手划向右
    {
        return hand.PalmVelocity.x > deltaVelocity && !isStationary(hand);
    }
    #endregion

    #region PalmDiraction  掌心朝向
    public bool isPalmFacingUp(Hand hand) //掌心向上
    {
        return hand.PalmNormal.y > deltaVelocity;
    }
    public bool isPalmFacingDown(Hand hand) //掌心向下
    {
        return hand.PalmNormal.y < -deltaVelocity;
    }
    public bool isPalmFacingLeft(Hand hand) //掌心向左
    {
        return hand.PalmNormal.x > deltaVelocity;
    }
    public bool isPalmFacingRight(Hand hand) //掌心向右
    {
        return hand.PalmNormal.x < -deltaVelocity;
    }
    #endregion

    #region Custom HandFigure 手掌状态
    public bool isOpenFullHand(Hand hand)   //手掌全展开
    {
        return hand.GrabStrength == 0;
    }

    public bool isCloseHand(Hand hand)     //握拳 
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        { //遍历所有的手指
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).magnitude < deltaCloseFinger)
            {
                count++;
            }
        }
        return (count == 5);
    }

    #endregion

    #region FingerFigure（Custom Gesture）  手指状态

    public void OnFingersUpdate(Hand hand) //更新手指状态，获取手指信息
    {
        //需要在子类中增加判断，令手指状态更新
        thumbFinger = hand.Fingers.FirstOrDefault(f => f.Type == Finger.FingerType.TYPE_THUMB);
        indexFinger = hand.Fingers.FirstOrDefault(f => f.Type == Finger.FingerType.TYPE_INDEX);
        middleFinger = hand.Fingers.FirstOrDefault(f => f.Type == Finger.FingerType.TYPE_MIDDLE);
        ringFinger = hand.Fingers.FirstOrDefault(f => f.Type == Finger.FingerType.TYPE_RING);
        pinkyFinger = hand.Fingers.FirstOrDefault(f => f.Type == Finger.FingerType.TYPE_PINKY);

        //reset flag
        fingersUpdated = true;
    }

    public bool IsIndexAbovePinky()//食指是否在小拇指上方，模拟翻页时手指状态
    {
        if (indexFinger != null && pinkyFinger != null)
        {
            return indexFinger.TipPosition.y > pinkyFinger.TipPosition.y;
        }
        return false;
    }

    public bool IsIndexAndMiddleExtened()//食指和中指伸直，其他手指配合Fist，模拟手势“怒发”
    {
        if (indexFinger != null && middleFinger != null)
        {
            return indexFinger.IsExtended && middleFinger.IsExtended; 
        }
        return false;
    }

    public bool IsIndexAndRingExtened()//食指和无名指伸直，大拇指和中指无名指接触，模拟手势“雨润”
    {
        if (indexFinger != null && middleFinger != null)
        {
            bool thumbTouchesMiddle = (thumbFinger.TipPosition-middleFinger.TipPosition).magnitude < deltaCloseFinger;
            bool thumbTouchesRing = (thumbFinger.TipPosition - ringFinger.TipPosition).magnitude < deltaCloseFinger;
            return indexFinger.IsExtended && ringFinger.IsExtended && thumbTouchesMiddle && thumbTouchesRing;
        }
        return false;
    }

    public bool IsThumbExtened()//大拇指伸直，其他手指配合Horns，模拟手势“双翘”
    {
        if (thumbFinger != null)
        {
            return thumbFinger.IsExtended;
        }
        return false;
    }
    #endregion
}
