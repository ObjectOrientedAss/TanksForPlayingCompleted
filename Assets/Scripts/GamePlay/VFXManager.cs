using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    private static Dictionary<string, VisualEffectAsset> effects;

    public void Awake()
    {
        effects = new Dictionary<string, VisualEffectAsset>();
        VisualEffectAsset[] visualEffects = Resources.LoadAll<VisualEffectAsset>("VFX/");
        for (int i = 0; i < visualEffects.Length; i++)
        {
            effects.Add(visualEffects[i].name, visualEffects[i]);
        }

        Resources.UnloadUnusedAssets();
        GP_EventSystem.OnShootEvent += OnShoot;

        DontDestroyOnLoad(gameObject);
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
                PlayVFX("MachineGunMuzzle", data.Position, data.Forward);
                break;
            case PoolType.CannonBullet:
                PlayVFX("MachineGunMuzzle", data.Position, data.Forward);
                break;
            case PoolType.MortarBullet:
                PlayVFX("MachineGunMuzzle", data.Position, data.Forward);
                break;
        }
    }

    public static void PlayVFX(string vfxName, Vector3 position, Vector3 forward)
    {
        VFX vfx = (VFX)PoolsManager.Get(PoolType.VFX);
        vfx.transform.position = position;
        vfx.transform.forward = forward;
        vfx.SetEffect(effects[vfxName]);
        vfx.gameObject.SetActive(true);
    }
}