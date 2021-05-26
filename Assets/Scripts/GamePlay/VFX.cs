using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX : MonoBehaviour, IPool
{
    private VisualEffect effect;

    public PoolType PoolType { get { return PoolType.VFX; } }

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    public void SetEffect(VisualEffectAsset asset)
    {
        effect.visualEffectAsset = asset;
    }

    protected IEnumerator AutoDisable()
    {
        yield return new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);
    }
}
