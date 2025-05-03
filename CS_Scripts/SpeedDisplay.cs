    using UnityEngine;
    using TMPro;  // Text Mesh Proの名前空間を追加
    
    public class SpeedDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI speedText;  // TextをTextMeshProUGUIに変更
    
        [SerializeField]
        private Rigidbody vehicleRigidbody;
    
        [SerializeField]
        private float speedMultiplier = 3.6f;
    
        private void Update()
        {
            if (vehicleRigidbody != null)
            {
                float speedMS = vehicleRigidbody.velocity.magnitude;
                float speedKMH = speedMS * speedMultiplier;
                speedText.text = speedKMH.ToString("F1") + " km/h";
            }
        }
    }
