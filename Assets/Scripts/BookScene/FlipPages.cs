using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using System.Linq;

public class FlipPages : HandFigureBase
{
    //Service
    LeapProvider mProvider;

    //Value
    private const float straight_sensitivity = 0.9f;
    private const float delta_move_time = 0.05f;
    //float timer = 0;

    //region
    private float farLeft = -0.3f;
    private float middleLeft = -0.15f;
    private float center = 0f;
    private float middleRight = 0.15f;
    private float farRight = 0.3f;

    
    private void Start()
    {
        mProvider = FindObjectOfType<LeapProvider>();
    }

    private void Update()
    {
        TurnPage();
    }

    //Record Hands Data
    private Dictionary<int, (float, float)> handRecordsL = new Dictionary<int, (float, float)>();
    private Dictionary<int, (float, float)> handRecordsR = new Dictionary<int, (float, float)>();
    
    private void TurnPage()
    {
        //Construct a frame to recognize
        Frame frame = mProvider.CurrentFrame;

        //遍历Hand数组，将区域分为四块，根据手掌位置判定翻页
        foreach (Hand hand in frame.Hands)
        {
            if (!fingersUpdated)
            {
                OnFingersUpdate(hand);
                //如果大致手势不是翻页手势，反复检测，不执行后置操作
                if (!IsIndexAbovePinky()){
                    fingersUpdated = false;
                    break;
                }
            }
            //左手实现向右翻页
            if (hand.IsLeft && isMoveRight(hand) && isOpenFullHand(hand))
            {
                // 初始化
                if (!handRecordsL.ContainsKey(hand.Id))
                {
                    //掌心位置、时间
                    handRecordsL[hand.Id] = (hand.PalmPosition.x, Time.time);
                }

                (float previousX, float startTime) = handRecordsL[hand.Id];

                // 更新掌心位置和时间的记录
                handRecordsL[hand.Id] = (hand.PalmPosition.x, Time.time);
                    
                // 检查记录的X位置是否在-0.3到-0.15之间,且手势正确
                if (farLeft <= previousX && previousX <= middleLeft && IsIndexAbovePinky())
                {
                    // 检查当前X位置是否在-0.15到0之间
                    if (middleLeft <= hand.PalmPosition.x && hand.PalmPosition.x <= center)
                    {
                        // 检查时间差
                        if (Time.time - startTime <= delta_move_time)
                        {
                            FlipLeftPage();
                            fingersUpdated = false;
                            break;
                        }
                    }
                        
                }
                    
            }
            //右手实现向左翻页
            if (hand.IsRight && isMoveLeft(hand) &&isOpenFullHand(hand))
            {
                if (!handRecordsR.ContainsKey(hand.Id))
                {
                    handRecordsR[hand.Id] = (hand.PalmPosition.x, Time.time);
                }

                (float previousX, float startTime) = handRecordsR[hand.Id];

                handRecordsR[hand.Id] = (hand.PalmPosition.x, Time.time);

                if (middleRight <= previousX && previousX <= farRight && IsIndexAbovePinky())
                {
                        if (center <= hand.PalmPosition.x && hand.PalmPosition.x <= middleRight)
                        {
                            if (Time.time - startTime <= delta_move_time)
                            {
                                FlipRightPage();
                                fingersUpdated = false;
                                break;
                            }
                        }
                }
            }
        }
    }
    //参考AutoFlip中的翻页函数
    public BookPro ControledBook;
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public bool AutoStartFlip = true;
    bool isPageFlipping = false;

    public void FlipLeftPage()
    {
        if (isPageFlipping) return;
        if (ControledBook.CurrentPaper <= 0) return;
        isPageFlipping = true;
        PageFlipper.FlipPage(ControledBook, PageFlipTime, FlipMode.LeftToRight, () => { isPageFlipping = false; });
    }

    public void FlipRightPage()
    {
        if (isPageFlipping) return;
        if (ControledBook.CurrentPaper >= ControledBook.papers.Length) return;
        isPageFlipping = true;
        PageFlipper.FlipPage(ControledBook, PageFlipTime, FlipMode.RightToLeft, () => { isPageFlipping = false; });
    }
}
