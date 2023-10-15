using System.Net;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<String> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
