using RogueSharpTutorial.Controller;
using RogueSharpTutorial.Model;
using RogueSharpTutorial.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject deathScreen;
    
    UI_Main mainScript; 
    Player playerScript;
    Game game;

    private void Start() => mainScript = GetComponent<UI_Main>();
    // Update is called once per frame
    void Update()
    {
        if(playerScript == null || game == null)
        {
            game = mainScript.PublicGame;
            playerScript = game.Player;
        }
        if (deathScreen == null) return; 
        deathScreen.SetActive(playerScript.Health <= 0);
        Time.timeScale = deathScreen.activeSelf ? 0 : 1f; 
    }
    public void Restart() => SceneManager.LoadScene(0);
    public void Leave() => Application.Quit();
}
