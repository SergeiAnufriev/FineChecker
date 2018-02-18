using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FineChecker
{
    class Program
    {
        static HttpClient HttpClient = new HttpClient();
        static string Url = "http://mvd.gov.by/Ajax.asmx/GetExt";

        public static void Main(string[] args)
        {
           string carsJson = File.ReadAllText("cars.json", Encoding.UTF8);

            List<Auto> cars = JsonConvert.DeserializeObject<List<Auto>>(carsJson);

            foreach (Auto car in cars)
            {
                CheckCar(car);
            }

            Console.ReadKey();
        }

        public static void CheckCar(Auto car)
        {
            var requestBodyJson = $"{{\"GuidControl\":2091," +
                    $"\"Param1\":\"{car.LastName}  {car.FirstName}  {car.MiddleName}\"," +
                    $"\"Param2\":\"{car.RegistrationCertificateNumber}\"," +
                    $"\"Param3\":\"{car.RegistrationCertificateSeries}\"}}";

            // создаем объект StringContent (тело запроса) 
            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            // отправляем POST http запрос
            var response = HttpClient.PostAsync(Url, content).GetAwaiter().GetResult();

            // читаем содержимое тела ответа
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Console.WriteLine(responseContent);
        }
    }
}


