using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.Constants;

[Table("players")]
public class SupabasePlayer : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class SupabaseManager : MonoBehaviour
{
    public static Supabase.Client SupabaseClient;

    async void Start()
    {
        SupabaseClient = new Supabase.Client(
            "https://xxxxxx.supabase.co",
            "YOUR_ANON_KEY"
        );

        await SupabaseClient.InitializeAsync();

        // 起動時にユーザー確認
        if (SupabaseClient.Auth.CurrentUser == null)
        {
            Debug.Log("⚠️ 未ログイン状態");
        }

        // データ操作例
        // var newPlayer = await SavePlayerData("テストプレイヤー");
        // await LoadPlayerData();
    }

    // ---------------------------
    // ---------------------------
    // 認証系
    // ---------------------------
    // ---------------------------

    // メール/パスワードでサインアップ
    public async Task SignUpWithEmail(string email, string password)
    {
        try
        {
            var session = await SupabaseClient.Auth.SignUp(email, password);
            Debug.Log("✅ サインアップ成功: " + session.User.Id);
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ サインアップ失敗: " + ex.Message);
        }
    }

    // メール/パスワードでサインイン
    public async Task SignInWithEmail(string email, string password)
    {
        try
        {
            var session = await SupabaseClient.Auth.SignIn(email, password);
            Debug.Log("✅ サインイン成功: " + session.User.Id);
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ サインイン失敗: " + ex.Message);
        }
    }

    // Google OAuth サインイン
    public void SignInWithGoogle()
{
    string provider = "google";
    string redirectUrl = "io.supabase.unity://login-callback"; // Supabase Auth 設定で登録
    string authUrl = $"https://xxxxxx.supabase.co/auth/v1/authorize?provider={provider}&redirect_to={redirectUrl}";

    // Web ブラウザで開く
    Application.OpenURL(authUrl);

    Debug.Log("🌐 Google OAuth 認証開始");
}


    // 匿名ログイン
    public async Task SignInAnonymously()
    {
        try
        {
            var session = await SupabaseClient.Auth.SignInAnonymously();
            Debug.Log("🆔 匿名ユーザー作成: " + session.User.Id);
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 匿名ログイン失敗: " + ex.Message);
        }
    }

    // ---------------------------
    // ---------------------------
    // プレイヤーデータ CRUD
    // ---------------------------
    // ---------------------------

    public async Task<SupabasePlayer> SavePlayerData(string playerName)
    {
        var user = SupabaseClient.Auth.CurrentUser;
        if (user == null)
        {
            Debug.LogError("❌ ユーザーが未ログイン");
            return null;
        }

        var player = new SupabasePlayer
        {
            UserId = user.Id,
            Name = playerName,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            var response = await SupabaseClient
                .From<SupabasePlayer>()
                .Insert(player);

            var insertedPlayer = response.Models[0];
            Debug.Log("✅ 保存成功: " + insertedPlayer.Name);
            return insertedPlayer;
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 保存失敗: " + ex.Message);
            return null;
        }
    }

    public async Task LoadPlayerData()
    {
        var user = SupabaseClient.Auth.CurrentUser;
        if (user == null) return;

        try
        {
            var response = await SupabaseClient
                .From<SupabasePlayer>()
                .Select("*")
                .Filter("user_id", Operator.Equals, user.Id)
                .Get();

            List<SupabasePlayer> players = response.Models;

            foreach (var p in players)
                Debug.Log($"👤 名前: {p.Name}, 作成日: {p.CreatedAt}");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 読み込み失敗: " + ex.Message);
        }
    }

    public async Task UpdatePlayerName(Guid playerId, string newName)
    {
        try
        {
            var response = await SupabaseClient
                .From<SupabasePlayer>()
                .Filter("id", Operator.Equals, playerId)
                .Update(new SupabasePlayer { Name = newName });

            if (response.Models.Count > 0)
                Debug.Log($"✅ 更新成功: {response.Models[0].Name}");
            else
                Debug.LogWarning("⚠️ 更新対象が見つかりません");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 更新失敗: " + ex.Message);
        }
    }

    public async Task DeletePlayer(Guid playerId)
    {
        try
        {
            await SupabaseClient
                .From<SupabasePlayer>()
                .Filter("id", Operator.Equals, playerId)
                .Delete();

            Debug.Log("✅ 削除完了");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 削除失敗: " + ex.Message);
        }
    }
}

