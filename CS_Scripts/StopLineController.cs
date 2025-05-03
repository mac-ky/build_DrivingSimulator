using UnityEngine;
using System.Collections;

public class StopLineController : MonoBehaviour
{
    public GameObject targetVehicle; // トリガー対象の車両

    // 出現するオブジェクト
    public GameObject showObject1;
    public GameObject showObject2;
    public float AppearanceDelay = 3f; // トリガー後に出現するまでの時間（デフォルト3秒）
    public float Duration = 92f; // 表示時間

    private void Start()
    {
        // 初期状態ではすべてのオブジェクトを非表示
        if (showObject1 != null) showObject1.SetActive(false);
        if (showObject2 != null) showObject2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetVehicle)
        {
            StartCoroutine(TriggerSequence());
        }
    }

    private IEnumerator TriggerSequence()
    {
        // 2つのオブジェクトを指定秒後に出現させ、一定時間後に非表示
        yield return new WaitForSeconds(AppearanceDelay);
        yield return StartCoroutine(ShowAndHideTwoObjects(showObject1, showObject2, Duration));

    }

    private IEnumerator ShowAndHideTwoObjects(GameObject obj1, GameObject obj2, float duration)
    {
        if (obj1 != null) obj1.SetActive(true);
        if (obj2 != null) obj2.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (obj1 != null) obj1.SetActive(false);
        if (obj2 != null) obj2.SetActive(false);
    }
}
