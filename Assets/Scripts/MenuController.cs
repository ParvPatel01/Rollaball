using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with the name of your game scene)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // This method will be called when the player clicks the "Quit Game" button
    public void QuitGame()
    {
        // Quit the application (note: this will only work in a built game, not in the editor)
        Application.Quit();
        Debug.Log("Quit Game");  // To see the quit action in the editor
    }
}
