using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    [Header("Troop Key Bindings")]
    [SerializeField] private Button[] troopKeyButtons = new Button[5]; // Assign in Inspector (Troop1-5 buttons)
    [SerializeField] private TMP_Text[] troopKeyTexts; // Changed from Text to TMP_Text

    private KeyCode[] troopKeys = new KeyCode[5] {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5
    };
    private int currentlyRebindingIndex = -1; // -1 = not rebinding

    void Start()
    {
        Initialize(); // Initialize if not already done
        // Set up button click listeners for all 5 troop keys
        for (int i = 0; i < 5; i++)
        {
            int index = i; // Capture index for closure
            troopKeyButtons[i].onClick.AddListener(() => StartRebinding(index));
        }
        UpdateAllKeyDisplays();
    }

    void Update()
    {
        if (currentlyRebindingIndex != -1 && Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Escape)
                    {
                        CancelRebinding();
                        return;
                    }

                    // Assign the new key
                    troopKeys[currentlyRebindingIndex] = key;
                    CancelRebinding();
                    return;
                }
            }
        }
    }

    private void StartRebinding(int troopIndex)
    {
        currentlyRebindingIndex = troopIndex;
        troopKeyTexts[troopIndex].text = "Press a key...";
    }

    private void CancelRebinding()
    {
        currentlyRebindingIndex = -1;
        UpdateAllKeyDisplays();
    }

    private void UpdateAllKeyDisplays()
    {
        for (int i = 0; i < 5; i++)
        {
            troopKeyTexts[i].text = troopKeys[i].ToString().Replace("Alpha", ""); // Shows "1" instead of "Alpha1"
        }
    }

    // Getter for other scripts
    public KeyCode GetTroopKey(int index) => troopKeys[index];

    private bool isInitialized = false;

    public bool IsInitialized() => isInitialized;

    public void Initialize()
    {
        if (isInitialized) return;

        // Validate UI elements
        if (troopKeyButtons.Length != 5 || troopKeyTexts.Length != 5)
        {
            Debug.LogError("Assign all 5 buttons and text fields in Inspector!");
            return;
        }

        // Set up listeners
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            troopKeyButtons[i]?.onClick.AddListener(() => StartRebinding(index));
        }

        UpdateAllKeyDisplays();
        isInitialized = true;
    }




}