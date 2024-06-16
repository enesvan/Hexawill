using UnityEngine;

public class Manager : MonoBehaviour {
    public virtual void AwakeManager() => Debug.Log($"{name} is initialized!");
}