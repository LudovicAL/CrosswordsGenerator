using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour {

	public Grille g;
	public TextAsset gridFile;
	public GameObject WhiteSpace;
	public GameObject BlackSpace;


	// Use this for initialization
	void Start () {
		g = new Grille (gridFile);
		for (int y = 0; y < g.nbLignes; y++) {
			for (int x = 0; x < g.nbLignes; x++) {
				if (g.listeLettres[x, y].valeur != null) {
					Object.Instantiate(WhiteSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				} else {
					Object.Instantiate(BlackSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
