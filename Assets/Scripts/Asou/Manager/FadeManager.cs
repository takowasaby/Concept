using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 画面遷移を管理するシングルトンクラス
public class FadeManager : MonoBehaviour
{
    public static FadeManager instance; // マネージャオブジェクトの唯一のインスタンス

    private const int layer = 1; // OnGUIの描画レイヤー

    public bool isFading = false; // フェード中かどうか
    private float fadeAlpha = 0f; // フェード中の透明度
    private Color fadeColor = Color.white; // フェード色

    // アプリ起動時にマネージャオブジェクトを起動する
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBoot()
    {
        new GameObject("FadeManager", typeof(FadeManager));
    }

    // プレハブ生成時にメンバを初期化する
    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 画面遷移時のフェードインアウトのGUI表示
    public void OnGUI()
    {
        if (isFading)
        {
            GUI.depth = layer;
            fadeColor.a = fadeAlpha;
            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    // シーン遷移用コルーチン
    private IEnumerator TransScene(string scene, float interval)
    {
        // シーン遷移開始
        isFading = true;

        // フェードアウト
        float time = 0;
        while (time <= interval)
        {
            fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        // シーン切り替え
        SceneManager.LoadScene(scene);

        // フェードイン
        time = 0;
        while (time <= interval)
        {
            fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        // シーン遷移終了
        isFading = false;
    }

    // フェードインアウト付きの画面遷移を開始する
    public bool LoadScene(string scene, float interval)
    {
        if (!isFading)
        {
            StartCoroutine(TransScene(scene, interval));
            return true;
        }
        else return false;
    }
}
