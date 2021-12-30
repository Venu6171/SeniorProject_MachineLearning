using UnityEngine;
using UnityEngine.UI;

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

    public IntelligenceType modelIntelligence;
    public int modelIteration = 0;
    public bool isModel = false;

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
        modelIntelligence = IntelligenceType.Naive;
        modelIteration = 0;
    }

    public void SetModelIteration()
    {
        switch (modelIntelligence)
        {
            case IntelligenceType.Naive:
                modelIteration = 500;
                break;
            case IntelligenceType.Smart:
                modelIteration = 5000;
                break;
            case IntelligenceType.Human:
                modelIteration = 10000;
                break;
        }
    }

    public int GetModelIteration()
    {
        return modelIteration;
    }

    public bool GetInputType()
    {
        return isModel;
    }
}
