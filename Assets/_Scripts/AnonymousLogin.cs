using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class AnonymousLogin : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Sign in anonymously succeeded!");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        };
        
        try
        {
            if(!AuthenticationService.Instance.IsSignedIn) {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Sign in anonymously failled with errr code: {ex.ErrorCode}, msg: {ex.Message}");
        }
    }

    void Update()
    {
        
    }
}
