UnityJustify
============

A Justify alignment system for the Unity GUI.
*CURRENTLY THIS ONLY SUPPORTS SINGLE-LINE STRINGS*

## Installation

Simply copy TextJustify.cs to any folder on your project, and you're ready to go.

## Usage

The `TextJustify` class is used to transform plain text into collections of
objects, which represent the text's individual words, and have three properties:

 - `word`: The word's text as a .NET string
 - `width`: The individual word's width, in fractional pixels
 - `nextSpacing`: The amount of blank space that should be left after this word to correctly justify it

It's usage is fairly simple: Initialize it with the required information, then 
call `GetWordsInfo` for further processing (e.g. drawing on the interface, etc).
The `GetWordsInfo` method returns relatively raw alignment data, see above.

A common way to display these is with a foreach loop and some basic UI code:

```csharp

public GUIStyle someUnityStyle;

//constructor takes text, target width and style
TextJustify justify = new TextJustify("kup teraz", 500, someUnityStyle);

void OnGUI() {
	IEnumerable<TextJustify.WordInfo> words = justify.GetWordsInfo();
	float xPos = 0; //horizontal offset
	foreach (TextJustify.WordInfo info in words) {
		Rect rect = new Rect(xPos, 10, info.width, 30);
		GUI.Label(rect, info.word, justify.Style);
		xPos += info.width + info.nextSpacing;
	}
}

```

Any changes to the instance's properties will recalculate the alignment data
accordingly, making easy to have dynamically resizable containers or text.
