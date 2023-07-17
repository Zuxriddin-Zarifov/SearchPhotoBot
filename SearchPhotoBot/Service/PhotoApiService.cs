using System.Text.Json;
using SearchPhotoBot.Model;

namespace SearchPhotoBot.Service;

public class PhotoApiService
{
    private HttpClient _photoApiClient;
    private string ApiKey = "";

    public PhotoApiService()
    {
        _photoApiClient = new HttpClient();
        _photoApiClient.DefaultRequestHeaders.Add("Authorization", ApiKey);
    }

    public async Task<string> SendPhoto(string searchName)
    {
        try
        {
            var resultHttpResponseMessage =
                await _photoApiClient.GetAsync($"https://api.pexels.com/v1/search?query={searchName}&per_page=1");
            string resultResponseString = await resultHttpResponseMessage.Content.ReadAsStringAsync();
            Root? ResultRoot = JsonSerializer.Deserialize<Root>(resultResponseString);
            return ResultRoot.Photos[0].Src.Original;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return "https://images.pexels.com/photos/4220084/pexels-photo-4220084.jpeg";
    }

    public async Task<List<Photo>> SendPhotos(string searchName)
    {
        try
        {
            var resultHttpResponseMessage =
                await _photoApiClient.GetAsync($"https://api.pexels.com/v1/search?query={searchName}");
            string resultResponseString = await resultHttpResponseMessage.Content.ReadAsStringAsync();
            Root? ResultRoot = JsonSerializer.Deserialize<Root>(resultResponseString);
            return ResultRoot.Photos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}