using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    public static TransitionCanvas Instance { get; private set; }
    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    public void MakeTransition(GameSceneManager.Scene scene)
    {
        StartCoroutine(LoadScene(scene));
    }

    private IEnumerator LoadScene(GameSceneManager.Scene scene)
    {
        animator.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        GameSceneManager.Load(scene);
    }
}
