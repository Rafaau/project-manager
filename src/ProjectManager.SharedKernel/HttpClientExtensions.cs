using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManager.SharedKernel;
public static class HttpClientExtensions
{
  public static async Task<Response<T>> GetResponse<T>(this HttpClient httpClient, string path)
  {
    var response = await httpClient.GetAsync(path);
    try
    {
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<Response<T>>();
    }
    catch (Exception e)
    {
      return ($"StatusCode: {response.StatusCode}\nPath: {path}\n\nBase address: {httpClient.BaseAddress}\n\nRequest: {response.RequestMessage.RequestUri}\nContent:\n {await response.Content.ReadAsStringAsync()}").ErrorResponse<T>();
    }
  }

  public static async Task<Response<R>> Post<T,R>(this HttpClient httpClient, string path, T input)
  {
    var response = await httpClient.PostAsJsonAsync(path, input);
    try
    {
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<Response<R>>();
    }
    catch (Exception e)
    {
      return ($"StatusCode: {response.StatusCode}\nRequest: {response.RequestMessage.RequestUri}\nContent:\n{await response.Content.ReadAsStringAsync()}").ErrorResponse<R>();
    }
  }

  public static async Task<Response<R>> Put<T,R>(this HttpClient httpClient, string path, T input)
  {
    var response = await httpClient.PutAsJsonAsync(path, input);
    try
    {
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<Response<R>>();
    }
    catch (Exception e)
    {
      return ($"StatusCode: {response.StatusCode}\nRequest: {response.RequestMessage.RequestUri}\nContent:\n{await response.Content.ReadAsStringAsync()}").ErrorResponse<R>();
    }
  }

  public static async Task<Response<R>> Patch<R>(this HttpClient httpClient, string path)
  {
    var req = new HttpRequestMessage(HttpMethod.Patch, path);
    var response = await httpClient.SendAsync(req);
    try
    {
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<Response<R>>();
    }
    catch (Exception e)
    {
      return ($"StatusCode: {response.StatusCode}\nRequest: {response.RequestMessage.RequestUri}\nContent:\n{await response.Content.ReadAsStringAsync()}").ErrorResponse<R>();
    }
  }

  public static async Task<Response<R>> Delete<R>(this HttpClient httpClient, string path)
  {
    var response = await httpClient.DeleteAsync(path);
    try
    {
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<Response<R>>();
    }
    catch (Exception e)
    {
      return ($"StatusCode: {response.StatusCode}\nRequest: {response.RequestMessage.RequestUri}\nContent:\n{await response.Content.ReadAsStringAsync()}").ErrorResponse<R>();
    }
  }
}
