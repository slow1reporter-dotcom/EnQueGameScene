using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

public class SupabaseAuthManager : MonoBehaviour
{
    private string supabaseUrl = "https://<YOUR-PROJECT-URL>.supabase.co";
    private string anonKey = "<YOUR-ANON-PUBLIC-KEY>";

    void Start()
    {
        StartCoroutine(SignUpRandomUser());
    }

    IEnumerator SignUpRandomUser()
    {
        string email = $"guest{System.Guid.NewGuid()}@example.com";
        string password = System.Guid.NewGuid().ToString();

        var json = JsonUtility.ToJson(new { email = email, password = password });

        using (UnityWebRequest request = new UnityWebRequest($"{supabaseUrl}/auth/v1/signup", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("apikey", anonKey);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"SignUp failed: {request.error}");
            }
            else
            {
                Debug.Log($"Anonymous user created: {request.downloadHandler.text}");
            }
        }
    }
}
