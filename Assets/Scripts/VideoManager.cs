using System.Security.Cryptography;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    public static VideoManager Instance;

    //Refs
    private VideoPlayer player;

    //Runtime
    private bool isVideoPlaying = true;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        Instance = this;
    }
    private void Start()
    {
        PlayOrPauseVideo();
    }

    public void SetVideo(VideoClip videoToPlay)
    {
        if (videoToPlay != null)
            player.clip = videoToPlay;
        else
            Debug.LogError("You didn't set a video for this jar, dimwit");
    }

    public void PlayOrPauseVideo()
    {
        if (isVideoPlaying)
        {
            player.Pause();
        }
        else
            player.Play();
        isVideoPlaying = !isVideoPlaying;
    }

}
