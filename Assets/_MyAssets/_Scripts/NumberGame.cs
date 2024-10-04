using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberGame : MonoBehaviour
{
    // All UI references.
    [SerializeField] TMP_Text p1Text;
    [SerializeField] TMP_Text p2Text;
    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_Text hintText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button submitButton;
    [SerializeField] Button restartButton;
    [SerializeField] Toggle keepPlayers;
    // The game state.
    byte gameState; // 1-byte integer.
    // Other game fields.
    string[] players; // string array for player names.
    byte currentPlayer; // 0 or 1 as array indices for players array.
    int targetNumber;
    int playerGuess;
    // The Hashy Script.
    [SerializeField] HashyScript hs;

    void Start()
    {
        Initialize();
    }

    void Initialize() // Our "start game" method. Resets/assigns starting values.
    {
        gameState = 0;
        hs.SetSprite(0);
        players = new string[2]; // array created with 2 elements. players[0] will hold Player 1's name.
        hintText.gameObject.SetActive(false); // Hide entire hint GameObject.
        // or hintText.text = "";
        statusText.text = "Player 1, enter your name:";
        gameState = 1;
    }

    public void ParseSubmit() // Every time the user clicks the "Submit" button, this method is invoked.
    {
        if (gameState == 1) // Player 1 has entered their name.
        {
            if (inputField.text.Length == 0) // If inputField is empty.
            {
                statusText.text = "Player 1, a name is required. Enter your name:";
            }
            else // Player 1 entered something.
            {
                players[0] = inputField.text.ToLower().FirstCharacterToUpper();
                p1Text.text = "Player 1: " + players[0]; // or: p1Text.text += players[0];
                // Initialize next state.
                gameState = 2;
                inputField.text = ""; // Clear the text in the inputField. Length = 0.
                statusText.text = "Player 2, enter your name:";
            }
        }
        else if (gameState == 2) // Player 2 has entered their name.
        {
            if (inputField.text.Length == 0) // If inputField is empty.
            {
                statusText.text = "Player 2, a name is required. Enter your name:";
            }
            else // Player 2 entered something.
            {
                players[1] = inputField.text.ToLower().FirstCharacterToUpper();
                p2Text.text = "Player 2: " + players[1]; // or: p2Text.text += players[1];
                // Initialize next state.
                InitState3();
            }
        }
        else if (gameState == 3)
        {
            if (!int.TryParse(inputField.text, out playerGuess) || playerGuess < 1 || playerGuess > 100) // Check if guess is valid.
            {
                statusText.text = players[currentPlayer] + ", invalid guess. Enter a number between 1 and 100:";
            }
            else // Guess is valid, now we check it.
            {
                if (hintText.gameObject.activeSelf == false) // Make sure we turn on the hints, but only once.
                {
                    hintText.gameObject.SetActive(true);
                }
                if (playerGuess == targetNumber) // Game over!
                {
                    hintText.text = $"{players[currentPlayer]}'s last guess of {playerGuess} is correct!.";
                    hs.SetSprite(3);
                    statusText.text = "Game over. " + players[currentPlayer] + " wins!";
                    inputField.gameObject.SetActive(false);
                    restartButton.gameObject.SetActive(true);
                    keepPlayers.gameObject.SetActive(true);
                    submitButton.GetComponentInChildren<TMP_Text>().text = "Quit";
                    gameState = 4;
                }
                else // Game proceeds.
                {
                    if (playerGuess > targetNumber)
                    {
                        hintText.text = $"{players[currentPlayer]}'s last guess of {playerGuess} is too high.";
                        hs.SetSprite(1);
                    }
                    else
                    {
                        hintText.text = $"{players[currentPlayer]}'s last guess of {playerGuess} is too low.";
                        hs.SetSprite(2);
                    }
                    // Switch to the other player.
                    currentPlayer = (byte)(currentPlayer == 0 ? 1 : 0);
                    statusText.text = players[currentPlayer] + ", enter a number between 1 and 100:";
                }

            }
            inputField.text = ""; // Clear the guess in the input field.
        }
        else if (gameState == 4) // Game over, uses presses "Quit".
        {
            Application.Quit();
        }
    }

    public void ParseRestart()
    {
        // Common cleanup.
        inputField.gameObject.SetActive(true); // Bring back the input field.
        restartButton.gameObject.SetActive(false); // Hide the restart button.
        keepPlayers.gameObject.SetActive(false); // Hide the toggle.
        submitButton.GetComponentInChildren<TMP_Text>().text = "Submit"; // Rename back to Submit
        if (keepPlayers.isOn) // Case for restart with current players. Game state goes back to 3.
        {
            InitState3();
        }
        else // Case for restart with new players. Game state goes back to 0.
        {
            p1Text.text = "Player 1: "; // Clear previous player 1 name.
            p2Text.text = "Player 2: "; // Clear previous player 2 name.
            Initialize();
        }
    }

    private void InitState3()
    {
        gameState = 3;
        targetNumber = Random.Range(1, 101); // Generating random number.
        currentPlayer = (byte)Random.Range(0, 2); // Randomizing who goes first. 0 or 1.
        inputField.text = ""; // Clear the text in the inputField.
        hintText.gameObject.SetActive(false);
        statusText.text = players[currentPlayer] + ", enter a number between 1 and 100:";
    }
}
