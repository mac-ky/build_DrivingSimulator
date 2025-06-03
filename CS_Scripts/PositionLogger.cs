using System;
using System.IO;
using UnityEngine;

public class PositionLogger : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        // Assets/GetTrajectory_own �Ƀ��O�t�@�C�����쐬
        filePath = Application.dataPath + "/GetTrajectory_own/PositionLog.csv";

        // �w�b�_�[����������
        File.WriteAllText(filePath, "time,Position_x,Position_y,Position_z\n");

        // 0.1�b�Ԋu��LogPosition()���J��Ԃ����s
        InvokeRepeating("LogPosition", 0f, 0.1f);

        Debug.Log("���O�o�͊J�n: " + filePath);
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
