using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 设置")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeTime = .8f;
    public float blackScreenDuration = 0.5f;   
    private bool isPlayerDead = false;

  
    private GameObject player;
    private PlayerController playerController;
    private Rigidbody2D playerRb;              

    void Awake()
    {
    
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            
            
            if (fadeCanvasGroup != null)
            {
                DontDestroyOnLoad(fadeCanvasGroup.transform.root.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitPlayerReferences();
    }

    
    void InitPlayerReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    public void PlayerDeath()
    {
        if (isPlayerDead) return;
        isPlayerDead = true;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
       
        if (playerController != null) playerController.canMove = false;
        if (playerRb != null) playerRb.velocity = Vector2.zero; 

      
        yield return StartCoroutine(Fade(1f));

        
        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneName);

      
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

      
        InitPlayerReferences(); 

        if (CheckPointManager.Instance != null && player != null)
        {
           
            player.transform.position = CheckPointManager.Instance.lastCheckpointPosition;
            
            if (playerController != null) playerController.canMove = false;
        }

      
        yield return new WaitForSeconds(blackScreenDuration);

     
        yield return StartCoroutine(Fade(0f));

     
        if (playerController != null) playerController.canMove = true; 

        isPlayerDead = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float speed = 1f / fadeTime;
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }


    private void ResetAllDetectors()
    {
        Detect[] allDetectors = FindObjectsOfType<Detect>();
        foreach (var d in allDetectors)
        {
            d.enabled = true;
        }
    }
}