using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    private static LevelManager Instance;
    public static LevelManager GetInstance()
    {
        return Instance;
    }

    private static ObjectPool<GameObject> _objectPool;
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private int _poolAmount;

    private Vector3 lastEndPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Level_Manager Instance created");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Level_Manager duplicate destroyed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _objectPool = new ObjectPool<GameObject>(() =>
        { return Instantiate(_levelPrefab); },
        level => { level.gameObject.SetActive(true); },
        level => { level.gameObject.SetActive(false); },
        level => { Destroy(level.gameObject); },
        false, 1, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnLevel()
    {
        for (var i = 0; i < _poolAmount; ++i)
        {
            var level = _objectPool.Get();
            level.transform.position = _levelPrefab.transform.position + new Vector3(-2.1500001f, 0.0f, 379.779999f);
        }
    }

    public void ReleaseObjects()
    {
        _objectPool.Release(_levelPrefab);
    }
}
