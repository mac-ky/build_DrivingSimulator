using UnityEngine;
using System.Collections;

public class SignalIndicationController : MonoBehaviour
{
    public GameObject targetVehicle; // トリガー対象の車両

    // 表示するオブジェクトとその表示時間
    public GameObject objectToShow;
    public float objectToShowDuration = 3f;

    public GameObject secondObject1;
    public GameObject secondObject2;
    public float secondObjectDuration = 9f;

    public GameObject thirdObject;
    public float thirdObjectDuration = 3f;

    public GameObject fourthObject;
    public float fourthObjectDuration = 80f;

    public GameObject finalObject; // 常に表示

    // トリガー通過時に即座に非表示にするオブジェクト
    public GameObject objectToHide;

    private void Start()
    {
        // 初期状態でオブジェクトを非表示
        if (objectToShow != null) objectToShow.SetActive(false);
        if (secondObject1 != null) secondObject1.SetActive(false);
        if (secondObject2 != null) secondObject2.SetActive(false);
        if (thirdObject != null) thirdObject.SetActive(false);
        if (fourthObject != null) fourthObject.SetActive(false);

        // `finalObject` は初期状態から表示する
        if (finalObject != null) finalObject.SetActive(true);

        // 初期状態で非表示にする対象オブジェクトは表示状態のまま
        if (objectToHide != null) objectToHide.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetVehicle)
        {
            // すぐに非表示にするオブジェクト
            if (objectToHide != null)
            {
                objectToHide.SetActive(false);
            }

            // 順番にオブジェクトを表示
            StartCoroutine(ShowSequence());
        }
    }

    private IEnumerator ShowSequence()
    {
        yield return StartCoroutine(ShowAndHide(objectToShow, objectToShowDuration));
        
        // `secondObject1` と `secondObject2` を同時に表示
        yield return StartCoroutine(ShowAndHideTwoObjects(secondObject1, secondObject2, secondObjectDuration));
        
        yield return StartCoroutine(ShowAndHide(thirdObject, thirdObjectDuration));
        yield return StartCoroutine(ShowAndHide(fourthObject, fourthObjectDuration));
        // 最後のオブジェクトはずっと表示させておく
        yield return StartCoroutine(ShowAndHide(finalObject, 1000));
    }

    private IEnumerator ShowAndHide(GameObject obj, float duration)
    {
        if (obj != null)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(duration);
            obj.SetActive(false);
        }
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
