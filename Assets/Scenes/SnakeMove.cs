using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveRate = 0.15f;
    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection = Vector2.right;

    private bool isGameOver = false;

    [Header("Map Limit")]
    public int minX = -9;
    public int maxX = 9;
    public int minY = -5;
    public int maxY = 5;

    [Header("Body")]
    public Transform bodyPrefab;
    private List<Vector3> positions = new();
    private List<Transform> bodyParts = new();

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    private int score = 0;

    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverText;

    [Header("Food Spawner")]
    public FoodSpawner foodSpawner;

    void Start()
    {
        positions.Clear();
        positions.Add(transform.position);

        score = 0;
        UpdateScore();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        InvokeRepeating(nameof(Move), moveRate, moveRate);
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
            nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
            nextDirection = Vector2.right;
    }

    void Move()
    {
        if (isGameOver) return;

        direction = nextDirection;
        Vector3 newPos = positions[0] + (Vector3)direction;

        // ===== CHECK TƯỜNG =====
        if (newPos.x < minX || newPos.x > maxX ||
            newPos.y < minY || newPos.y > maxY)
        {
            GameOver();
            return;
        }

        // ===== CHECK CẮN THÂN =====
        if (positions.Contains(newPos))
        {
            GameOver();
            return;
        }

        positions.Insert(0, newPos);

        if (positions.Count > bodyParts.Count + 1)
            positions.RemoveAt(positions.Count - 1);

        transform.position = positions[0];

        for (int i = 0; i < bodyParts.Count; i++)
            bodyParts[i].position = positions[i + 1];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            AddScore(1);
            foodSpawner?.SpawnFood();
            Destroy(other.gameObject);
        }
    }

    void Grow()
    {
        Transform body = Instantiate(bodyPrefab);
        body.position = positions[^1];
        bodyParts.Add(body);
        positions.Add(body.position);
    }

    void GameOver()
    {
        isGameOver = true;
        CancelInvoke();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
    }

    void AddScore(int value)
    {
        score += value;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
