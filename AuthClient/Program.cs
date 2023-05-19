using System.Text;

var url = "https://localhost:7014/";
var username = "user1";
var password = "password";

var client = new HttpClient();
client.BaseAddress = new Uri(url);
client.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

var response = await client.GetAsync("/WeatherForecast");
if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);
}
else
{
    Console.WriteLine(response.StatusCode.ToString());
}

