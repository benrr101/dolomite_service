﻿using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace DolomiteWcfService
{
    [ServiceContract]
    interface IStaticPlaylistEndpoint
    {
        #region Create Methods

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/")]
        Message CreateStaticPlaylist(Stream body);

        #endregion

        #region Retrieve Methods

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/")]
        Message GetAllStaticPlaylists();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{guid}")]
        Message GetStaticPlaylist(string guid);

        #endregion

        #region Update Methods

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{guid}")]
        Message AddTrackToStaticPlaylist(Stream body, string guid);

        #endregion

        #region Delete Methods

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{playlist}/{track}")]
        Message DeleteTrackFromStaticPlaylist(string playlist, string track);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{guid}")]
        Message DeleteStaticPlaylist(string guid);

        #endregion

        [WebInvoke(Method = "OPTIONS", UriTemplate = "/*")]
        bool PreflyRequest();
    }
}
