using System.Collections;
using System.Text;
using UnityEngine;

public class SimpleWritingManager : MonoBehaviour
{
    public TextMesh textField;

    private StringBuilder m_fullText;
    private const int MAX_INPUT_SIZE = 750; // this of course is not very pretty and could be defined dynamically

    private void Start()
    {
        m_fullText = new StringBuilder();
        m_fullText.Append(textField.text);
    }

    private void Update()
    {
        // Handle the two special keys first
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            textField.color = Random.ColorHSV();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            m_fullText.Remove(0, m_fullText.Length);
            UpdateTextField();
        }
        // Otherwise just copy the last characters that were input
        else if (Input.inputString != ""
            && !Input.GetKey(KeyCode.Keypad1)
            && !Input.GetKey(KeyCode.Keypad2))
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b' // Backspace
                    && m_fullText.Length > 0)
                {
                    m_fullText.Remove(m_fullText.Length - 1, 1);
                }
                else if (c != '\r') // Not a carriage return
                {
                    m_fullText.Append(c);
                }
            }

            UpdateTextField();
        }
    }

    /// <summary>
    /// This asks the text field's font about the size of each character, and
    /// puts the last n ones fitting the text field into it.
    /// </summary>
    private void UpdateTextField()
    {
        var font = textField.font;
        var size = textField.fontSize;
        var style = textField.fontStyle;
        CharacterInfo charInfo;
        int totalSize = 0;
        StringBuilder displayedText = new StringBuilder();

        // Traverse the string backwards until we hare too far
        for(int i = m_fullText.Length - 1; i >= 0; i--)
        {
            char c = m_fullText[i];
            font.GetCharacterInfo(c, out charInfo, size, style);
            totalSize += charInfo.advance;
            if (totalSize <= MAX_INPUT_SIZE)
                displayedText.Insert(0, c); // insert at the start since we go backwards
            else
                break;
        }
        
        textField.text = displayedText.ToString();
    }
}
