using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;


public class ResourceManager : Singleton<ResourceManager>
{
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            return;
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (op) =>
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }
}
