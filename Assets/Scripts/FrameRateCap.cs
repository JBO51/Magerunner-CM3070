//Small script for capping fps
using UnityEngine;

public class FrameRateCap : MonoBehaviour
{
    [SerializeField] private int maxFrameRate = 144;

    void Awake()
    {
        //Tells Unity to target a specific frame rate
        Application.targetFrameRate = maxFrameRate;
    }
}