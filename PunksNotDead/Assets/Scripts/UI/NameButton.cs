using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameButton : MonoBehaviour
{
    public TextMeshProUGUI Letter;
    private List<char> Alphabet = new List<char>(){'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 
                                                    'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 
                                                    'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

    private int LetterID = 0;

    public void TopButton()
    {
        LetterID++;
        if (LetterID >= Alphabet.Count) LetterID = 0;
        Letter.text = "" + Alphabet[LetterID];
    }
    
    public void BottomButton()
    {
        LetterID--;
        if (LetterID < 0) LetterID = Alphabet.Count - 1;
        Letter.text = "" + Alphabet[LetterID];
    }
}
