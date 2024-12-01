using System;
using UnityEngine;

public class Play : MonoBehaviour
{
    public event EventHandler OnPlay;

    

    public void Playa() {
        OnPlay?.Invoke(this, EventArgs.Empty);
        UnityEngine.SceneManagement.SceneManager.LoadScene( 1 );
    }
}
