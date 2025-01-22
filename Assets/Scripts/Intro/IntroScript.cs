using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public bool canMove = false;
    [SerializeField] private GameObject intro;
    private void Start()
    {
        StartCoroutine(GoToGame());
    }

    private IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(5f);
        intro.SetActive(false);
        canMove = true;
    }
}
