using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] private GameObject _UFO;
    [SerializeField] private TextMeshProUGUI _sheepCountText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Transform UFOSpawn;

    [Space]

    public UnityEvent SheepAbducted = new();
    public UnityEvent GameReset = new();

    public bool GameOver { get; private set; } = true;
    public bool GamePlaying { get; private set; } = false;
    private int _sheepCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ResetBoard();
    }

    public void StartGame()
    {
        GamePlaying = true;
        ResetBoard();
    }

    public void SetGamePaused(bool paused)
    {
        if (!GameOver)
            GamePlaying = !paused;
    }

    public void OnGameOver()
    {
        GamePlaying = false;
        _gameOverPanel.SetActive(true);
        GameOver = true;
    }

    public void AddSheep()
    {
        SetSheepCount(_sheepCount + 1);
        SheepAbducted.Invoke();
    }

    public void ResetBoard()
    {
        _UFO.transform.position = UFOSpawn.position;
        _gameOverPanel.SetActive(false);
        SetSheepCount(0);
        GameOver = false;
        GameReset.Invoke();
    }

    private void SetSheepCount(int count)
    {
        _sheepCount = count;
        _sheepCountText.SetText($"Sheep Abducted: {_sheepCount}");
    }
}
