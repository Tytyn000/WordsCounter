using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ReadInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text textDisplay;

    public Text CurrentNumberOfWords;
    public Text CurrentNumberOfCharacters;
    public Text NumberOfWords;
    public Text TextNumberOfCharacters;

    public bool AreWordsLimited;
    public bool AreCharactersLimited;

    public int NumberOfWordsLimit;
    public int NumberOfCharactersLimit;

    public InputField WordsLimitInputField;
    public InputField CharactersLimitInputField;

    public int WordsCount;
    public int CharactersCount;

    public bool IsOverWordsLimit;
    public bool IsOverCharactersLimit;

    public int ExcessWords;
    public int ExcessCharacters;

    public Text ExcessWordsText;
    public Text ExcessCharactersText;

    public GameObject ExcessWordsPanel;
    public GameObject ExcessCharactersPanel;

    public string SearchQuery;

    public GameObject SearchOnWebButton;
    public GameObject CopySelectedTextButton;

    public GameObject SettingsPanel;

    public string SearchEngine;
    public string SearchEngineURL;
    public bool IsThereASecondeSearchEngine;
    public string SecondSearchEngineURL;

    public Text SearchEngineText;


    public List<string> IgnoredStrings = new List<string>() { "<i>", "</i>", "<b>", "</b>", "<size>", "</size", "<color>", "/<color>", "<material>", "</material>", "<quad>, </quad>" };
    public List<string> IgnoredWords = new List<string>() 
    { 
        ".", 
        ";", 
        ",", 
        "?", 
        "!", 
        "(", 
        ")", 
        "[", 
        "]", 
        "*", 
        ":", 
        "<", 
        ">", 
        "{", 
        "}",
        "/",
        "`",
        "'",
        "|",
    };
    void Start()
    {
        WordsLimitInputField.interactable = false;
        CharactersLimitInputField.interactable = false;
        SearchEngine = "Google";
        SearchEngineURL = "https://www.google.com/search?q=";
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && SearchOnWebButton.activeSelf == true)
        {
            SettingsPanel.SetActive(true);
        }
        else
        {
            SettingsPanel.SetActive(false);
        }
        SearchEngineText.text = SearchEngine.ToString();
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            inputField.text = "";
            SearchQuery = "";
            CharactersCount = 0;
            WordsCount = 0;
        }
        GetTextStatistics(inputField.text);
        Count();
        if (inputField.selectionAnchorPosition <= inputField.text.Length && inputField.selectionFocusPosition <= inputField.text.Length)
        {
            SearchQuery = inputField.text.Substring(Mathf.Min(inputField.selectionAnchorPosition, inputField.selectionFocusPosition),Mathf.Abs(inputField.selectionFocusPosition - inputField.selectionAnchorPosition));
        }
        else
        {
            SearchQuery = "";
        }
        if (SearchQuery == "" || SearchQuery == null)
        {
            SearchOnWebButton.SetActive(false);
            CopySelectedTextButton.SetActive(false);
        }
        else
        {
            SearchOnWebButton.SetActive(true);
            CopySelectedTextButton.SetActive(true);
        }
    }

    public void GetTextStatistics(string InputText)
    {
        textDisplay.text = InputText;
        if (string.IsNullOrEmpty(InputText))
        {
            WordsCount = 0;
            CharactersCount = 0;
            CurrentNumberOfWords.text = "0";
            CurrentNumberOfCharacters.text = "0";
            return;
        }

        foreach (string ignoredString in IgnoredStrings)
        {
            InputText = InputText.Replace(ignoredString, "");
        }

        string[] words = InputText.Split(new char[] { ' ', '\n', '\r', '\t', '\a' }, System.StringSplitOptions.RemoveEmptyEntries);

        WordsCount = words.Length;

        CharactersCount = InputText.Replace(" ", "").Length;

        CurrentNumberOfWords.text = WordsCount.ToString();
        CurrentNumberOfCharacters.text = CharactersCount.ToString();
    }


    public void WordsLimited()
    {
        AreWordsLimited = !AreWordsLimited;
        WordsLimitInputField.interactable = AreWordsLimited;
    }

    public void CharactersLimited()
    {
        AreCharactersLimited = !AreCharactersLimited;
        CharactersLimitInputField.interactable = AreCharactersLimited;
    }

    public void WordsLimit(string limit)
    {
        int.TryParse(limit, out NumberOfWordsLimit);
    }

    public void CharactersLimit(string limit)
    {
        int.TryParse(limit, out NumberOfCharactersLimit);
    }

    public void Count()
    {
        if (AreCharactersLimited)
        {
            if (ExcessCharactersPanel.activeSelf == false)
            {
                ExcessCharactersPanel.SetActive(true);
            }
            if (CharactersCount > NumberOfCharactersLimit)
            {
                IsOverCharactersLimit = true;
            }
            else
            {
                IsOverCharactersLimit = false;
            }
            ExcessCharacters = CharactersCount - NumberOfCharactersLimit;
            ExcessCharactersText.text = ExcessCharacters.ToString();
            if (ExcessCharacters > 0)
            {
                ExcessCharactersText.text = "<color=red>" + "+" + ExcessCharacters + "</color>";
            }
            else
            {
                ExcessCharactersText.text = "<color=green>" + ExcessCharacters + "</color>";
            }
        }
        else
        {
            ExcessCharactersPanel.SetActive(false);
        }

        if (AreWordsLimited)
        {
            if (!ExcessWordsPanel.activeSelf)
            {
                ExcessWordsPanel.SetActive(true);
            }
            ExcessWords = WordsCount - NumberOfWordsLimit;
            ExcessWordsText.text = ExcessWords.ToString();
            if (ExcessWords > 0)
            {
                ExcessWordsText.text = "<color=red>" + "+" + ExcessWords + "</color>";
            }
            else
            {
                ExcessWordsText.text = "<color=green>" + ExcessWords + "</color>";
            }
        }
        else
        {
            ExcessWordsPanel.SetActive(false);
        }

        if (WordsCount <= 1)
        {
            NumberOfWords.text = "Nombre de mot";
        }
        else
        {
            NumberOfWords.text = "Nombre de mots";
        }

        if (CharactersCount <= 1)
        {
            TextNumberOfCharacters.text = "Nombre de caract�re";
        }
        else
        {
            TextNumberOfCharacters.text = "Nombre de caract�res";
        }

        CurrentNumberOfWords.text = WordsCount.ToString();
        CurrentNumberOfCharacters.text = CharactersCount.ToString();
    }

    public void SearchTextOnInternet()
    {
        if (string.IsNullOrEmpty(SearchQuery))
        {
            return;
        }
        if (IsThereASecondeSearchEngine == true)
        {
            SearchQuery = SearchQuery.Trim();
            SearchQuery = SearchQuery.Replace(" ", "");
            if (SearchQuery.All(char.IsDigit))
            {
                string SearchURL = SearchEngineURL + SearchQuery + "/";
                Application.OpenURL(SearchURL);
                string SecondSearchURL = SecondSearchEngineURL + SearchQuery;
                Application.OpenURL(SecondSearchURL);
            }
        }
        if (IsThereASecondeSearchEngine == false)
        {
            if (SearchEngine == "Enka")
            {
                SearchQuery = SearchQuery.Trim();
                SearchQuery = SearchQuery.Replace(" ", "");
                string searchURL = SearchEngineURL + SearchQuery + "/";
                Application.OpenURL(searchURL);
            }
            else if (SearchEngine == "Akasha")
            {
                SearchQuery = SearchQuery.Trim();
                SearchQuery = SearchQuery.Replace(" ", "");
                string searchURL = SearchEngineURL + SearchQuery;
                Application.OpenURL(searchURL);
            }
            else
            {
                string SearchURL = SearchEngineURL + SearchQuery;
                Application.OpenURL(SearchURL);
            }
        }
    }
    public void CopySelectedText()
    {
        GUIUtility.systemCopyBuffer = SearchQuery;
    }
    public void CopyAllText()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = inputField.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
    public void ClickOnGoogleSearchEngine()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://www.google.com/search?q=";
        SearchEngine = "Google";
    }
    public void ClickOnBingSearchEngine()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://www.bing.com/search?q=";
        SearchEngine = "Bing";
    }
    public void ClickOnYahooSearchEngine()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://search.yahoo.com/search?p=";
        SearchEngine = "Yahoo !";
    }
    public void ClickOnYandexSearchEngine()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://yandex.com/search/?text=";
        SearchEngine = "Yandex";
    }
    public void ClickOnYoutube()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://www.youtube.com/results?search_query=";
        SearchEngine = "Youtube";
    }
    public void ClickOnTikTok()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://www.tiktok.com/search?q=";
        SearchEngine = "TikTok";
    }
    public void ClickOnSpotify()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://open.spotify.com/search/";
        SearchEngine = "Spotify";
    }
    public void ClickOnWikipedia()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://en.wikipedia.org/wiki/Special:Search?search=";
        SearchEngine = "Wikip�dia";
    }
    public void ClickOnEnka()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://enka.network/u/";
        SearchEngine = "Enka";
    }
    public void ClickOnEnkaAndAkasha()
    {
        IsThereASecondeSearchEngine = true;
        SearchEngineURL = "https://enka.network/u/";
        SecondSearchEngineURL = "https://akasha.cv/profile/";
        SearchEngine = "Enka & Akasha";
    }
    public void ClickOnAkasha()
    {
        IsThereASecondeSearchEngine = false;
        SearchEngineURL = "https://akasha.cv/profile/";
        SearchEngine = "Akasha";
    }
}
