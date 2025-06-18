using System.IO;
using UnityEngine;
using Newtonsoft.Json;  // JSON.NET 사용 시

public class SaveManager : Singleton<SaveManager>
{
    private string savePath;

    private void Awake()
    {
        // 예시: 데이터를 저장할 경로 설정
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log("Save path: " + savePath);

        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // SaveData 객체를 JSON 파일에 저장하는 함수
    public void SaveGame(SaveData data)
    {
        // JSON 직렬화를 통해 SaveData 객체를 문자열로 변환
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("게임 저장 완료");
    }

    // 저장된 JSON 파일을 읽어 SaveData 객체로 복원하는 함수
    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log("게임 불러오기 완료");
            return data;
        }
        else
        {
            Debug.LogWarning("저장된 파일이 없습니다.");
            return null;
        }
    }
}