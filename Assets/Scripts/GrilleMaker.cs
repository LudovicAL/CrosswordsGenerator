using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GrilleMaker {

	/// <summary>
	/// Construit une grille
	/// </summary>
	/// <returns></returns>
	public string ConstruireGrille(int nbLignes, int nbColonnes, float probabilitesCasesNoire) {
		float max = nbLignes + nbColonnes;
		int nbCasesNoires = 0;
		int[,,] grid = null;
		bool estValide = false;
		while (estValide == false) {
			nbCasesNoires = 0;
			grid = new int[nbLignes, nbColonnes, 2];
			for (int y = 0; y < nbLignes; y++) {
				for (int x = 0; x < nbColonnes; x++) {
					if (x < 5 && y == 0) {
						grid[x, y, 0] = 1; //Les 4 premieres cases sont toujours blanches
					} else {
						int tailleMotHorizontal = ObtenirTailleMotHorizontal(x, y, grid);
						int tailleMotVertical = ObtenirTailleMotVertical(x, y, grid);
						int total = tailleMotHorizontal + tailleMotVertical;
						float ratio = ((float)total / max) * probabilitesCasesNoire;
						float resultat = Random.Range(0.0f, 1.0f);
						if (ratio > resultat) {
							grid[x, y, 0] = 0; //Case noire
							nbCasesNoires++;
						} else {
							grid[x, y, 0] = 1; //Case blanche
						}
					}
				}
			}
			estValide = VerifieValidite(grid, nbLignes, nbColonnes);
		}
		Debug.Log(nbCasesNoires + " cases noires");
		return TransformerGridEnString(grid, nbLignes, nbColonnes);
	}

	//Vérifie qu'une grille est valide (que chacune de ses cases peut atteindre la première case)
	private bool VerifieValidite(int[,,] grid, int nbLignes, int nbColonnes) {
		//Parcourir chaque case
		for (int x = 0; x < nbColonnes; x++) {
			for (int y = 0; y < nbLignes; y++) {
				//À chaque case, réinitialiser toutes les cases
				for (int x2 = 0; x2 < nbColonnes; x2++) {
					for (int y2 = 0; y2 < nbLignes; y2++) {
						grid[x2, y2, 1] = 0;
					}
				}
				//Vérifier que la courante peut atteindre la première case
				if (!EstAtteignable(x, y, grid, nbLignes, nbColonnes)) {
					return false;
				}
			}
		}
		return true;
	}

	//Une structure de données permettant de stocker des coordonnées 2D (x, y)
	private class Node {
		public int x;
		public int y;

		public Node(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}


	//Vérifie qu'il existe un chemin entre la case passée en paramètre et la première case de la grille
	private bool EstAtteignable(int x, int y, int[,,] grid, int nbLignes, int nbColonnes) {
		List<Node> nodeList = new List<Node>();
		nodeList.Add(new Node(x, y));
		grid[x, y, 1] = 9;
		while (nodeList.Count > 0) {
			for (int i = (nodeList.Count - 1); i >= 0; i--) {
				if (nodeList[i].x == 0 && nodeList[i].y == 0) {
					return true;
				}
				//Voisin du bas
				if (nodeList[i].y < (nbLignes - 1)) {
					if (grid[nodeList[i].x, nodeList[i].y + 1, 0] != 0 && grid[nodeList[i].x, nodeList[i].y + 1, 1] != 9) {
						nodeList.Add(new Node(nodeList[i].x, nodeList[i].y + 1));
						grid[nodeList[i].x, nodeList[i].y + 1, 1] = 9;
					}
				}
				//Voisin de droite
				if (nodeList[i].x < (nbColonnes - 1)) {
					if (grid[nodeList[i].x + 1, nodeList[i].y, 0] != 0 && grid[nodeList[i].x + 1, nodeList[i].y, 1] != 9) {
						nodeList.Add(new Node(nodeList[i].x + 1, nodeList[i].y));
						grid[nodeList[i].x + 1, nodeList[i].y, 1] = 9;
					}
				}
				//Voisin de gauche
				if (nodeList[i].x > 0) {
					if (grid[nodeList[i].x - 1, nodeList[i].y, 0] != 0 && grid[nodeList[i].x - 1, nodeList[i].y, 1] != 9) {
						nodeList.Add(new Node(nodeList[i].x - 1, nodeList[i].y));
						grid[nodeList[i].x - 1, nodeList[i].y, 1] = 9;
					}
				}
				//Voisin du haut
				if (nodeList[i].y > 0) {
					if (grid[nodeList[i].x, nodeList[i].y - 1, 0] != 0 && grid[nodeList[i].x, nodeList[i].y - 1, 1] != 9) {
						nodeList.Add(new Node(nodeList[i].x, nodeList[i].y - 1));
						grid[nodeList[i].x, nodeList[i].y - 1, 1] = 9;
					}
				}
				nodeList.RemoveAt(i);
			}
		}
		return false;
	}

	/// <summary>
	/// Retourne la taille du mot horizontal au coordonnées spécifiées
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="grid"></param>
	/// <returns></returns>
	private int ObtenirTailleMotHorizontal(int x, int y, int[,,] grid) {
		int taille = 1;
		x--;
		while (x >= 0 && grid[x, y, 0] != 0) {
			taille++;
			x--;
		}
		return taille;
	}

	/// <summary>
	/// Retourne la taille du mot vertical au coordonnées spécifiées
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="grid"></param>
	/// <returns></returns>
	private int ObtenirTailleMotVertical(int x, int y, int[,,] grid) {
		int taille = 1;
		y--;
		while (y >= 0 && grid[x, y, 0] != 0) {
			taille++;
			y--;
		}
		return taille;
	}

	/// <summary>
	/// Retourne la grille sous forme de string
	/// </summary>
	/// <param name="grid"></param>
	private string TransformerGridEnString(int[,,] grid, int nbLignes, int nbColonnes) {
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < nbLignes; y++) {
			for (int x = 0; x < nbColonnes; x++) {
				sb.Append(grid[x, y, 0]);
			}
			if (y < nbLignes - 1) {
				sb.AppendLine();
			}
		}
		return sb.ToString();
	}
}
