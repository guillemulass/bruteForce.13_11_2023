using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace contraseña._13_11_2023;

class Program
{
    
    static bool passwordFound = false;
    static void Main()
    {
        string path = "./2151220-passwords.txt";
        List<string> lines = new List<string>(File.ReadAllLines(path));
        int totalLineNum = lines.Count;

        Random random = new Random();
        int randomNumber = random.Next(1, totalLineNum);

        string password = lines[randomNumber];
        string encryptedPassword = CalculateSha256(password);

        Console.WriteLine($"\nIndice: {randomNumber}\nContraseña: {password}\nContraseña Encriptada: {encryptedPassword}");
        Console.WriteLine("\n---------------\n");

        int lineDivision = NumberLineDivision(totalLineNum, 8);

        for (int i = 0; i < 8; i++)
        {
            int startLine = LineDivisor(lineDivision, i);
            Thread thread = new Thread(() => UnEncrypter(startLine, lines, encryptedPassword, lineDivision));
            thread.Start();
        }
    }


// Encuentra la contraseña encriptada, transformando una lista de contraseñas a hash256 y comprarando cada contraseña
// con la contreña encriptada
// @param limit Donde empieza a leer el archivo de contraseñas
// @param lines Archivo de contraseñas, metido en una lista
// @param encriptedPassword Contraseña a desencriptar
// @param numberLineDivision numero de lineas que debe leer
    static void UnEncrypter(int limit, List<string> lines, string encryptedPassword, int numberLineDivision)
    {
        try
        {
            var actualLines = lines.GetRange(limit, numberLineDivision);

            bool continues = true;

            while (continues && !passwordFound)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (var i in actualLines)
                {
                    if (encryptedPassword == CalculateSha256(i))
                    {
                        Console.WriteLine("Contraseña Encriptada : " + encryptedPassword);
                        Console.WriteLine("Contraseña : " + i);
                        stopwatch.Stop();
                        Console.WriteLine("\nTiempo transcurrido : " + stopwatch.ElapsedMilliseconds + " ms");
                        continues = false;
                        passwordFound = true;
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

// Da el numero de lineas que debe leer cada particion para que todas lean lo mismo
// @param totalLineNum numero de lineas totales en el archivo a leer
// @param totalPartitionNum numero de particiones que vamos a realizar del archivo
    static int NumberLineDivision(int totalLineNum, int totalPartitionNum)
    {
        return totalLineNum / totalPartitionNum;
    }

// Da el numero en el que debe empezar a leer cada particion
// @param actualPartitionNum numero de la particion actual (si tenemos 8 particiones, contamos de 0 a 7)
    static int LineDivisor(int numberLineDivision, int actualPartitionNum)
    {
        return numberLineDivision * actualPartitionNum;
    }

// Encriptador de contraseñas
    static string CalculateSha256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convertir cadena en bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Calcular el hash
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Convertir los bytes del hash a una cadena hexadecimal
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
    
}