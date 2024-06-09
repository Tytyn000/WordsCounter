using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public string textToSpeak = "Hello, world!";

    void Start()
    {
        WordsLimitInputField.interactable = false;
        CharactersLimitInputField.interactable = false;
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            inputField.text = "";
        }
        Count();
    }

    public void GetTextStatistics(string InputText)
    {
        textDisplay.text = InputText;
        if (string.IsNullOrEmpty(InputText))
        {
            CurrentNumberOfWords.text = "0";
            CurrentNumberOfCharacters.text = "0";
            return;
        }

        string[] words = InputText.Split(new char[] { ' ', '\n', '\r', '\t', '\a' }, System.StringSplitOptions.RemoveEmptyEntries);

        WordsCount = words.Length;

        if (WordsCount <= 1)
        {
            NumberOfWords.text = "Nombre de mot";
        }
        else
        {
            NumberOfWords.text = "Nombre de mots";
        }

        CharactersCount = InputText.Replace(" ", "").Length;

        if (CharactersCount <= 1)
        {
            TextNumberOfCharacters.text = "Nombre de caractère";
        }
        else
        {
            TextNumberOfCharacters.text = "Nombre de caractères";
        }

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
            ExcessCharactersText.text = "NaN";
        }

        if (AreWordsLimited)
        {
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
            ExcessWordsText.text = "NaN";
        }
    }
}
