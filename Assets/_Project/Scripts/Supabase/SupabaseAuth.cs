using UnityEngine;
using Supabase;
using Supabase.Gotrue;
using System;
using System.Threading.Tasks;

public class SupabaseAuth : MonoBehaviour
{
    private Supabase.Client supabase;

    async void Start()
    {
        Debug.Log("▶ Start() 実行: Supabase 初期化と匿名ログインを開始します");
        await InitializeSupabase();
        await SignUpAnonymous();
    }

    private async Task InitializeSupabase()
    {
        Debug.Log("▶ InitializeSupabase(): 開始");

        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        supabase = new Supabase.Client(
            "https://epgoeizuiesplinvsnvf.supabase.co", // ← Supabase プロジェクトURL
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVwZ29laXp1aWVzcGxpbnZzbnZmIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTkyNzUyMzEsImV4cCI6MjA3NDg1MTIzMX0.xW7roMocqdh7DdIZyU4zA80Q1x89YMS4vHWzyoWjkq4",                     // ← anon key
            options
        );

        await supabase.InitializeAsync();
        Debug.Log("✅ Supabase 初期化完了");
    }

    private async Task SignUpAnonymous()
    {
        Debug.Log("▶ SignUpAnonymous(): 匿名ユーザー作成開始");

        // ランダムなユーザーを生成
        string randomEmail = $"guest_{Guid.NewGuid()}@gmail.com";
        string randomPassword = Guid.NewGuid().ToString();

        Debug.Log($"📧 生成したメール: {randomEmail}");
        Debug.Log($"🔑 生成したパスワード: {randomPassword}");

        try
        {
            // SignUpAsync は存在しない → SignUp() を使用
            var response = await supabase.Auth.SignUp(randomEmail, randomPassword);

            if (response.User != null)
            {
                Debug.Log("✅ 匿名ユーザー登録成功！");
                Debug.Log($"🆔 ユーザーID: {response.User.Id}");
            }
            else
            {
                Debug.LogWarning("⚠ 登録は完了しましたが User が null です");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ 匿名ユーザー登録失敗: {ex.Message}");
        }
    }
}
