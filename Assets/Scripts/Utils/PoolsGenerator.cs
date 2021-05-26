using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolData
{
    public GameObject PoolPrefab;
    public uint PoolSize;
}

public class PoolsGenerator : MonoBehaviour
{
    public PoolData[] DesiredPools;
    private Dictionary<PoolType, Queue<IPool>> pools;

    public void Awake()
    {
        pools = new Dictionary<PoolType, Queue<IPool>>();
        GeneratePools();
    }

    private void GeneratePools()
    {
        if (DesiredPools.Length > 0)
        {
            GameObject poolsContainer = new GameObject("PoolsContainer");

            for (int i = 0; i < DesiredPools.Length; i++)
            {
                IPool modelInterface = DesiredPools[i].PoolPrefab.GetComponent<IPool>();
                if(modelInterface == null)
                {
                    Debug.LogError("Pool generation error: " + DesiredPools[i].PoolPrefab.name + " prefab has no IPool interface");
                    continue;
                }

                if (DesiredPools[i].PoolSize <= 0)
                {
                    Debug.LogError("Pool generation error: " + DesiredPools[i].PoolPrefab.name + " pool size is <= 0");
                    continue;
                }

                foreach (PoolType t in pools.Keys)
                {
                    if (t == modelInterface.PoolType)
                    {
                        Debug.LogError("Pool generation error: " + modelInterface.PoolType.ToString() + " pool already exists");
                        continue;
                    }
                }

                Queue<IPool> pool = new Queue<IPool>();

                for (int j = 0; j < DesiredPools[i].PoolSize; j++)
                {
                    GameObject obj = Instantiate(DesiredPools[i].PoolPrefab, poolsContainer.transform);

                    IPool script = obj.GetComponent<IPool>();

                    obj.SetActive(false);
                    pool.Enqueue(script);
                }
                pools.Add(modelInterface.PoolType, pool);
            }

            PoolsManager.Init(pools);
            Destroy(gameObject);
        }
    }
}
