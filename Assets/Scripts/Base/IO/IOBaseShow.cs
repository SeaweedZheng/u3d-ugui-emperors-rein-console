using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOBaseShow : IOBaseBtnSelection
{

    [HideInInspector]
    public Text titleText;

    [HideInInspector]
    public List<Text> contentList = new List<Text>();

    public override Color Color { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("Title")?.GetComponent<Text>();
        var rect = transform.Find("rect");
        for (int i = 0; i < rect.childCount; i++)
            contentList.Add(transform.Find($"rect/Content{i}").GetComponent<Text>());
    }

    public void SetContent(string title, List<string> contents)
    {
        if (titleText == null)
            titleText = transform.Find("Title")?.GetComponent<Text>();
        if (contentList.Count == 0)
        {
            var rect = transform.Find("rect");
            for (int i = 0; i < rect.childCount; i++)
                contentList.Add(transform.Find($"rect/Content{i}").GetComponent<Text>());
        }
       titleText.text = title;
        for (int i = 0; i < contentList.Count; i++)
        {
            if (contents.Count > i)
            {
                contentList[i].text = contents[i];
                contentList[i].gameObject.SetActive(true);
            }
        }
    }
}
