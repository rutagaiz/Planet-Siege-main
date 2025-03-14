using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
 
    public static ExperienceManager Instance;

    public delegate void ExperienceChangeHandler(int amountC, int amountE);
    public event ExperienceChangeHandler OnChange;
    public event ExperienceChangeHandler OnLevelUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Add(int amountC, int amountE)
    {
        OnChange?.Invoke(amountC,amountE);
    }
}
