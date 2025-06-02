using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int totalScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("Puan sýfýrlandý");
    }
    public void AddScore(int amount)
    {
        totalScore += amount;
        Debug.Log("Puan" + totalScore);
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}
