using UnityEngine;
using System.Collections;

public class HighLightControl : MonoBehaviour
{

    //持有当前外发光需要的组件
    private HighlightableObject m_ho;
   public  bool flat = false;
   // int x = 1;

    void Awake()
    {
        m_ho = GetComponent<HighlightableObject>();
        m_ho.ConstantOn(Color.green);

    }
    private void Update()
    {
    //    if (flat==true)
     //   {
            m_ho.ConstantOn(Color.green);

   //     }
    //    else
    //    {
    //        m_ho.ConstantOff();

    //    }
    }
    public void SetRedLight()
    {
        m_ho.ConstantOn(Color.red);
    }
    public void SetGreedLight()
    {
        m_ho.ConstantOn(Color.green);
    }
    public void CloseLight()
    {
        m_ho.ConstantOff();
    }

    void HifhLightFunction()
    {
        //循环往复外发光开启（参数为：颜色1，颜色2，切换时间）
       // m_ho.FlashingOn(Color.green, Color.blue, 1f);

        //关闭循环往复外发光
       // m_ho.FlashingOff();


        //持续外发光开启（参数：颜色）
       // m_ho.ConstantOn(Color.yellow);

        //关闭持续外发光
      //  m_ho.ConstantOff();
    }

    ///// <summary>
    ///// 鼠标指向模型时触发
    ///// </summary>
    private void onmouseenter()
    {
        //开启外发光
        if (flat == false)
        {

            m_ho.ConstantOn(Color.green);
        }
        
    }

    /// <summary>
    /// 鼠标离开模型时触发
    /// </summary>
    private void onmouseexit()
    {
        if (flat == false)
        {

            m_ho.ConstantOff();
        }
       
    }
}