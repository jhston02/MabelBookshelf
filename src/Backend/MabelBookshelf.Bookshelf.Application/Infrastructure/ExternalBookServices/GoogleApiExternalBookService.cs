using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Interfaces;
using MabelBookshelf.Bookshelf.Application.Models;

namespace MabelBookshelf.Bookshelf.Application.Infrastructure.ExternalBookServices
{
    public class GoogleApiExternalBookService : IExternalBookService
    {
        private const string GoogleBooksBaseUri = "https://www.googleapis.com/books/v1";
        private const string IsbnIdentifier = "ISBN_13";
        private readonly HttpClient _client;

        public GoogleApiExternalBookService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ExternalBook> GetBookAsync(string externalBookId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using var responseMessage =
                await _client.GetAsync(GoogleBooksBaseUri + $"/volumes/{externalBookId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var googleBook =
                    JsonSerializer.Deserialize<GoogleApiBookDto>(await responseMessage.Content.ReadAsStringAsync(),
                        options);
                var externalBook = new ExternalBook(googleBook.Id, googleBook.VolumeInfo.Title,
                    googleBook.VolumeInfo.Authors.ToArray(),
                    googleBook.VolumeInfo.IndustryIdentifiers.First(x => x.Type == IsbnIdentifier).Identifier,
                    googleBook.VolumeInfo.PageCount, googleBook.VolumeInfo.Categories.ToArray());
                return externalBook;
            }

            throw new ArgumentException("Invalid book id");
        }

        private class IndustryIdentifier
        {
            public string Type { get; set; }
            public string Identifier { get; set; }
        }

        private class ReadingModes
        {
            public bool Text { get; set; }
            public bool Image { get; set; }
        }

        private class PanelizationSummary
        {
            public bool ContainsEpubBubbles { get; set; }
            public bool ContainsImageBubbles { get; set; }
        }

        public class ImageLinks
        {
            public string SmallThumbnail { get; set; }
            public string Thumbnail { get; set; }
            public string Small { get; set; }
            public string Medium { get; set; }
            public string Large { get; set; }
            public string ExtraLarge { get; set; }
        }

        private class VolumeInfo
        {
            public string Title { get; set; }
            public List<string> Authors { get; set; }
            public string Publisher { get; set; }
            public string PublishedDate { get; set; }
            public string Description { get; set; }
            public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
            public ReadingModes ReadingModes { get; set; }
            public int PageCount { get; set; }
            public int PrintedPageCount { get; set; }
            public string PrintType { get; set; }
            public List<string> Categories { get; set; }
            public int AverageRating { get; set; }
            public int RatingsCount { get; set; }
            public string MaturityRating { get; set; }
            public bool AllowAnonLogging { get; set; }
            public string ContentVersion { get; set; }
            public PanelizationSummary PanelizationSummary { get; set; }
            public ImageLinks ImageLinks { get; set; }
            public string Language { get; set; }
            public string PreviewLink { get; set; }
            public string InfoLink { get; set; }
            public string CanonicalVolumeLink { get; set; }
        }

        private class Layer
        {
            public string LayerId { get; set; }
            public string VolumeAnnotationsVersion { get; set; }
        }

        private class LayerInfo
        {
            public List<Layer> Layers { get; set; }
        }

        private class ListPrice
        {
            public double Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int AmountInMicros { get; set; }
        }

        private class RetailPrice
        {
            public double Amount { get; set; }
            public string CurrencyCode { get; set; }
            public int AmountInMicros { get; set; }
        }

        private class Offer
        {
            public int FinskyOfferType { get; set; }
            public ListPrice ListPrice { get; set; }
            public RetailPrice RetailPrice { get; set; }
            public bool Giftable { get; set; }
        }

        private class SaleInfo
        {
            public string Country { get; set; }
            public string Saleability { get; set; }
            public bool IsEbook { get; set; }
            public ListPrice ListPrice { get; set; }
            public RetailPrice RetailPrice { get; set; }
            public string BuyLink { get; set; }
            public List<Offer> Offers { get; set; }
        }

        private class Epub
        {
            public bool IsAvailable { get; set; }
            public string AcsTokenLink { get; set; }
        }

        private class Pdf
        {
            public bool IsAvailable { get; set; }
            public string AcsTokenLink { get; set; }
        }

        private class AccessInfo
        {
            public string Country { get; set; }
            public string Viewability { get; set; }
            public bool Embeddable { get; set; }
            public bool PublicDomain { get; set; }
            public string TextToSpeechPermission { get; set; }
            public Epub Epub { get; set; }
            public Pdf Pdf { get; set; }
            public string WebReaderLink { get; set; }
            public string AccessViewStatus { get; set; }
            public bool QuoteSharingAllowed { get; set; }
        }

        private class GoogleApiBookDto
        {
            public string Kind { get; set; }
            public string Id { get; set; }
            public string Etag { get; set; }
            public string SelfLink { get; set; }
            public VolumeInfo VolumeInfo { get; set; }
            public LayerInfo LayerInfo { get; set; }
            public SaleInfo SaleInfo { get; set; }
            public AccessInfo AccessInfo { get; set; }
        }
    }
}