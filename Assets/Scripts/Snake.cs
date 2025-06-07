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
    public float speed = 10f;   // Başlangıç hızı, Inspector’dan ≥8 yapın

    private Rigidbody2D rb;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    [SerializeField] private TMP_Text scoreText;

    private float speedReductionPerSegment = 2f;
    private bool isGameOver = false;

    // Joystick benzeri dokunma için
    private Vector2 touchStartPos;
    private bool isDragging = false;

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
        if (isGameOver) return;

        scoreText.text = ScoreManager.Instance.GetTotalScore().ToString();

        // 1) Joystick benzeri dokunma kontrolü (Mobil)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = t.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (!isDragging) break;

                    Vector2 touchCurrentPos = t.position;
                    Vector2 dragVector = touchCurrentPos - touchStartPos;

                    if (dragVector.magnitude < 50f) break; // Eşik değeri

                    // Hareket yönünü sürekli güncelle
                    if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
                    {
                        if (dragVector.x > 0 && _direction != Vector2.left)
                            _direction = Vector2.right;
                        else if (dragVector.x < 0 && _direction != Vector2.right)
                            _direction = Vector2.left;
                    }
                    else
                    {
                        if (dragVector.y > 0 && _direction != Vector2.down)
                            _direction = Vector2.up;
                        else if (dragVector.y < 0 && _direction != Vector2.up)
                            _direction = Vector2.down;
                    }

                    // touchStartPos’u her karede güncelleyerek sürekli yön değişimini joystick gibi yap
                    touchStartPos = touchCurrentPos;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
        else
        {
            // 2) Klavye kontrolleri (PC için)
            if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
                _direction = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
                _direction = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
                _direction = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
                _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver) return;

        Vector2 newPos = rb.position + _direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
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
            Grow();
        }
        speed += 5f;
    }

    public void Shrink(int count)
    {
        if (_segments.Count > 1)
        {
            int removeCount = Mathf.Min(count, _segments.Count - 1);
            for (int i = 0; i < removeCount; i++)
            {
                Transform segmentToRemove = _segments[_segments.Count - 1];
                _segments.RemoveAt(_segments.Count - 1);
                Destroy(segmentToRemove.gameObject);
                speed = Mathf.Max(0f, speed - speedReductionPerSegment);
            }
            speed = Mathf.Max(speed, 8f);
        }
        else
        {
            StartCoroutine(HandleGameOver());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

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
        // BombCollision içinde Shrink çağrılır
    }

    private IEnumerator HandleGameOver()
    {
        isGameOver = true;
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
