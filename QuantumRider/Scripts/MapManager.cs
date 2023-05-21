using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get => _instance; }
    static MapManager _instance;
    [SerializeField] private float _fadeTime;
    private SceneID beforeMap = SceneID.None;//直前にいたシーン名を格納
    public enum SceneID
    {
        //別スクリプトからマップ名を指定するときに使用
        Title,
        Tutorial,
        EasyMap,
        MainGameScene,
        GameOver,
        GameClear,
        None
    }
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        //FPSを固定
        Application.targetFrameRate = 160;

    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        Fade.fadeDelegate += CallFadeIn;

        //タイトルシーンのみで実行
        if (SceneManager.GetActiveScene().name == "TitleSeki")
        {
            SoundManager.instance.Play("Title");
            //FadeからImageを取得
            Fade.GetFadeImage();//タイトルでボタンが押せない問題を修正のため
            Fade._fadeImage.gameObject.SetActive(false);
        }

    }
    //Sceneのマップに移動
    public void SceneChange(SceneID Scene)
    {
        switch (Scene)
        {
            case SceneID.Title:
                SceneManager.LoadScene("TitleSeki");
                SoundManager.instance.Play("Title");
                break;
            case SceneID.Tutorial:
                SceneManager.LoadScene("TutorialMap");
                SoundManager.instance.Play("MainGame");
                beforeMap = Scene;
                HPManager.instance.HpReset();
                break;
            case SceneID.EasyMap:
                SceneManager.LoadScene("EasyMap");
                SoundManager.instance.Play("MainGame");
                beforeMap = Scene;
                HPManager.instance.HpReset();
                break;
            case SceneID.MainGameScene:
                SceneManager.LoadScene("MainGameScene");
                SoundManager.instance.Play("MainGame");
                beforeMap = Scene;
                HPManager.instance.HpReset();
                break;
            case SceneID.GameOver:
                SceneManager.LoadScene("GameOver");
                SoundManager.instance.Play("GameOver");
                HPManager.instance.HpReset();
                break;
            case SceneID.GameClear:
                SceneManager.LoadScene("GameClear");
                SoundManager.instance.Play("GameClear");
                break;
            default:
                Debug.LogWarning("そのマップは存在しません");
                break;
        }
    }
    public void CallFadeIn(SceneID scene)
    {
        Fade.FadeChange(_fadeTime, SceneChange, scene);
    }
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //メインゲームに移動したらマウスカーソルを消す
        if (SceneManager.GetActiveScene().name == "MainGameScene" ||
            SceneManager.GetActiveScene().name == "EasyMap" ||
            SceneManager.GetActiveScene().name == "TutorialMap")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        //GameOverスクリプトにリトライ時に移動するマップを指定する
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            GetComponent<GameOver>();
            GameOver.mapName = beforeMap;
        }
            StartCoroutine(Fade.IEFadeIn(_fadeTime));
    }
}
