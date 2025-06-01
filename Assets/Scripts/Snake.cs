using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Snake : MonoBehaviour
{
    public  AudioClip eatSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    private Vector2 _direction = Vector2.right;
    public float speed=5;
    Rigidbody2D rb;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    int coinCount = 0;
    private int score;
    [SerializeField] private TMP_Text scoreText;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }
    private void Update()
    {
        scoreText.text = score.ToString();
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for(int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        Vector2 position = rb.position + _direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }
    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void ResetState()
    {
       
        for (int i=1;i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);


        this.transform.position = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            score++;
            speed= speed + 0.4f;
            Grow();
            if (audioSource != null && eatSound != null)
            {
               audioSource.PlayOneShot(eatSound);
            }
        }
        else if (other.tag == "Obstacle")
        {
            if (audioSource != null && gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }
            ResetState();
        }
       

    }

}
