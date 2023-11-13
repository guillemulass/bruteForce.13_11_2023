using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace contraseña._13_11_2023
{
    internal class Program
    {
        public static void Main()
        {
            
            
            List<String> lineas = new List<string>(File.ReadAllLines("./2151220-passwords.txt"));
            
            Random random = new Random();
            
            int numeroAleatorio = random.Next(1,2151219);

            
            string password = lineas[numeroAleatorio];
            string encriptedPassword = CalcularSha256(password);
            
            Console.WriteLine("Indice : " + numeroAleatorio); 
            Console.WriteLine("---------------");
           
           Stopwatch stopwatch = new Stopwatch();
           stopwatch.Start();
           
           foreach (var i in lineas)
           {
               if (encriptedPassword == CalcularSha256(i))
               {
                   Console.WriteLine("Contraseña Encriptada : " + encriptedPassword);
                   Console.WriteLine("Contraseña : " + i);
                   stopwatch.Stop();
                   Console.WriteLine("Tiempo transcurrido: " + stopwatch.ElapsedMilliseconds + " ms");
               }
           }
           
           
           static string CalcularSha256(string input)
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
    }
}

