using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class botonReiniciar : MonoBehaviour {

    public void nivelUno()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void vPrincipal()
    {
        SceneManager.LoadScene("bienvenida");
    }
	
}
