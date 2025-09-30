using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

namespace Demo
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public GameObject item;
        public int totalCount = -1;
        public int idxRecord;
        public int value;
        public int heightValue;

        // Implement your own Cache Pool here. The following is just for example.
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(item);
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            // Use `DestroyImmediate` here if you don't need Pool
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", idx);
        }

        void Start()
        {
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = totalCount;
            ls.RefillCells();
        }

        void Update()
        {
            var loopScrollRect = GetComponent<LoopScrollRect>();
            int tempIdx = loopScrollRect.GetLastItem(out _);
            value = heightValue * 10 + (tempIdx % 10);
            if (tempIdx != idxRecord && tempIdx % 10 == 0)
            {
                heightValue += 1;
                Debug.LogError("GO There");
            }
            if (tempIdx != idxRecord)
                idxRecord = tempIdx;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScrollByTimes(11);
            }

            Debug.Log("curValue = " + (value));
        }

        public void ScrollByTimes(int times)
        {
            var loopScrollRect = GetComponent<LoopScrollRect>();
            loopScrollRect.ScrollToCell(idxRecord + times, 400);
        }
    }
}