using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thoughs : MonoBehaviour
{
    [System.Serializable]
    public class Thought
    {
        public string text;
        public float duration;
        public bool showOnce;
    }

    [SerializeField] private Text uiText;
    public List<Thought> thoughts;
    private HashSet<int> shownThoughts = new HashSet<int>();

    public void ShowThought(int index)
    {
        if (index < 0 || index >= thoughts.Count)
        {
            return;
        }

        Thought thought = thoughts[index];

        if (thought.showOnce && shownThoughts.Contains(index))
        {
            return;
        }

        StartCoroutine(DisplayThought(thought, index));
    }

    private IEnumerator DisplayThought(Thought thought, int index)
    {
        uiText.text = thought.text;
        uiText.gameObject.SetActive(true);
        shownThoughts.Add(index);

        yield return new WaitForSeconds(thought.duration); 

        uiText.gameObject.SetActive(false);
    }
}
