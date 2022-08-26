# Unity PlayerPrefsPro

Unity PlayerPrefsPro uses Unity's default [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) to save complex data types and classes (E.g. Booleans, Colors, Vectors, Lists, ...).

# Syntax

The syntax is actually the same as [PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html).

You may need to call namespace as follow to call PlayerPrefsPro.
```c#
using HarioGames.PlayerPrefsPro; 
```

# Some Examples

An example script is provided in the package and here are some examples of saving and loading datas with PlayerPrefsPro.

## ✷ Booleans :
```c#
bool testBoolean = true;
bool resultBoolean;

//Save
PlayerPrefsPro.Save("Test_Bool", testBoolean); //"Test_Bool" is the key to store data because PlayerPrefs needs a key to store data.

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
PlayerPrefsPro.Save("Test_Color", testColor); //"Test_Color" is the key to store data because PlayerPrefs needs a key to store data.
PlayerPrefsPro.Save("Test_Color32", testColor32); //"Test_Color32" is the key to store data because PlayerPrefs needs a key to store data.

//The keys to store data need to be different for each data.

//Load
resultColor = PlayerPrefsPro.Load("Test_Color", Color.black); //With default value
resultColor = PlayerPrefsPro.Load<Color>("Test_Color"); //Without providing default value
resultColor32 = PlayerPrefsPro.Load("Test_Color32", Color.black); //With default value
resultColor32 = PlayerPrefsPro.Load<Color32>("Test_Color32"); //Without providing default value
```

## ✷ Vector3 and Quaternion :
```c#
Vector3 testVector3 = Vector3.one;
Vector3 resultVector3;

Quaternion testQuaternion = Quaternion.identity;
Quaternion resultQuaternion;

//Save
PlayerPrefsPro.Save("Test_Vector3", testVector3); //"Test_Vector3" is the key to store data because PlayerPrefs needs a key to store data.
PlayerPrefsPro.Save("Test_Quaternion", testQuaternion); //"Test_Quaternion" is the key to store data because PlayerPrefs needs a key to store data.

//Load
resultVector3 = PlayerPrefsPro.Load("Test_Vector3", Vector3.zero); //With default value
resultVector3 = PlayerPrefsPro.Load<Vector3>("Test_Vector3"); //Without providing default value
resultQuaternion = PlayerPrefsPro.Load("Test_Quaternion", Quaternion.identity); //With default value
resultQuaternion = PlayerPrefsPro.Load<Quaternion>("Test_Quaternion"); //Without providing default value
```

## ✷ List :
```c#
List<int> testList = new List<int>() { 1, 2, 3};
List<int> resultList;

//Save
PlayerPrefsPro.Save("Test_List", testList); //"Test_List" is the key to store data because PlayerPrefs needs a key to store data.

//Load
resultList = PlayerPrefsPro.Load("Test_List", new List<int>()); //With default value
resultList = PlayerPrefsPro.Load<List<int>>("Test_List"); //Without providing default value
```

## ✷ Objects :
```c#
//Class
[System.Serializable]
public class MyClass
{
   public int testInt = 20;
   public string testString = "Hi";

   //Parameterless Constructor
   public MyClass()
   {

   }

   public MyClass(int testInt, string testString)
   {
       this.testInt = testInt;
       this.testString = testString;
   }
}

public MyClass testClass = new MyClass(123, "Leo");
public MyClass resultClass;

//Save
PlayerPrefsPro.Save("Test_Class", testClass); //"Test_Class" is the key to store data because PlayerPrefs need a key to store data.

//Load
resultClass = PlayerPrefsPro.Load("Test_Class", new MyClass()); //With default value
resultClass = PlayerPrefsPro.Load<MyClass>("Test_Class"); //Without providing default value
```



# Contacts : 

You can contact me via waiyanzawwinstar8@gmail.com.

## Social Medias :

[<img align="left" alt="Wai Yan Zaw Win | Facebook" width="28px" src="https://img.icons8.com/ios-glyphs/30/1778f2/facebook-new.png" />][facebook]
[<img align="left" alt="Wai Yan Zaw Win | Instagram" width="28px" src="https://img.icons8.com/material-outlined/24/aaaaaa/instagram-new--v1.png" />][instagram]
[<img align="left" alt="Wai Yan Zaw Win | LinkedIn" width="28px" src="https://img.icons8.com/fluent-systems-filled/50/0077b5/linkedin.png" />][linkedin]

<br />

[facebook]: https://www.facebook.com/WaiYanZawWin.Leo
[instagram]: https://www.instagram.com/waiyanzawwin0_0
[linkedin]: https://www.linkedin.com/in/wai-yan-zaw-win
