using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        yield return new WaitForSeconds(1);
        ConfigManager.instance.InitConfig(() =>
        {
            GameServiceManager.instance.InitGameService();
        });
        yield return new WaitUntil(() => GameServiceManager.instance.isEndGameservice);

        // Check Firebase
        FirebaseAuthenticationManager.instance.CheckAuthentication();
        yield return new WaitUntil(() => FirebaseAuthenticationManager.instance.Firebase_Account != string.Empty);

        //Data Local or firebase
        FirebaseDataControl.instance.CheckDataServer();
        yield return new WaitUntil(() => FirebaseDataControl.instance.isGetDataDone);
        DataController.instance.CheckDataFirebase(FirebaseDataControl.instance.GetData(), InitData);
    }
    private void InitData()
    {
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
            FirebaseDataControl.instance.UpdateVersionSignIn();
        });
    }
}
