using System;
using System.IO;
using UnityEngine;

public class PositionLogger : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        // Assets/GetTrajectory_own にログファイルを作成
        filePath = Application.dataPath + "/GetTrajectory_own/PositionLog.csv";

        // ヘッダーを書き込み
        File.WriteAllText(filePath, "time,Position_x,Position_y,Position_z\n");

        // 0.1秒間隔でLogPosition()を繰り返し実行
        InvokeRepeating("LogPosition", 0f, 0.1f);

        Debug.Log("ログ出力開始: " + filePath);
    }

    void LogPosition()
    {
        Vector3 pos = transform.position;

        string logLine = string.Format(
            "{0:F3},{1:F5},{2:F5},{3:F5}",
            Time.time, pos.x, pos.y, pos.z
        );

        File.AppendAllText(filePath, logLine + "\n");
    }
}
