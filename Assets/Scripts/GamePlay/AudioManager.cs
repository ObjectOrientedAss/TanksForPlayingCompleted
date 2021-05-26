using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource sfxSource; //set in inspector
    private static Dictionary<string, AudioClip> effects;

    private static AudioSource musicSource; //set in inspector
    private static Dictionary<string, AudioClip> musics;

    public static float MusicVolume { get { return musicSource.volume; } set { musicSource.volume = value; } }
    public static float EffectsVolume { get { return sfxSource.volume; } set { sfxSource.volume = value; } }

    public void Awake()
    {
        AudioClip[] clips;

        effects = new Dictionary<string, AudioClip>();
        clips = Resources.LoadAll<AudioClip>("Audio/SFX/");
        for (int i = 0; i < clips.Length; i++)
        {
            effects.Add(clips[i].name, clips[i]);
        }

        musics = new Dictionary<string, AudioClip>();
        clips = Resources.LoadAll<AudioClip>("Audio/Musics/");
        for (int i = 0; i < clips.Length; i++)
        {
            musics.Add(clips[i].name, clips[i]);
        }
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = 0.05f;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 0.04f;
        musicSource.loop = true;

        Resources.UnloadUnusedAssets();
        DontDestroyOnLoad(gameObject);

        GP_EventSystem.OnShootEvent += OnShoot;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnShootEvent -= OnShoot;
    }

    private void OnShoot(Events.ShootData data)
    {
        switch (data.BulletType)
        {
            case PoolType.MachineGunBullet:
                PlaySFX("MG_Shot");
                break;
            case PoolType.CannonBullet:
                PlaySFX("Cannon_Shot");
                break;
            case PoolType.MortarBullet:
                PlaySFX("Mortar_Shot");
                break;
        }
    }

    /// <summary>
    /// Play a sound effect.
    /// </summary>
    /// <param name="sfx"></param>
    public static void PlaySFX(string sfx)
    {
        sfxSource.clip = effects[sfx];
        sfxSource.PlayOneShot(effects[sfx]);
    }

    /// <summary>
    /// Play a sound effect pitching between the given values.
    /// Beware that this will also pitch the other sound effects until the desired pitch is not restored again.
    /// </summary>
    /// <param name="sfx"></param>
    /// <param name="minPitch"></param>
    /// <param name="maxPitch"></param>
    public static void PlaySFX(string sfx, float minPitch, float maxPitch)
    {
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.clip = effects[sfx];
        sfxSource.PlayOneShot(effects[sfx]);
    }

    /// <summary>
    /// Play a music, optionally with a pitch.
    /// </summary>
    /// <param name="music"></param>
    /// <param name="pitch"></param>
    public static void PlayMusic(string music, float pitch = 1)
    {
        musicSource.pitch = pitch;
        musicSource.clip = musics[music];
        musicSource.Play();
    }
}