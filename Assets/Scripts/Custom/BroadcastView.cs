using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastView : MonoBehaviour
{

    private float width;
    private float moveTime = 60f;
    private float intervalTime = 6f;
    private int curIdx;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<RectTransform>().rect.width;

        StartCoroutine(ScrollBroadcastText());
    }

    private IEnumerator ScrollBroadcastText()
    {
        while (true)
        {
            PoolMgr.Instance.GetObj("broadcastText", false, "game", callBack: (obj) =>
            {
                obj.transform.SetParent(transform);
                obj.name = "broadcastText";
                obj.transform.localPosition = new Vector3(width, 0, 0);
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.SetAsLastSibling();
                var rect = obj.GetComponent<RectTransform>();
                BroadcastText broadcastText = obj.GetComponent<BroadcastText>();
                broadcastText.SetText(Model.Instance.broadCastNames[curIdx]);
                obj.SetActive(true);
                AsyncActionUtils.ApplyAnchoredMovement(this, rect, new Vector2(width + rect.rect.width, 0), new Vector2(-rect.rect.width, 0), moveTime, TweenUtils.VectorTweenLinear, onComplete: () =>
                {
                    PoolMgr.Instance.PushObj("broadcastText", obj);
                });
            });
            intervalTime = Model.Instance.broadCastNames[curIdx] switch
            {
                "文化部准入机型" => 9,
                "多机台组合式联机奖励系统" => 15,
                _ => (float)6,
            };
            curIdx = curIdx + 1 > Model.Instance.broadCastNames.Count - 1 ? 0 : curIdx + 1;
            yield return new WaitForSeconds(intervalTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
