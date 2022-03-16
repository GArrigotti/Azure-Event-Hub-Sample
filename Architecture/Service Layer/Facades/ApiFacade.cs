using Azure_Connection_Sample.Architecture.Console;
using Azure_Connection_Sample.Architecture.Domain_Layer.Records;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Azure_Connection_Sample.Architecture.Service_Layer
{
    public class ApiFacade : IApiFacade
    {
        private bool disposed;
        private readonly ILogger logger;
        private readonly HttpClient client;

        #region Constructor:

        public ApiFacade(HttpClient client, ILogger logger)
        {
            this.client = client;
            this.logger = logger.ForContext<ApiFacade>();
        }

        #endregion

        public TokenRecord GenerateToken(string url, string policy, string key)
        {
            var expiration = (long)DateTime.UtcNow.AddMinutes(30).Subtract(DateTime.UnixEpoch).TotalSeconds;
            var endpoint = HttpUtility.UrlEncode(url);

            using var hash = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = HttpUtility.UrlEncode(Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes($"{endpoint}\n{expiration}"))));

            logger.Information($"┌{new string('─', 100)}┐");
            logger.Information($"│{$"Generating Token...".Center()}│");
            logger.Information($"│{$"Transmitting to: {url}".Center()}│");
            logger.Information($"│{$"Token Expiration: {DateTime.UnixEpoch.AddSeconds(expiration)}".Center()}│");
            logger.Information($"│{$"Policy: {policy}".Center()}│");
            logger.Information($"└{new string('─', 100)}┘");

            hash.Dispose();
            return new TokenRecord($"SharedAccessSignature sr={endpoint}&sig={signature}&se={expiration}&skn={policy}", DateTime.UnixEpoch.AddSeconds(expiration));
        }

        public async void Send<TEntity>(Uri endpoint, TEntity entity, TokenRecord token)
        {
            try
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = endpoint,
                    Method = HttpMethod.Post,
                    Content = JsonContent.Create(entity)
                };

                request.Headers.Add("Authorization", $"{token.signature}");
                request.Headers.Add("Host", endpoint.Host);

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                logger.Information($"┌{new string('─', 100)}┐");
                logger.Information($"│{$"Completed...".Center()}│");
                logger.Information($"│{$"{endpoint}".Center()}│");
                logger.Information($"│{$"Status Code: {response.StatusCode} - {(int)response.StatusCode}".Center()}│");
                logger.Information($"└{new string('─', 100)}┘");
            }

            catch(Exception exception)
            {
                exception.Decorate(logger);
                throw new Exception($"Unable to send message to {endpoint}");
            }
        }

        public async void SendBatch<TCollection>(Uri endpoint, TCollection collection, TokenRecord token)
        {
            try
            {

                var request = new HttpRequestMessage()
                {
                    RequestUri = endpoint,
                    Method = HttpMethod.Post,
                    Content = JsonContent.Create(collection)
                };

                request.Headers.Add("Authorization", $"{token.signature}");
                request.Headers.Add("Host", endpoint.Host);

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                logger.Information($"┌{new string('─', 100)}┐");
                logger.Information($"│{$"Completed...".Center()}│");
                logger.Information($"│{$"{endpoint}".Center()}│");
                logger.Information($"│{$"Status Code: {response.StatusCode} - {(int)response.StatusCode}".Center()}│");
                logger.Information($"└{new string('─', 100)}┘");
            }

            catch (Exception exception)
            {
                exception.Decorate(logger);
                throw new Exception($"Unable to send message to {endpoint}");
            }
        }

        #region Dispose:

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}