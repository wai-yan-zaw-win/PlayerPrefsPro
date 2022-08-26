# Unity PlayerPrefsPro

Unity PlayerPrefsPro uses Unity's default [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) to save complex data types and classes (E.g. Bool, Colors, Vectors, Lists, ...).

# Syntax

The syntax is actually the same as [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html).

# Some Examples

## Booleans :
```c#
bool testBoolean = true;
bool resultBoolean;

//Save
PlayerPrefsPro.Save("Test_Bool", testBoolean); //"Test_Bool" is the key to store data because PlayerPrefs need a key to store data.

//Load
resultBoolean = PlayerPrefsPro.Load("Test_Bool", false); //With default value
resultBoolean = PlayerPrefsPro.Load<bool>("Test_Bool"); //Without providing default value
```
