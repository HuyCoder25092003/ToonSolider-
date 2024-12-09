using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class FirebaseAuthenticationManager : BYSingletonMono<FirebaseAuthenticationManager>
{
    FirebaseAuth auth;
    FirebaseUser user = null;
    string fbUUID;
    public string Firebase_Account
    {
        get
        {
            return "test_acc";
        }
        set
        {
            fbUUID = value;
        }
    }
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Firebase_Account = user.UserId;
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
    public void CheckAuthentication()
    {
        if (user != null)
        {
            Debug.LogError("CheckAuthentication user != null");
            Firebase_Account = user.UserId;
        }
        else
        {
            StartCoroutine(nameof(StartCheckAccount));

        }
    }
    IEnumerator StartCheckAccount()
    {
        yield return new WaitForEndOfFrame();
        Debug.LogError("CheckAuthentication SignInWithEmailAndPasswordAsync");
        //1. try sign in:
        bool is_SignInDone = false;
        string email = GameServiceManager.instance.UUID() + "@by.com";
        string password = "Aa123Bb456";
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            else if (task.IsFaulted)
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            else
            {
                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                Firebase_Account = result.User.UserId;
            }
            is_SignInDone = true;
        });
        //2. try sign up
        yield return new WaitUntil(() => is_SignInDone);
        if (Firebase_Account == string.Empty)
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                if (task.IsFaulted)
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                // Firebase user has been created.
                AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                Firebase_Account = result.User.UserId;
            });
        }
    }
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}
