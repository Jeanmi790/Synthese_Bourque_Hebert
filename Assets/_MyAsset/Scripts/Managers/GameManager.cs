using UnityEngine;

public class GameManager : MonoBehaviour, IGameInfo
{
    [Header("Player")]
    [SerializeField] private Player player = default;

    private Vector2 playerPosition;

    public float InGameTime { get; set; }
    public float Score { get; set; }
    public bool IsPlayerDead { get; set; }

    private float ajustTime { get; set; }

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        InGameTime = 0f;
        ajustTime = 0f;
        Score = 0f;
        loadPlayerInfo();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsPlayerDead) { UpdateTime(); }
    }

    public void loadPlayerInfo()
    {
        player = FindObjectOfType<Player>();
        playerPosition = player.transform.position;
        IsPlayerDead = false;
    }

    public float GetTime()
    {
        return InGameTime;
    }

    public void UpdateTime()
    {
        InGameTime = Time.timeSinceLevelLoad;
    }

    public void AddScore(float score)
    {
        Score += score;
    }

    public float GetScore()
    {
        return Score;
    }

    public void GameOver()
    {
        IsPlayerDead = true;
        Debug.Log(GetScore());
        PlayerPrefs.SetFloat("Score", GetScore());
        PlayerPrefs.SetFloat("Time", InGameTime);
        PlayerPrefs.Save();
    }

    public void RestartGame()
    {
        InGameTime = 0f;
        ajustTime = 0f;
        Score = 0f;
        IsPlayerDead = false;
    }
}