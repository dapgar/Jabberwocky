using UnityEngine;
using UnityEngine.Events; // 1

/* https://www.kodeco.com/2826197-scriptableobject-tutorial-getting-started/page/2 */

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent; // 2
    [SerializeField]
    private UnityEvent response; // 3

    private void OnEnable() // 4
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable() // 5
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised() // 6
    {
        response.Invoke();
    }
}