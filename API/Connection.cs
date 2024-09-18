namespace API;
public class Connection(string apiUrl, string token, string[] endpoints, HttpClient? httpClient = null)
{
    public string Get(int option)
    {

        ArgumentOutOfRangeException.ThrowIfGreaterThan(option, endpoints.Length);
        string url = $"{apiUrl}/{endpoints[option - 1]}?token={token}";

        using (var client = httpClient ?? new HttpClient())
        {
            try
            {
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    return responseContent;
                }
                throw new ApplicationException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
