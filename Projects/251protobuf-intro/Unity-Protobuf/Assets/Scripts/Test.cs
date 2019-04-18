using BigTalkUnity.AddressBook;
using UnityEngine;
using static BigTalkUnity.AddressBook.Person.Types;
using Google.Protobuf;
using System.IO;

public class Test : MonoBehaviour
{
    void Start()
    {
        Person john = new Person
        {
            Id = 1234,
            Name = "John Doe",
            Email = "jdoe@example.com",
            Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = PhoneType.Home } }
        };

        // 写入stream
        using (var output = File.Create("john.dat"))
        {
            john.WriteTo(output);
        }

        // 转为json字符串
        var jsonStr = john.ToString();

        // 转为bytestring
        var byteStr = john.ToByteString();

        // 转为byte数组
        var byteArray = john.ToByteArray();

        // 从stream中解析
        using (var input = File.OpenRead("john.dat"))
        {
            john = Person.Parser.ParseFrom(input);
        }

        // 从字节串中解析
        john = Person.Parser.ParseFrom(byteStr);

        // 从字节数组中解析
        john = Person.Parser.ParseFrom(byteArray);

        // 从json字符串解析
        john = Person.Parser.ParseJson(jsonStr);

    }
}
