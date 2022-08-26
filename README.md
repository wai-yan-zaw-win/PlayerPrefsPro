# Unity PlayerPrefsPro

Unity PlayerPrefsPro uses Unity's default [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) to save complex data types and classes (E.g. Bool, Colors, Vectors, Lists, ...).

# Syntax

The syntax is actually the same as [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html).

You may need to call namespace as follow to call PlayerPrefsPro.
```c#
using HarioGames.PlayerPrefsPro; 
```

# Some Examples

## ✷ Booleans :
```c#
bool testBoolean = true;
bool resultBoolean;

//Save
PlayerPrefsPro.Save("Test_Bool", testBoolean); //"Test_Bool" is the key to store data because PlayerPrefs need a key to store data.

//Load
resultBoolean = PlayerPrefsPro.Load("Test_Bool", false); //With default value
resultBoolean = PlayerPrefsPro.Load<bool>("Test_Bool"); //Without providing default value
```

## ✷ Colors :
```c#
Color testColor = Color.white;
Color resultColor;

//Color 32bit
Color32 testColor32 = Color.red;
Color32 resultColor32;

//Save
PlayerPrefsPro.Save("Test_Color", testColor); //"Test_Color" is the key to store data because PlayerPrefs need a key to store data.
PlayerPrefsPro.Save("Test_Color32", testColor32); //"Test_Color32" is the key to store data because PlayerPrefs need a key to store data.

//The keys to store data need to be different for each data.

//Load
resultColor = PlayerPrefsPro.Load("Test_Color", Color.black); //With default value
resultColor = PlayerPrefsPro.Load<Color>("Test_Color"); //Without providing default value
resultColor32 = PlayerPrefsPro.Load("Test_Color32", Color.black); //With default value
resultColor32 = PlayerPrefsPro.Load<Color32>("Test_Color32"); //Without providing default value
```
