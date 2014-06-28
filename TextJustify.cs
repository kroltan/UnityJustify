using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class TextJustify {
	public struct WordInfo {
		public string word;
		public float width;
		public float nextSpacing;

		public WordInfo(string word, float width, float nextSpacing) {
			this.word = word;
			this.width = width;
			this.nextSpacing = nextSpacing;
		}
	}
	private static Regex boundaries = new Regex(@"\b");
	private float wordSpacing;
	private string text;
	private float width;
	private GUIStyle style;
	public string Text {
		get {
			return text;
		}
		set {
			text = value;
			Recalculate();
		}
	}
	public float Width {
		get {
			return width;
		}
		set {
			width = value;
			Recalculate();
		}
	}
	public GUIStyle Style {
		get {
			return style;
		}
		set {
			style = value;
			Recalculate();
		}
	}
	private IEnumerable<WordInfo> words;

	public TextJustify(string text, float width, GUIStyle style) {
		this.text = text;
		this.width = width;
		this.style = style;
		Recalculate();
	}

	private IEnumerable<CharacterInfo> GetCharacterInfos(string word, Font font) {
		List<CharacterInfo> cInfos = new List<CharacterInfo>();
		foreach (char chr in word) {
			CharacterInfo info;
			font.GetCharacterInfo(chr, out info);
			cInfos.Add(info);
		}
		return cInfos;
	}

	public float GetWordWidth(string word, Font font) {
		List<CharacterInfo> cInfos = new List<CharacterInfo>();
		foreach (char chr in word) {
			CharacterInfo info;
			font.GetCharacterInfo(chr, out info);
			cInfos.Add(info);
		}
		return cInfos
			.Aggregate<CharacterInfo, float>(0, 
				(acc, inf) => acc + inf.width);
	}

	private IEnumerable<string> SplitWords() {
		return boundaries.Split(text)
			.Where(word => word.Trim().Length > 0);
	}
	
	public IEnumerable<WordInfo> GetWordsInfo() {
		return this.words;
	}

	public float GetTotalWordWidth(IEnumerable<WordInfo> infos) {
		return infos.Aggregate<WordInfo, float>(0f, (a, w) => a+w.width);
	}

	public void Recalculate() {
		List<WordInfo> infos = new List<WordInfo>();
		IEnumerable<WordInfo> existingInfo = GetWordsInfo();
		if (existingInfo != null) {
			infos.AddRange(existingInfo);
		}
		wordSpacing = (width - GetTotalWordWidth(infos)) / (infos.Count() - 1);
		IEnumerable<string> wrds = SplitWords();
		this.words = wrds.Select((word, i) => new WordInfo(word, GetWordWidth(word, style.font), i == wrds.Count()-1? 0 : wordSpacing));
	}
}
