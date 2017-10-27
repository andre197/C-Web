namespace SomeProject.Server.HTTP.Responses
{
    using System.Text;
    using Contracts;
    using Enums;
    using Server.Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        private readonly IView view;

        protected HttpResponse(string redirectUrl)
        {
            this.StatusCode = HttpResponseStatusCode.Found;
            this.AddHeader("Location", redirectUrl);
        }

        protected HttpResponse(HttpResponseStatusCode responseCode, IView view)
        {
            this.StatusCode = responseCode;
            this.view = view;
        }

        private HttpHeaderCollection HeaderConllection { get; set; } = new HttpHeaderCollection();

        private HttpResponseStatusCode StatusCode { get; set; }

        private string StatusMessage => this.StatusCode.ToString();

        public string Response
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                int statusCode = (int)this.StatusCode;

                sb.AppendLine($"HTTP/1.1 {statusCode} {StatusMessage}");
                sb.AppendLine($"{this.HeaderConllection}");
                sb.AppendLine();

                if (statusCode < 300 || 400 < statusCode)
                {
                    sb.AppendLine(this.view.View());
                }

                return sb.ToString();
            }
        }

        private void AddHeader(string location, string redirectUrl)
        {
            HttpHeader header = new HttpHeader(location, redirectUrl);

            this.HeaderConllection.Add(header);
        }
    }
}
