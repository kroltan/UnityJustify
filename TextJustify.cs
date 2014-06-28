using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Kroltan.TextTools.Extensions {
	public static class GUIExtensions {
		public static void JustifiedLabel(Vector2 position, TextJustify justify) {
			IEnumerable<TextJustify.WordInfo> words = justify.GetWordsInfo();
			float xPos = 0;
			foreach (TextJustify.WordInfo word in words) {
				Rect pos = new Rect(position.x+xPos, position.y, word.width, justify.Style.fontSize);
				GUI.Label(pos, word.word, justify.Style);
				xPos += word.width + word.nextSpacing;
			}
		}
	}
}

namespace Kroltan.TextTools {
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
		private static Regex boundaries = new Regex(@"\s");
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
		
		public float GetWordWidth(string word) {
			List<CharacterInfo> cInfos = new List<CharacterInfo>();
			foreach (char chr in word) {
				CharacterInfo info;
				Debug.Log(style.font.GetCharacterInfo(chr, out info, style.fontSize, style.fontStyle));
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
			return words;
		}
		
		public float GetTotalWordWidth(IEnumerable<WordInfo> infos) {
			return infos.Aggregate<WordInfo, float>(0f, (a, w) => a+w.width);
		}
		
		public void Recalculate() {
			IEnumerable<string> wrds = SplitWords();
			words = wrds.Select<string, WordInfo>((word, i) => new WordInfo(word, GetWordWidth(word), i == wrds.Count()-1? 0 : wordSpacing));
			Debug.Log(words.Aggregate<WordInfo, string>("",(a, wi) => a+" "+wi.width));
			wordSpacing = Mathf.Abs((width - GetTotalWordWidth(words)) / (words.Count() - 1));
		}
	}
}
