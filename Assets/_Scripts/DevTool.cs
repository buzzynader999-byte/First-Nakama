using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
//using RTLTMPro;
using UnityEngine;

public class DevTool : MonoBehaviour
{
    [SerializeField] bool showDevTool = false;
    //[SerializeField] private GameConfig gameConfig;
    //[SerializeField] private RTLTextMeshPro rtlTextFixer;

    bool showInfoLabel;
    string infoTextSize;


    private string deviceId = string.Empty;
    private string ipText = string.Empty;


    private void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            Debug.Log("Mouse clicked at: " + Event.current.mousePosition);
        }

        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;
        style.active.textColor = Color.green;
        style.fontSize = 35;

        if (showDevTool)
        {
            GUILayout.BeginVertical();

            ShowCloseBtn(style);


            GUILayout.BeginHorizontal();


            deviceId = GUILayout.TextField(deviceId, style, GUILayout.Height(Screen.height / 15),
                GUILayout.Width(Screen.width / 2));
            if (GUILayout.Button("Reload", style, GUILayout.Height(Screen.height / 15),
                    GUILayout.Width(Screen.width / 2)))
            {
            }

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            ipText = GUILayout.TextField(ipText, style, GUILayout.Height(Screen.height / 15),
                GUILayout.Width(Screen.width / 2));
            if (GUILayout.Button("ip", style, GUILayout.Height(Screen.height / 15),
                    GUILayout.Width(Screen.width / 2)))
            {
                PlayerPrefs.SetString("nakama host", ipText);
                var h = PlayerPrefs.GetString("nakama host", "0.0.0.0");
                Services.Get<NetworkManager>().Connection.Host = h;
            }

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (!showInfoLabel)
            {
                infoTextSize = GUILayout.TextField(infoTextSize, style, GUILayout.Height(Screen.height / 15),
                    GUILayout.Width(Screen.width / 4));
                if (GUILayout.Button("ShowInfo", style, GUILayout.Height(Screen.height / 15),
                        GUILayout.Width(Screen.width / 4)))
                {
                    showInfoLabel = true;
                }
            }
            else
            {
                if (GUILayout.Button("HideInfo", style, GUILayout.Height(Screen.height / 15),
                        GUILayout.Width(Screen.width / 2)))
                {
                    showInfoLabel = false;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        else
        {
            ShowDevToolBtn(style);

            if (showInfoLabel)
            {
                //show info
            }
        }
    }


    private void ShowDevToolBtn(GUIStyle gUIStyle)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("DevTool", gUIStyle, GUILayout.Height(Screen.height / 25),
                GUILayout.Width(Screen.width / 5)))
        {
            showDevTool = true;
        }

        GUILayout.EndHorizontal();
    }


    private void ShowCloseBtn(GUIStyle gUIStyle)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Close", gUIStyle, GUILayout.Height(Screen.height / 15),
                GUILayout.Width(Screen.width)))
        {
            showDevTool = false;
        }

        GUILayout.EndHorizontal();
    }
}