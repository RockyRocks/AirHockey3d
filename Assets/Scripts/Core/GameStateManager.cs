using UnityEngine;
using System.Collections;


public enum GameState
{
	eGS_Menu,
	eGS_Playing,
	eGS_Paused,
	eGS_Ended,
}

public class GameStateManager 
{
	private static GameStateManager 		m_Instance;
	private GameStateManager()
	{
#if UNITY_EDITOR
		m_CurrentGameState = GameState.eGS_Playing;
#endif
	}

	public static GameStateManager Get()
	{
		if (m_Instance == null)
			m_Instance = new GameStateManager ();

		return m_Instance;
	}


	private GameState				m_CurrentGameState;

	public bool IsGamePaused()
	{
		return (m_CurrentGameState == GameState.eGS_Paused);
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		m_CurrentGameState = GameState.eGS_Paused;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		m_CurrentGameState = GameState.eGS_Playing;
	}

	public bool IsGameStarted()
	{
		return (m_CurrentGameState == GameState.eGS_Playing);
	}
}
