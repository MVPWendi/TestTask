using System.Net;
using System.Text;

namespace TestTask
{
    public class HTTP
    {

        public static async void SendHTTP(string phone, string NewStatus)
        {

            string PostData = $"Обновлен статус соискателя, с номером телефона: {phone}, на {NewStatus}";
            var data = new StringContent(PostData, Encoding.UTF8, "application/json");
            var url = "https://web-api/postmethod";

            using var client = new HttpClient();
            //var response = await client.PostAsync(url, data);
        }
    }
}
