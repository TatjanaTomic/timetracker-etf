﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTrackerEtf.Client.Security;

namespace TimeTrackerEtf.Client.Services
{
    public class ApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly TokenAuthenticationStateProvider _authStateProvider;

        public ApiServices(HttpClient httpClient, TokenAuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<T> GetAsync<T>(string url, string token = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _authStateProvider.GetTokenAsync();
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.ApiResourceUrl}{url}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Parse<T>(
                responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

    }
}
