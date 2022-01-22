using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeveloperConsole : MonoBehaviour
{
    private Dictionary<string, Action<string>> commands;
    private List<string> commandLog = new List<string>();
    private int commandLogIndex = 0;

    [SerializeField] private TMP_InputField input;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI log;
    [SerializeField] private GameObject content;

    public KeyCode consoleToggleKey = KeyCode.BackQuote;

    private float logWidth, logHeight;

    private void Start()
    {
        GetMinimumLogBounds();
        SetupListeners();
        SetupConsoleCommands();
    }

    private void Update()
    {
        if (Input.GetKeyDown(consoleToggleKey))
        {
            content.SetActive(!content.activeInHierarchy);

            if (content.activeInHierarchy)
                OnInputSelect();
            if (!content.activeInHierarchy)
                OnInputDeselect();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DecreaseCommandLogIndex();
            EnterCommandLogToInput();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            IncreaseCommandLogIndex();
            EnterCommandLogToInput();
        }
    }

    #region SETUP
    private void GetMinimumLogBounds()
    {
        logWidth = log.rectTransform.sizeDelta.x;
        logHeight = log.rectTransform.sizeDelta.y;
    }
    
    private void SetupListeners()
    {
        input.onSelect.AddListener(OnInputSelect);
        input.onDeselect.AddListener(OnInputDeselect);
        input.onSubmit.AddListener(HandleCommandSubmit);
    }

    // Any command that can be evaluated should be added to the dictionary in this function
    // Each of these functions must have only one string parameter
    // If the command doesn't require a parameter pass it empty string ""
    private void SetupConsoleCommands()
    {
        commands = new Dictionary<string, Action<string>>();

        commands.Add("commands", Commands);
        commands.Add("say", LogToConsole);
        commands.Add("clear", Clear);

        commands.Add("add", Add);
        commands.Add("multiply", Multiply);
        commands.Add("hello", Hello);
        commands.Add("love_you", LoveYou);
    }
    #endregion

    #region EVENTS
    public void OnInputSelect(string s = "")
    {
        input.ActivateInputField();

        // You need to deactivate the input of other GameObjects here.

        //CameraController.Singleton.ignoreAllInput = true; // This is just an example that was suitable for my case
    }

    public void OnInputDeselect(string s = "")
    {
        // You need to reactivate the input of other GameObjects here.

        //CameraController.Singleton.ignoreAllInput = false; // This is just an example that was suitable for my case
    }

    private void HandleCommandSubmit(string s)
    {
        input.text = "";
        input.ActivateInputField();
        RunCommand(s);
        
        if (s != "")
        {
            commandLog.Add(s);
            commandLogIndex = commandLog.Count;
        }
    }
    #endregion

    #region COMMAND LOG
    private void IncreaseCommandLogIndex()
    {
        if (commandLogIndex == commandLog.Count)
            return;
        else
            commandLogIndex++;
    }

    private void DecreaseCommandLogIndex()
    {
        if (commandLogIndex == 0)
            return;
        else
            commandLogIndex--;
    }

    private void EnterCommandLogToInput()
    {
        if (commandLogIndex == commandLog.Count)
            input.text = "";
        else
            input.text = commandLog[commandLogIndex];
    }
    #endregion

    /// <summary>
    /// Evaluates and runs the given command.
    /// {command} {params}
    /// </summary>
    /// <param name="s">Command to run</param>
    public void RunCommand(string s)
    {
        string[] split = s.Split(new string[] { " " }, 2, StringSplitOptions.None);
        string command = split[0];

        if (commands.ContainsKey(command))
        {
            string parameters = split.Length > 1 ? split[1] : "";
            commands[command].Invoke(parameters);
        }
        else
            LogToConsole($"Can't find command: '{s}'.Type 'commands' for a list of commands.");
    }

    #region COMMANDS
    private void Commands(string s)
    {
        LogToConsole($"List of commands:");
        foreach (var key in commands.Keys)
        {
            LogToConsole($"{key}");
        }
    }

    private void LogToConsole(string s)
    {
        log.text += $"{s}\n";

        Vector2 size = log.GetPreferredValues();
        size.x = logWidth;
        size.y = Mathf.Max(logHeight, size.y);
        log.rectTransform.sizeDelta = size;
        log.ForceMeshUpdate();

        scrollbar.value = 0;
    }

    private void Clear(string s)
    {
        log.text = "";
        scrollbar.size = 1;
        LogToConsole("");
    }

    private void Add(string s)
    {
        string[] parameters = s.Split(' ');

        float total = 0;
        foreach (string parameter in parameters)
        {
            if (int.TryParse(parameter, out int integer))
            {
                total += integer;
            }
            else
            {
                if (float.TryParse(parameter, out float number))
                {
                    total += number;
                }
                else
                {
                    LogToConsole($"Given parameter {parameter} was not a number.");
                    return;
                }
            }
        }
        LogToConsole($"{s.Replace(' ', '+')}");
        LogToConsole($"= {total}");
    }

    private void Multiply(string s)
    {
        string[] parameters = s.Split(' ');

        float total = 1;
        foreach (string parameter in parameters)
        {
            if (int.TryParse(parameter, out int integer))
            {
                total *= integer;
            }
            else
            {
                if (float.TryParse(parameter, out float number))
                {
                    total *= number;
                }
                else
                {
                    LogToConsole($"Given parameter {parameter} was not a number.");
                    return;
                }
            }
        }
        LogToConsole($"{s.Replace(' ', '*')}");
        LogToConsole($"= {total}");
    }
    
    private void Hello(string s)
    {
        LogToConsole($"Hello!");
    }

    private void LoveYou(string s)
    {
        LogToConsole($"Aww, thanks.");
    }
    #endregion
}