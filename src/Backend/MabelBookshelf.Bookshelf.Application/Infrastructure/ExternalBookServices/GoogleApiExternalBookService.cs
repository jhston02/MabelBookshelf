using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Application.Models;

namespace MabelBookshelf.Bookshelf.Application.Infrastructure.ExternalBookServices
{
    public class GoogleApiExternalBookService : IExternalBookService
    {
        private const string GOOGLE_BOOKS_BASE_URI = "https://www.googleapis.com/books/v1";
        private const string ISBN_IDENTIFIER = "ISBN_13";
        private HttpClient _client;
        public GoogleApiExternalBookService(HttpClient client)
        {
            this._client = client;
        }

        public async Task<ExternalBook> GetBook(string externalBookId)
        {
            using var responseMessage = await _client.GetAsync(GOOGLE_BOOKS_BASE_URI + $"/volumes/{externalBookId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var googleBook = JsonSerializer.Deserialize<GoogleApiBookDto>(await responseMessage.Content.ReadAsStringAsync());
                return new ExternalBook(googleBook.Id, googleBook.VolumeInfo.Title, googleBook.VolumeInfo.Authors,
                    googleBook.IndustryIdentifiers.First(x => x.Type == ISBN_IDENTIFIER).Identifier,
                    googleBook.PageCount, googleBook.Categories);
            }
            else
            {
                throw new ArgumentException("Invalid book id");
            }
        }

        private class GoogleApiBookDto
        {
            public string Id { get; set; }
            public VolumeInfo VolumeInfo { get; set; }
            public IndustryIdentifiers[] IndustryIdentifiers { get; set; }
            public string[] Categories { get; set; }
            public int PageCount { get; set; }
        }

        private class VolumeInfo
        {
            public string Title { get; set; }
            public string[] Authors { get; set; }
        }

        private class IndustryIdentifiers
        {
            public string Type { get; set; }
            public string Identifier { get; set; }
        }
    }
}