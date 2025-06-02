// Snake.cs
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Snake : MonoBehaviour
{
    public AudioClip eatSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    private Vector2 _direction = Vector2.right;
    public float speed = 5f;
    private Rigidbody2D rb;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    [SerializeField] private TMP_Text scoreText;

    private float speedReductionPerSegment = 0.6f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _segments = new List<Transform> { transform };
    }

    private void Update()
    {
        scoreText.text = ScoreManager.Instance.GetTotalScore().ToString();

        if (Input.GetKeyDown(KeyCode.W)) _direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S)) _direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A)) _direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D)) _direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        Vector2 newPos = rb.position + _direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }
    public void GrowMultiple(int count)
    {
        for (int i = 0; i < count; i++)
        {
            speed += 0.4f; 
            Grow();        
        }
    }


    public void Shrink(int count)
    {
        int removeCount = Mathf.Min(count, _segments.Count - 1);
        for (int i = 0; i < removeCount; i++)
        {
            Transform segmentToRemove = _segments[_segments.Count - 1];
            _segments.RemoveAt(_segments.Count - 1);
            Destroy(segmentToRemove.gameObject);
            speed = Mathf.Max(0f, speed - speedReductionPerSegment);
        }
        if (_segments.Count <= 1)
            StartCoroutine(HandleGameOver());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            ScoreManager.Instance.AddScore(1);
            Grow();
            speed += 0.4f;

            if (audioSource != null && eatSound != null)
                audioSource.PlayOneShot(eatSound);

            yem fruitScript = other.GetComponent<yem>();
            if (fruitScript != null)
            {
                fruitScript.RandomizePosition();
                fruitScript.RandomizeSprite();
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (audioSource != null && gameOverSound != null)
                audioSource.PlayOneShot(gameOverSound);
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
