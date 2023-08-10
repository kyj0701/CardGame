using UnityEngine;

public class LevelToggle : MonoBehaviour
{
    public void SetLevel01()
    {
        LevelData.Instance.SetLevel(1);
    }
    public void SetLevel02()
    {
        LevelData.Instance.SetLevel(2);
    }    
    public void SetLevel03()
    {
        LevelData.Instance.SetLevel(3);
    }    
}
