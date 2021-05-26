using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolsManager
{
    private static Dictionary<PoolType, Queue<IPool>> pools;
 
    /// <summary>
    /// Initialize the PoolsManager with the given set of pools.
    /// </summary>
    /// <param name="generatedPools"></param>
    public static void Init(Dictionary<PoolType, Queue<IPool>> generatedPools)
    {
        pools = generatedPools;
    }

    /// <summary>
    /// Get an object from the given pool type. Remember to cast it to the right type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="immediateRecycle"></param>
    /// <returns></returns>
    public static IPool Get(PoolType type, bool immediateRecycle = true)
    {
        IPool obj = pools[type].Dequeue();

        if(immediateRecycle)
            Recycle(obj);

        return obj;
    }

    /// <summary>
    /// Manually recycle the given item into the relative pool.
    /// You should only call this method manually if when calling the Get method you passed false to immediateRecycle.
    /// </summary>
    /// <param name="item"></param>
    public static void Recycle(IPool item)
    {
        pools[item.PoolType].Enqueue(item);
    }
}
