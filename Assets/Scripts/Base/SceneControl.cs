using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    #region 异步加载场景
    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        //SceneManager.Load("过渡画面")
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            //进度条加载
            yield return null;
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    #endregion

    #region 首页界面
    public void OnPress_Enter()
    {
        StartCoroutine(LoadSceneAsync(1));
    }
    public void OnPress_Teach()
    {
        StartCoroutine(LoadSceneAsync(2));
    }
    public void OnPress_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region 梨园界面
    public void Enter_Book()
    {
        StartCoroutine(LoadSceneAsync(3));
    }
    public void Enter_Perform()
    {
        StartCoroutine(LoadSceneAsync(4));
    }


    //服饰展示界面
    public void Enter_Display()//默认进入“凤冠”
    {
        StartCoroutine(LoadSceneAsync(5));
    }

    public void Load_Garden()
    {
        StartCoroutine(LoadSceneAsync("1_PearGarden"));
    }

    public void Next_Display()//“凤冠”进入“女蟒”
    {
        StartCoroutine(LoadSceneAsync(6));
    }

    public void Last_Display()//“女蟒”返回“凤冠”
    {
        StartCoroutine(LoadSceneAsync(5));
    }
    #endregion

    #region 教学界面
    public void Load_HomePage()
    {
        StartCoroutine(LoadSceneAsync("0_HomePage"));
    }





    #endregion
}
