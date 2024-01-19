using Insurance.Common.Interfaces;

namespace Insurance.Common.Services
{
    public class ApplicationHttpRequest : IApplicationHttpRequest
    {
        readonly Guid requestId;


        public ApplicationHttpRequest()
        {
            requestId = Guid.NewGuid(); // since the service is scoped, each request has a unique requestID
        }


        public Guid GetRequestId()
        {
            return requestId;
        }
    }
}
