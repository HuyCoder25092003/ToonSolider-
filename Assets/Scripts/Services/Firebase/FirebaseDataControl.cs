using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;
public class FirebaseDataControl : BYSingletonMono<FirebaseDataControl>
{
    [SerializeField] DataModel dataLocalModel;
    PlayerData playerData;
    DatabaseReference user_reference;
    public bool isGetDataDone;
    int ver;
    public void CheckDataServer()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        user_reference = reference.Child("LOCAL_DATA").Child(FirebaseAuthenticationManager.instance.Firebase_Account);
        user_reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("Task IsFaulted");
                playerData = null;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    Debug.LogError("Data: " + snapshot.GetRawJsonValue());
                    playerData = JsonConvert.DeserializeObject<PlayerData>(snapshot.GetRawJsonValue());
                }
                else
                {
                    Debug.LogError("Data empty: ");
                    playerData = null;
                }
            }
            isGetDataDone = true;
        });
    }
    public PlayerData GetData()
    {
        return playerData;
    }
    public void SaveAllData()
    {
        string json = JsonConvert.SerializeObject(dataLocalModel.GetAllData());
        user_reference.SetRawJsonValueAsync(json);
    }
    public void UpdateVersionSignIn()
    {
        user_reference.Child("version_sign_in").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Version Sign In Failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    ver = int.Parse(snapshot.Value.ToString());
                    ver++;
                    user_reference.Child("version_sign_in").SetValueAsync(ver).ContinueWith(task =>
                    {
                        user_reference.Child("version_sign_in").ValueChanged += VersionSignIn_ValueChanged;
                    });
                }
            }
        });
    }

    void VersionSignIn_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
            Debug.LogError(e.DatabaseError.Message);
        else
        {
            int ver_new = int.Parse(e.Snapshot.Value.ToString());
            if (ver_new > ver)
            {
                Debug.LogError("Cho cút account trùng");
                //DialogManager.instance.ShowDialog(DialogIndex.TheSameDialogAccount);
            }
            Debug.LogError("Version_sigin_value changed: " + ver_new);
        }
    }
}
