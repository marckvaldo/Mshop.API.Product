using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.Common
{
    public class APIClient
    {
        private readonly HttpClient _httpCliente;

        public APIClient(HttpClient httpCliente)
        {
            _httpCliente = httpCliente;
        }

        public async Task<(HttpResponseMessage?, TOutPut?)> Post<TOutPut>(string route, object payload) where TOutPut : class // aqui estou falando que TOutPut é do tipo class sendo asim é perminido retornar null 
        {
            var response = await _httpCliente.PostAsync(
                route,
                new StringContent(JsonSerializer.Serialize(payload),Encoding.UTF8,"application/json")
                );

            var outputString = await response.Content.ReadAsStringAsync();

            if(string.IsNullOrWhiteSpace(outputString) ) {
               return (response, null);
            }

            var outPut = JsonSerializer.Deserialize<TOutPut>(
                outputString,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }
                ); 

            return (response, outPut);
        }


        public async Task<(HttpResponseMessage?, TOutPut?)> Put<TOutPut>(string route, object payload) where TOutPut: class
        {
            var response = await _httpCliente.PutAsync(
                route, 
                new StringContent(
                    JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"
                ));

            var outputString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(outputString))
            {
                return (response, null);
            }

            var outPut = JsonSerializer.Deserialize<TOutPut>(
                outputString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true}
                );

            return (response, outPut);
        }


        public async Task<(HttpResponseMessage?, TOutPut?)> Delete<TOutPut>(string route) where TOutPut: class
        {
            var response = await _httpCliente.DeleteAsync(route);

            var outputString = await response.Content.ReadAsStringAsync();

            if(string.IsNullOrWhiteSpace(outputString))
            {
                return (response, null);
            }

            var output = JsonSerializer.Deserialize<TOutPut>
            (
                outputString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true}
            );

            return (response, output);
        }
    }
}
