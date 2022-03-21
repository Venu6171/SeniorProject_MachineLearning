using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public enum IntelligenceType
    {
        Naive = 0,
        Smart = 1,
        Human = 2
    }

    private static UIManager Instance;
    public static UIManager GetInstance()
    {
        return Instance;
    }

    [System.Serializable]
    public class ModelIntelligence
    {
        public IntelligenceType modelIntelligence;
        public int modelIteration = 0;
    }

    public ModelIntelligence[] modelIntelligences;

    public bool isModel = false;
    public int currentIteration = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("UI_Manager Instance created");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("UI_Manager duplicate Destroyed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            if (GameManager.player.isModelTrained)
                GameManager.GetInstance().generationCountText.text = "Generation: " + GameManager.GetInstance().generationCount.ToString();
        }
    }

    public void SetModelIteration(int iteration)
    {
        foreach (var model in modelIntelligences)
        {
            if (model.modelIntelligence == (IntelligenceType)iteration)
                currentIteration = model.modelIteration;
        }
    }

    public int GetModelIteration()
    {
        return currentIteration;
    }

    public bool GetInputType()
    {
        return isModel;
    }

    public void DisplayGameOver()
    {
        GameManager.GetInstance().gameOverText.gameObject.SetActive(true);
    }

    public void DisplayGameWon()
    {
        GameManager.GetInstance().gameWonText.gameObject.SetActive(true);
    }
}
