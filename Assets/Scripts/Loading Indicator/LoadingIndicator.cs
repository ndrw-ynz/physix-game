using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
    public static LoadingIndicator Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        this.gameObject.SetActive(false);
    }
}
