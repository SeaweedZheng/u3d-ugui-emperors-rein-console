using UnityEngine;
using UnityEngine.UI;

public class IOClientUSNDataSection : MonoBehaviour
{
    public Text USNText;
    public Text isLegal;
    public Text delete;
    public Button button;
    public Button legalBtn;
    public Button deleteBtn;
    //private ClientUSNData clientUSNData;

    private void Start()
    {
        legalBtn.onClick.AddListener(OnLegalBtnClick);
        deleteBtn.onClick.AddListener(OnDeleteBtnClick);
    }

    //public void SetTextContent(ClientUSNData clientUSNData)
    //{
    //    this.clientUSNData = clientUSNData;
    //    USNText.text = InsertDashEvery4Chars(clientUSNData.USN);
    //    isLegal.text = clientUSNData.isLegal ? Utils.GetLanguage("ClientLegal") : Utils.GetLanguage("ClientUnlegal");
    //    delete.text = Utils.GetLanguage("DELETE");
    //}

    private string InsertDashEvery4Chars(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (i > 0 && i % 4 == 0)
                sb.Append("-");
            sb.Append(input[i]);
        }
        return sb.ToString();
    }

    private void OnLegalBtnClick()
    {
        //clientUSNData.isLegal = !clientUSNData.isLegal;
        //isLegal.text = clientUSNData.isLegal ? Utils.GetLanguage("ClientLegal") : Utils.GetLanguage("ClientUnlegal");
        //if (clientUSNData.isLegal)
        //    NetModel.Instance.legalClientUSNList.Add(clientUSNData.USN);
        //else
        //    NetModel.Instance.legalClientUSNList.Remove(clientUSNData.USN);
        //SQLite.Instance.UpdateClientUSNData(clientUSNData);
    }

    private void OnDeleteBtnClick()
    {
        //clientUSNData.isDelete = !clientUSNData.isDelete;
        //clientUSNData.isLegal = false;
        //SQLite.Instance.UpdateClientUSNData(clientUSNData);
        //NetModel.Instance.legalClientUSNList.Remove(clientUSNData.USN);
        //EventCenter.Instance.EventTrigger(EventHandle.REFRESH_CLIENT_DATA_PANEL);
    }
}
