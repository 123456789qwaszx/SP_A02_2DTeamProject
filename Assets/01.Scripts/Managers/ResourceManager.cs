using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;


public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, Object> _resources = new Dictionary<string, Object>();


    // 키 값을 이용해 _resources에 등록된 프리팹을 생성함
    // parent를 등록 시 SetParent, 대신 Pooling시에는 PoolManager에서 자동으로 Root를 SetParent 하기에 등록 불필요
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }

        if (pooling)
            return PoolManager.Instance.Pop(prefab);

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    // 키 값을 이용해 데이터를 로드 하는 용도
    // HierArchy 창에 객체를 생성하지 않음
    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
            return resource as T;

        return null;
    }


    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (PoolManager.Instance.Push(go))
            return;

        Object.Destroy(go);
    }

    #region Addressable
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
    {
        // 만약 이전에 사용한 적이 있으면, 키 값을 이용해서 찾는다. (캐시 확인.)
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        // 만약 못 찾았으면 어드레서블을 이용해 로드하는 방식. (리소스 비동기 로딩 시작.)
        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (op) =>
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    // 원하는 라벨이 붙어있는 모든 것들을 로드해서 _resources에 넣어 줌
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                //PrimaryKey = 방금 내가 로드한 파일의 키값
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
    #endregion
}