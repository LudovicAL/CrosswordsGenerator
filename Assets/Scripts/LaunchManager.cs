using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour {

	public Grille grille;
	public Bd bd;
	public TextAsset gridFile;
	public TextAsset[] fichiersDicos;
	public GameObject WhiteSpace;
	public GameObject BlackSpace;

	// Use this for initialization
	void Awake () {
		grille = new Grille (gridFile);
		for (int y = 0; y < grille.nbLignes; y++) {
			for (int x = 0; x < grille.nbLignes; x++) {
				if (grille.listeLettres[x, y].valeur != null) {
					grille.listeLettres[x, y].Go = Object.Instantiate(WhiteSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				} else {
					grille.listeLettres[x, y].Go = Object.Instantiate(BlackSpace, new Vector3((float)x, (float)-y, 0.0f), Quaternion.identity);
				}
			}
		}
		bd = new Bd (fichiersDicos, grille.PlusLongMot);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
