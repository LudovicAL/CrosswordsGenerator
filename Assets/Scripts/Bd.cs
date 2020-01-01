using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Bd {
	private List<List<MotDico>> listeDicos;

	#region Création

	public Bd(TextAsset[] fichiersDicos, int plusLongMot) {
		listeDicos = new List<List<MotDico>>();
		for (int i = 0; i <= plusLongMot; i++) {
			AjouterDico(fichiersDicos[i]);
		}
		CalculerScoresMotsDesDicos(fichiersDicos, plusLongMot);
	}

	#endregion Création

	#region Écriture

	public void MarquerMotUtilise(string contenu, bool utilise) {
		listeDicos[contenu.Length].Find(o => o.contenu == contenu).utilise = utilise;
	}

	public void MarquerMotUtilise(MotDico motDico, bool utilise) {
		listeDicos[motDico.longueur].Find(o => o == motDico).utilise = utilise;
	}

	#endregion Écriture

	#region OutilsDeRecherche

	public MotDico RechercherMotParContenu(string contenu) {
		return listeDicos[contenu.Length].Find(x => x.contenu == contenu);
	}

	public MotDico RechercherMotParContenuPourDefinitions(string contenu) {
		return listeDicos[0].Find(x => x.contenu == contenu);
	}

	public List<List<MotDico>> ListeDicos {
		get {
			return this.listeDicos;
		}
	}

	public bool ExistentMotsPossibles(string pattern) {
		foreach (MotDico motScore in listeDicos[pattern.Length]) {
			if (!motScore.utilise && Regex.IsMatch(motScore.contenu, pattern)) {
				return true;
			}
		}
		return false;
	}

	public int NbMotsPossibles(string pattern) {
		return listeDicos[pattern.Length].Where(x => Regex.IsMatch(x.contenu, pattern)).Where(e => e.utilise == false).Count<MotDico>();
	}

	public List<MotDico> ListeMotsPossiblesTriesParScore(string pattern) {
		return listeDicos[pattern.Length].Where(x => Regex.IsMatch(x.contenu, pattern)).Where(e => e.utilise == false).OrderByDescending(o => o.score).ToList<MotDico>();
	}

	public List<MotDico> ListeMotsPossibles(string pattern) {
		return listeDicos[pattern.Length].Where(x => Regex.IsMatch(x.contenu, pattern)).Where(e => e.utilise == false).ToList<MotDico>();
	}

	#endregion OutilsDeRecherche

	#region Initialisation

	private void AjouterDico(TextAsset textAsset) {
		List<string> strList = textAsset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
		List<MotDico> lmd = new List<MotDico>();
		foreach (string str in strList) {
			MotDico motDico = new MotDico();
			if (str.Contains(";")) {
				List<string> defList = str.Split(new string[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
				motDico.contenu = defList[0];
				defList.RemoveAt(0);
				motDico.definitions = defList.ToArray();
			} else {
				motDico.contenu = str;
			}
			motDico.longueur = motDico.contenu.Length;
			lmd.Add(motDico);
		}
		listeDicos.Add(lmd);
	}

	//Calcule les scores des mots  de tous les dictionnaires
	private void CalculerScoresMotsDesDicos(TextAsset[] fichiersDicos, int plusLongMot) {
		Dictionary<string, int> decompteLettres = CompterLettresDansDicos(fichiersDicos, plusLongMot);
		int nbLettresDansDico = CalculerNbLettresTotalDansDico(decompteLettres);
		Dictionary<string, float> scoresLettres = CalculerScoresDesLettres(decompteLettres, nbLettresDansDico);
		for (int i = 2; i <= plusLongMot; i++) {
			CalculerScoresMotsDUnDico(listeDicos[i], scoresLettres);
		}
	}

	//Calcule le nombre total de lettres dans tous les dictionnaires
	private Dictionary<string, int> CompterLettresDansDicos(TextAsset[] textAssets, int plusLongMot) {
		Dictionary<string, int> decompteLettres = new Dictionary<string, int>();
		for (int i = 2; i <= plusLongMot; i++) {
			Dictionary<string, int> tempDico = CompterLettresDansDico(textAssets[i]);
			foreach (KeyValuePair<string, int> entry in tempDico) {
				if (decompteLettres.ContainsKey(entry.Key)) {
					decompteLettres[entry.Key] += entry.Value;
				} else {
					decompteLettres.Add(entry.Key, entry.Value);
				}
			}
		}
		return decompteLettres;
	}

	//Calcule le nombre de lettres dans un dictionnaire
	private Dictionary<string, int> CompterLettresDansDico(TextAsset textAsset) {
		Dictionary<string, int> decompteDico = new Dictionary<string, int>();
		for (char c = 'A'; c <= 'Z'; c++) {
			decompteDico.Add(c.ToString(), CompterLettreDansDico(c, textAsset));
		}
		return decompteDico;
	}

 	//Calcule le nombre d'occurences d'une lettre dans un dictionnaire
	private int CompterLettreDansDico(char lettre, TextAsset textAsset) {
		return textAsset.text.Count(x => x == lettre);
	}

	//Calcule le total des nombres de lettre de tous les dictionnaires
	private int CalculerNbLettresTotalDansDico(Dictionary<string, int> decompteLettres) {
		int nbLettresDansDico = 0;
		foreach (KeyValuePair<string, int> entry in decompteLettres) {
			nbLettresDansDico += entry.Value;
		}
		return nbLettresDansDico;
	}

	//Calcule les scores de toutes les lettres de l'alphabet
	private Dictionary<string, float> CalculerScoresDesLettres(Dictionary<string, int> decompteLettres, int nbLettresDansDico) {
		Dictionary<string, float> scoresLettres = new Dictionary<string, float>();
		foreach (KeyValuePair<string, int> entry in decompteLettres) {
			scoresLettres.Add(entry.Key, ((float)entry.Value / (float)nbLettresDansDico));
		}
		return scoresLettres;
	}

	//Calcule les scores de tous les mots d'un dictionnaire
	private void CalculerScoresMotsDUnDico(List<MotDico> dico, Dictionary<string, float> scoresLettres) {
		foreach (MotDico motScore in dico) {
			for (int j = 0, max2 = motScore.contenu.Length; j < max2; j++) {
				motScore.score += scoresLettres[motScore.contenu.Substring(j, 1)];
			}
		}
	}

	#endregion Initialisation
}
