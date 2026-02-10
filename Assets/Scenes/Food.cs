using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;

    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    private GameObject currentFood;

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

        Vector2 spawnPos = new Vector2(
            Mathf.Round(Random.Range(minX, maxX)),
            Mathf.Round(Random.Range(minY, maxY))
        );

        currentFood = Instantiate(foodPrefab, spawnPos, Quaternion.identity);
    }

    // 🔥 GỌI HÀM NÀY KHI RẮN ĂN
    public void EatFood(GameObject food)
    {
        if (food == currentFood)
        {
            Destroy(currentFood);
            currentFood = null;
            SpawnFood();
        }
    }
}
