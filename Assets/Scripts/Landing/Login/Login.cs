using UnityEngine;
using TMPro;
using System.Collections;

public class Login : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loginLog;

    public void OnLogin(string _userInput, string _passInput)
    {
        if (_userInput.Length > 0 && _passInput.Length > 0)
        {
            StartCoroutine(LogTextController(loginLog, "Logging in..."));
            APIManager.Instance.PostLogin(_userInput, _passInput);
        }
        else
        {
            StartCoroutine(LogTextController(loginLog, "Masukkan Username dan Password dengan benar!"));
        }
    }

    WaitForSeconds wait = new WaitForSeconds(3);
    public IEnumerator LogTextController(TextMeshProUGUI _log, string _logTxt)
    {
        loginLog.text = _logTxt;
        yield return wait;
        loginLog.text = "";
    }
}
