using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GeneradorAliens : MonoBehaviour
{

    //Conteo de frames para el movimiento de los aliens
    private static int baseFrames = 30;
    private int maxFrames = baseFrames;
    private int frame = 0;
    private bool limiteAlcanzado = false;
    private int aliensUp;
    private int framesPorAlien = (baseFrames / FILAS * COLUMNAS) + 20;

    // Publicamos la variable para conectarla desde el editor
    public Rigidbody2D prefabAlien1;

	// Referencia para guardar una matriz de objetos
	private Rigidbody2D[,] aliens;

	// Tamaño de la invasión alienígena
	private const int FILAS = 4;
	private const int COLUMNAS = 7;

	// Enumeración para expresar el sentido del movimiento
	private enum direccion { IZQ, DER };

	// Rumbo que lleva el pack de aliens
	private direccion rumbo = direccion.DER;

	// Posición vertical de la horda (lo iremos restando de la .y de cada alien)
	private float altura = 0.5f;

	// Límites de la pantalla
	private float limiteIzq;
	private float limiteDer;

	// Velocidad a la que se desplazan los aliens (medido en u/s)
	private float velocidad = 5f;

	// Use this for initialization
	void Start ()
	{
		// Rejilla de 4x7 aliens
		generarAliens (FILAS, COLUMNAS, 1.5f, 1.0f);

		// Calculamos la anchura visible de la cámara en pantalla
		float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;

		// Calculamos el límite izquierdo y el derecho de la pantalla (añadimos una unidad a cada lado como margen)
		limiteIzq = -1.0f * distanciaHorizontal + 2;
		limiteDer = 1.0f * distanciaHorizontal - 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
        int contadorAliens = 0;

        for(int i=0; i<FILAS; i++)
        {
            for(int j=0; j < COLUMNAS; j++)
            {
                contadorAliens++;
            }
        }

		// Contador para saber si hemos terminado
		int numAliens = 0;

		// Variable para saber si al menos un alien ha llegado al borde
        if (frame == maxFrames)
        {
            frame = 0;
            if (limiteAlcanzado == false)
            {
                // Recorremos la horda alienígena
                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {

                        // Comprobamos que haya objeto, para cuando nos empiecen a disparar
                        if (aliens[i, j] != null)
                        {

                            // Un alien más
                            numAliens += 1;

                            // ¿Vamos a izquierda o derecha?
                            if (rumbo == direccion.DER)
                            {

                                // Nos movemos a la derecha (todos los aliens que queden)
                                aliens[i, j].transform.Translate(1.0f, 0.0f, 0.0f);

                                // Comprobamos si hemos tocado el borde
                                if (aliens[i, j].transform.position.x > limiteDer)
                                {
                                    limiteAlcanzado = true;
                                }
                            }
                            else
                            {

                                // Nos movemos a la derecha (todos los aliens que queden)
                                aliens[i, j].transform.Translate(-1.0f, 0.0f, 0.0f);

                                // Comprobamos si hemos tocado el borde
                                if (aliens[i, j].transform.position.x < limiteIzq)
                                {
                                    limiteAlcanzado = true;
                                }
                            }
                        }
                    }
                }
                if (numAliens == 0)
                {
                    SceneManager.LoadScene("victoria");
                }
            }
            else
            {
                for (int i = 0; i < FILAS; i++)
                {
                    for (int j = 0; j < COLUMNAS; j++)
                    {

                        // Comprobamos que haya objeto, para cuando nos empiecen a disparar
                        if (aliens[i, j] != null)
                        {
                            aliens[i, j].transform.Translate(0.0f, -1.0f, 0.0f);
                            limiteAlcanzado = false;
                        }
                    }
                }


                if (rumbo == direccion.DER)
                {
                    rumbo = direccion.IZQ;
                }
                else
                {
                    rumbo = direccion.DER;
                }
            }
        }
        //Siguiente frame
        frame++;
		
	}

	void generarAliens (int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
	{
		/* Creamos una rejilla de aliens a partir del punto de origen
		 * 
		 * Ejemplo (2,5):
		 *   A A A A A
		 *   A A O A A
		 */

		// Calculamos el punto de origen de la rejilla
		Vector2 origen = new Vector2 (transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);

		// Instanciamos el array de referencias
		aliens = new Rigidbody2D[filas, columnas];

		// Fabricamos un alien en cada posición del array
		for (int i = 0; i < filas; i++) {
			for (int j = 0; j < columnas; j++) {

				// Posición de cada alien
				Vector2 posicion = new Vector2 (origen.x + (espacioH * j), origen.y + (espacioV * i));

				// Instanciamos el objeto partiendo del prefab
				Rigidbody2D alien = (Rigidbody2D)Instantiate (prefabAlien1, posicion, transform.rotation);

				// Guardamos el alien en el array
				aliens [i, j] = alien;

				// Escala opcional, por defecto 1.0f (sin escala)
				// Nota: El prefab original ya está escalado a 0.2f
				alien.transform.localScale = new Vector2 (0.2f * escala, 0.2f * escala);
			}
		}
	}

}
