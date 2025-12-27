using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using UnityEngine.Profiling;
using Unity.VisualScripting;

public class CameraControl : HandFigureBase
{
    //Service
    LeapProvider mProvider;
    Camera mCamera;
    SceneControl sceneControl;
    //Value
    private const float moveSpeed = 30.0f; //位移速度 
    private const float duration = 1.0f;//旋转动画持续时间
    private float rotateAngle = 60.0f;//旋转的角度（速度）
    public float maxUpwardAngle = 30f; // 向上旋转的最大角度限制
    public float maxDownwardAngle = -30f; // 向下旋转的最大角度限制
    private bool isCollided = false;//检测碰撞
    private float reboundTime = 8f;//回弹倍率
    void Start()
    {
        mProvider = FindObjectOfType<LeapProvider>();
        mCamera =FindObjectOfType<Camera>();
        sceneControl = GetComponent<SceneControl>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            isCollided = true;
        }
        //与相关物体接触，切换场景
        if (collision.gameObject.tag == "Book")
        {
            sceneControl.Enter_Book();
        }
        if (collision.gameObject.tag == "HangPainting")
        {
            sceneControl.Enter_Perform();
        }
        if (collision.gameObject.tag == "Dress")
        {
            sceneControl.Enter_Display();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isCollided = false;
    }

    #region Move:前后左右
    public void Move_Forward()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            OnFingersUpdate(hand);
            if (hand.IsRight && IsThumbExtened())//右手"双翘"
            {
                if (!isCollided)
                {
                    mCamera.transform.Translate(mCamera.transform.forward * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    mCamera.transform.Translate(-mCamera.transform.forward * moveSpeed * reboundTime * Time.deltaTime, Space.World);
                }
                /* Vector3 direction = mCamera.transform.forward;
                 float distance = move_sensi * Time.deltaTime;
                 Vector3 newPosition = mRigidbody.position + direction * distance;
                 RaycastHit hit;
                 bool hitObstacle = Physics.Linecast(mRigidbody.position, newPosition, out hit) && hit.collider.gameObject.tag == "Obstacle";
                 if (!hitObstacle)
                 {
                     mCamera.transform.Translate(mCamera.transform.forward * move_sensi * Time.deltaTime, Space.World);
                 }*/
                //mCamera.transform.Translate(0, 0, -move_sensi * Time.deltaTime);
            }
        }
    }

    public void Move_Back()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            OnFingersUpdate(hand);
            if (hand.IsRight && IsThumbExtened())
            {
                if (!isCollided)
                {
                    mCamera.transform.Translate(-mCamera.transform.forward * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    mCamera.transform.Translate(mCamera.transform.forward * moveSpeed * reboundTime * Time.deltaTime, Space.World);
                }
            }
        }
    }

    public void Move_Left()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {

            if (hand.IsRight)
            {
                if (!isCollided)
                {
                    mCamera.transform.Translate(-mCamera.transform.right * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    mCamera.transform.Translate(mCamera.transform.right * moveSpeed * reboundTime * Time.deltaTime, Space.World);
                }
            }
        }
    }

    public void Move_Right()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                if (!isCollided)
                {
                    mCamera.transform.Translate(mCamera.transform.right * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    mCamera.transform.Translate(-mCamera.transform.right * moveSpeed * reboundTime * Time.deltaTime, Space.World);
                }
                
            }
        }
    }
    #endregion

    #region Rotate:上下左右//上下有bug，建议做成VR（


    public void Rotate_Up()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            Vector3 normalAngle = mCamera.transform.eulerAngles;
            Debug.Log(normalAngle.x);
            if (hand.IsLeft)                                                    //待修改
            {
                StartCoroutine(RotateCamera(Vector3.right, rotateAngle)); 
                Debug.Log("UP");
            }
        }
    }
    public void Rotate_Down()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            Vector3 normalAngle = mCamera.transform.eulerAngles;
            Debug.Log(normalAngle.x);
            if (hand.IsLeft)
            {
                StartCoroutine(RotateCamera(Vector3.right, -rotateAngle)); 
                Debug.Log("DOWN");
            }
        }
    }
    public void Rotate_Left()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                StartCoroutine(RotateCamera(Vector3.up, -rotateAngle)); 
            }
        }
    }
    public void Rotate_Right()
    {
        Frame frame = mProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                StartCoroutine(RotateCamera(Vector3.up, rotateAngle)); 
            }
        }
    }

    //协程控制摄像头旋转
    IEnumerator RotateCamera(Vector3 axis, float speed)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(axis * speed * Time.deltaTime) * currentRotation;
        transform.rotation = finalRotation;
        yield return null; // 等待下一帧
    }

    //重载控制摄像头旋转的过渡画面
    IEnumerator RotateCamera(Vector3 axis, float angle, float duration)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(axis * angle) * currentRotation;
        float passedTime = 0.0f;

        //过渡
        while (passedTime < duration)
        {
            // 计算旋转插值
            transform.rotation = Quaternion.Slerp(currentRotation, finalRotation, passedTime / duration);
            passedTime += Time.deltaTime;
            //等待下一帧
            yield return null;
        }

        // 确保旋转最终到达目标角度
        transform.rotation = finalRotation;
    }

    void NormalizeEuler(ref Vector3 rEuler)
    {
        if (rEuler.x < -180f)
        {
            rEuler.x += 360f;
        }
        else if (rEuler.x > 180f)
        {
            rEuler.x -= 360f;
        }
        if (rEuler.y < -180f)
        {
            rEuler.y += 360f;
        }
        else if (rEuler.y > 180f)
        {
            rEuler.y -= 360f;
        }
    }
    #endregion


}
