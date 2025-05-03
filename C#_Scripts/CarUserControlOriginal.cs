using System;
using UnityEngine;

namespace LogitechSDK
{
    [RequireComponent(typeof(CarControllerOriginal))]
    public class CarUserControlOriginal : MonoBehaviour
    {
        private CarControllerOriginal m_Car; // the car controller we want to use

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarControllerOriginal>();
            Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
        }

        private void FixedUpdate()
        {
            if (!LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING))
            {
                // ハンドルに中心に向けた力を加えるように設定
                LogitechGSDK.LogiPlaySpringForce(0, 0, 30, 100);
            }
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                //CONTROLLER STATE
                LogitechGSDK.DIJOYSTATE2ENGINES rec = LogitechGSDK.LogiGetStateUnity(0);
                float steering = rec.lX / 32768f;               // ハンドル
                float accel = rec.lY / 65536f + 0.5f;           // アクセル
                float brake = rec.lRz / 65536f + 0.5f;          // ブレーキ
                float back = rec.rglSlider[0] / 65536f + 0.5f;  // バック
                Debug.Log(steering);

#if !MOBILE_INPUT
                float handbrake;
                handbrake = 0; // 今回はハンドブレーキは、なしで
                if (System.Math.Abs(steering) < 0.01) steering = 0; // 中心に向けた力が加わらない範囲は直進
                m_Car.Move(steering, 0.6f + back * 0.5f - accel, 1 - brake, handbrake);
#else
                m_Car.Move(steering, 1, 0, 0);
#endif
            }
        }
    }
}