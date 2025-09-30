using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDeviceItemView : MonoBehaviour
{
    private Image bg;
    private Text gameName;
    private Text macId;
    private List<Sprite> bgSprites;

    private bool isInit;

    private void Start()
    {
        InitView();
    }

    private void InitView()
    {
        if (isInit) return;
        bg = transform.Find("bg").GetComponent<Image>();
        gameName = transform.Find("gameName").GetComponent<Text>();
        macId = transform.Find("macId").GetComponent<Text>();
        bgSprites = new List<Sprite>();
        for (int i = 0; i < 4; i++)
        {
            // 从 AssetBundle 中加载 Texture2D
            Texture2D texture = ResMgr.Instance.LoadObjectFromAssetBundle("game", $"deviceBG{i}") as Texture2D;
            if (texture != null)
            {
                // 将 Texture2D 转换为 Sprite
                Sprite sprite = Texture2DToSprite(texture);
                bgSprites.Add(sprite);
            }
            else
            {
                Debug.LogError($"Failed to load Texture2D: deviceBG{i}");
            }
        }
        isInit = true;
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void SetViewData(int idx, Player player)
    {
        InitView();
        idx %= 4;
        bg.sprite = bgSprites[idx];
        //gameName.text = Model.Instance.GetGameName(player.gameType);
        this.macId.text = player.macId.ToString();
        Utils.SetUIGray(gameObject, !player.IsOnline);
        gameObject.SetActive(true);
    }
}
