using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [HideInInspector] public bool isSoundOff = false;
    [HideInInspector] public StageData.StageType stageType = StageData.StageType.stage;

    public AudioMixerGroup audioMixer;

    public AudioSource click;
    public AudioSource cuttingApple;
    public AudioSource hitOnFruit;
    public AudioSource hitOnLog;
    public AudioSource hitOnKnife;
    public AudioSource openNewKnife;
    public AudioSource destroyLog;
    public AudioSource destroyFruit;
    public AudioSource victory;


    private void Start()
    {
        Instance = this;
        GameManager.OnCollision += OnHit;

        isSoundOff = SaveObject.Save.isSoundOff;
        ToggleSound(isSoundOff);
    }

    public void ToggleSound(bool isTurnOn)
    {
        isSoundOff = isTurnOn;
        SaveObject.Save.isSoundOff = isTurnOn;

        if (!isSoundOff) 
            audioMixer.audioMixer.SetFloat("SoundVolume", 0);
        else 
            audioMixer.audioMixer.SetFloat("SoundVolume", -80);
    }

    private void OnHit(Collider2D col)
    {
        string tag = col.tag;
        float pitch = Random.Range(0.8f, 1.2f);
        switch (tag)
        {
            
            case "Target":
                if (stageType == StageData.StageType.stage)
                {
                    hitOnLog.pitch = pitch;
                    hitOnLog.Play();
                }                  
                else
                {
                    hitOnFruit.pitch = pitch;
                    hitOnFruit.Play();
                }
                break;
            case "Apple":
                cuttingApple.pitch = pitch;
                cuttingApple.Play();
                break;
            case "Knife":
                hitOnKnife.pitch = pitch;
                hitOnKnife.Play();
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.OnCollision -= OnHit;
    }
}
