using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    // singleton
    private static LevelData _instance;
    public static LevelData Instance
    {   
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelData>();
                if (_instance == null)
                {
                    GameObject levelData = new GameObject("LevelData");
                    _instance = levelData.AddComponent<LevelData>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    
    // Gamelevel  1level = 3*4, 2level = 4*4, 3level = 4*5
    private int gameLevel = 1;
    public int GameLevel => gameLevel;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetLevel(int i)
    {
        gameLevel = i;
    }
}