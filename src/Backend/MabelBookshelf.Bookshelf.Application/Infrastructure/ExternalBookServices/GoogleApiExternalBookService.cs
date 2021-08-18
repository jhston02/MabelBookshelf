using System;
using System.Collections.Generic;
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
        private Dictionary<string, ExternalBook> externalBooksCache;
        private const string GOOGLE_BOOKS_BASE_URI = "https://www.googleapis.com/books/v1";
        private const string ISBN_IDENTIFIER = "ISBN_13";
        private HttpClient _client;

        public GoogleApiExternalBookService(HttpClient client)
        {
            this._client = client;
            externalBooksCache = new Dictionary<string, ExternalBook>();
        }

        public async Task<ExternalBook> GetBook(string externalBookId)
        {
            if (externalBooksCache.ContainsKey(externalBookId))
            {
                return externalBooksCache[externalBookId];
            }
            else
            {
                using var responseMessage =
                    await _client.GetAsync(GOOGLE_BOOKS_BASE_URI + $"/volumes/{externalBookId}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var googleBook =
                        JsonSerializer.Deserialize<GoogleApiBookDto>(await responseMessage.Content.ReadAsStringAsync());
                    var externalBook = new ExternalBook(googleBook.id, googleBook.volumeInfo.title,
                        googleBook.volumeInfo.authors.ToArray(),
                        googleBook.volumeInfo.industryIdentifiers.First(x => x.type == ISBN_IDENTIFIER).identifier,
                        googleBook.volumeInfo.pageCount, googleBook.volumeInfo.categories.ToArray());
                    externalBooksCache[externalBookId] = externalBook;
                    return externalBook;
                }
                else
                {
                    throw new ArgumentException("Invalid book id");
                }
            }
        }

        private class IndustryIdentifier
        {
            public string type { get; set; }
            public string identifier { get; set; }
        }

        private class ReadingModes
        {
            public bool text { get; set; }
            public bool image { get; set; }
        }

        private class PanelizationSummary
        {
            public bool containsEpubBubbles { get; set; }
            public bool containsImageBubbles { get; set; }
        }

        public class ImageLinks
        {
            public string smallThumbnail { get; set; }
            public string thumbnail { get; set; }
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string extraLarge { get; set; }
        }

        private class VolumeInfo
        {
            public string title { get; set; }
            public List<string> authors { get; set; }
            public string publisher { get; set; }
            public string publishedDate { get; set; }
            public string description { get; set; }
            public List<IndustryIdentifier> industryIdentifiers { get; set; }
            public ReadingModes readingModes { get; set; }
            public int pageCount { get; set; }
            public int printedPageCount { get; set; }
            public string printType { get; set; }
            public List<string> categories { get; set; }
            public int averageRating { get; set; }
            public int ratingsCount { get; set; }
            public string maturityRating { get; set; }
            public bool allowAnonLogging { get; set; }
            public string contentVersion { get; set; }
            public PanelizationSummary panelizationSummary { get; set; }
            public ImageLinks imageLinks { get; set; }
            public string language { get; set; }
            public string previewLink { get; set; }
            public string infoLink { get; set; }
            public string canonicalVolumeLink { get; set; }
        }

        private class Layer
        {
            public string layerId { get; set; }
            public string volumeAnnotationsVersion { get; set; }
        }

        private class LayerInfo
        {
            public List<Layer> layers { get; set; }
        }

        private class ListPrice
        {
            public double amount { get; set; }
            public string currencyCode { get; set; }
            public int amountInMicros { get; set; }
        }

        private class RetailPrice
        {
            public double amount { get; set; }
            public string currencyCode { get; set; }
            public int amountInMicros { get; set; }
        }

        private class Offer
        {
            public int finskyOfferType { get; set; }
            public ListPrice listPrice { get; set; }
            public RetailPrice retailPrice { get; set; }
            public bool giftable { get; set; }
        }

        private class SaleInfo
        {
            public string country { get; set; }
            public string saleability { get; set; }
            public bool isEbook { get; set; }
            public ListPrice listPrice { get; set; }
            public RetailPrice retailPrice { get; set; }
            public string buyLink { get; set; }
            public List<Offer> offers { get; set; }
        }

        private class Epub
        {
            public bool isAvailable { get; set; }
            public string acsTokenLink { get; set; }
        }

        private class Pdf
        {
            public bool isAvailable { get; set; }
            public string acsTokenLink { get; set; }
        }

        private class AccessInfo
        {
            public string country { get; set; }
            public string viewability { get; set; }
            public bool embeddable { get; set; }
            public bool publicDomain { get; set; }
            public string textToSpeechPermission { get; set; }
            public Epub epub { get; set; }
            public Pdf pdf { get; set; }
            public string webReaderLink { get; set; }
            public string accessViewStatus { get; set; }
            public bool quoteSharingAllowed { get; set; }
        }

        private class GoogleApiBookDto
        {
            public string kind { get; set; }
            public string id { get; set; }
            public string etag { get; set; }
            public string selfLink { get; set; }
            public VolumeInfo volumeInfo { get; set; }
            public LayerInfo layerInfo { get; set; }
            public SaleInfo saleInfo { get; set; }
            public AccessInfo accessInfo { get; set; }

        }
    }
}