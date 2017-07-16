using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bd {
	private List<List<string>> listeDicos;

	public Bd(TextAsset[] fichiersDicos, int plusLongMot) {
		listeDicos = new List<List<string>>();
		for (int i = 0; i <= plusLongMot; i++) {
			List<string> lm = fichiersDicos[i].text.Split (new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries).ToList ();
			listeDicos.Add (lm);
		}
	}
}
