namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class PostRequest
    {
        [JsonPropertyName("idBlog")]
        public int IdBlog { get; set; }

        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("fecha")]
        public string? Fecha { get; set; }

        [JsonPropertyName("foto")]
        public string? Foto { get; set; }

        [JsonPropertyName("texto")]
        public string? Texto { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("seoTitle")]
        public string? SeoTitle { get; set; }

        [JsonPropertyName("metaDescription")]
        public string? MetaDescription { get; set; }
    }
}
