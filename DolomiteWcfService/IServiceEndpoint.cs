﻿using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DolomiteWcfService
{
    [ServiceContract]
    public interface IServiceEndpoint
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/tracks/")]
        void UploadTrack(Stream file);

        // TODO: REMOVE THIS TEST METHOD
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/tracks/", ResponseFormat = WebMessageFormat.Json)]
        List<string> GetTracks();
    }
}
