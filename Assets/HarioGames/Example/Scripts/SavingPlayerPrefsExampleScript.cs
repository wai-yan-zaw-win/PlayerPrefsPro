using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarioGames.PlayerPrefsPro;

/*TO DO :
 * Need to fix enum saving.
 */

public class SavingPlayerPrefsExampleScript : MonoBehaviour
{
    [Header("Datas to Save")]
    public int number_int = 10;
    public long number_long = 1234567891069420;
    public double number_double = 420.6969696969f;

    public uint number_int_unsigned = 10;
    public ulong number_long_unsigned = 1234567891069420;
    public ushort number_short_unsigned = 8;

    public Vector3 vector3 = Vector3.one;
    public bool testBool = true;
    public Quaternion quaternion = Quaternion.identity;
    public Color testColor = Color.white;
    public TestEnum testEnum = TestEnum.enum3;

    public MyClass testClass = new MyClass(123, "Leo");
    public List<MyClass> testClassList = new List<MyClass>() { new MyClass(50, "Mio"), new MyClass(1000, "Dio") };

    [Header("Loaded Datas")]
    public int result_number_int;
    public long result_number_long;
    public double result_number_double;

    public uint result_number_int_unsigned;
    public ulong result_number_long_unsigned;
    public ushort result_number_short_unsigned;

    public Vector3 result_vector3;
    public bool result_bool;
    public Quaternion result_quaternion;
    public Color result_testColor;
    public TestEnum result_testEnum;

    public MyClass result_testClass;
    public List<MyClass> result_testClassList = new List<MyClass>();

    public enum TestEnum
    {
        enum1,
        enum2,
        enum3
    }

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


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SavePlayerPrefs();
        }

        if (Input.GetMouseButtonUp(1))
        {
            LoadPlayerPrefs();
        }
    }

    public void SavePlayerPrefs()
    {
        //Saving Common Data Types
        PlayerPrefsPro.Save("number_int", number_int);
        PlayerPrefsPro.Save("number_long", number_long);
        PlayerPrefsPro.Save("number_double", number_double);

        //Saving Unsigned Data Types
        PlayerPrefsPro.Save("number_int_unsigned", number_int_unsigned);
        PlayerPrefsPro.Save("number_long_unsigned", number_long_unsigned);
        PlayerPrefsPro.Save("number_short_unsigned", number_short_unsigned);

        //Saving Objects and Classes
        PlayerPrefsPro.Save("Test_Vector3", vector3);
        PlayerPrefsPro.Save("Test_Bool", testBool);
        PlayerPrefsPro.Save("Test_Quaternion", quaternion);
        PlayerPrefsPro.Save("Test_Color", testColor);
        PlayerPrefsPro.Save("Test_Enum", testEnum);
        PlayerPrefsPro.Save("Test_Class", testClass);
        PlayerPrefsPro.Save("Test_List_Class", testClassList);

        //Saving Encrypted Datas
        PlayerPrefsPro.EncrytedSave("Encrypted_Test_Class", testClass);
    }

    public void LoadPlayerPrefs()
    {
        //Loading Common Data Types
        result_number_int = PlayerPrefsPro.Load<int>("number_int");
        result_number_long = PlayerPrefsPro.Load<long>("number_long");
        result_number_double = PlayerPrefsPro.Load<double>("number_double");

        //Loading Unsigned Data Types
        result_number_int_unsigned = PlayerPrefsPro.Load<uint>("number_int_unsigned");
        result_number_long_unsigned = PlayerPrefsPro.Load<ulong>("number_long_unsigned");
        result_number_short_unsigned = PlayerPrefsPro.Load<ushort>("number_short_unsigned");

        //Saving Objects and Classes
        result_vector3 = PlayerPrefsPro.Load<Vector3>("Test_Vector3");
        result_quaternion = PlayerPrefsPro.Load<Quaternion>("Test_Quaternion");
        result_testColor = PlayerPrefsPro.Load<Color>("Test_Color");

        result_bool = PlayerPrefsPro.Load<bool>("Test_Bool");
        result_testEnum = PlayerPrefsPro.Load<TestEnum>("Test_Enum");

        result_testClass = PlayerPrefsPro.Load<MyClass>("Test_Class");
        result_testClassList = PlayerPrefsPro.Load<List<MyClass>>("Test_List_Class");

        //Loading Encrypted Datas
        result_testClass = PlayerPrefsPro.EncryptedLoad<MyClass>("Encrypted_Test_Class");
    }
}
