using System.Net;

namespace Sales.WEB.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage) 
        { 
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public T? Response { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if(Error)
            {
                return null;
            }

            var codigoStatus = HttpResponseMessage.StatusCode;
            if (codigoStatus == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
}
            else if (codigoStatus == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoStatus == HttpStatusCode.Unauthorized)
            {
                return "Tienes que logearte para hacer esta operación";
            }
            else if (codigoStatus == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta operación";
            }
            return "Ha ocurrido un error inesperado";
        }
    }
}
