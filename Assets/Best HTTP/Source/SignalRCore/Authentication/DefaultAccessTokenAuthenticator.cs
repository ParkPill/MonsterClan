#if !BESTHTTP_DISABLE_SIGNALR_CORE
using BestHTTP.Connections;
using System;

namespace BestHTTP.SignalRCore.Authentication
{
    public sealed class DefaultAccessTokenAuthenticator : IAuthenticationProvider
    {
        /// <summary>
        /// No pre-auth step required for this type of authentication
        /// </summary>
        public bool IsPreAuthRequired { get { return false; } }

#pragma warning disable 0067
        /// <summary>
        /// Not used event as IsPreAuthRequired is false
        /// </summary>
        public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

        /// <summary>
        /// Not used event as IsPreAuthRequired is false
        /// </summary>
        public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

#pragma warning restore 0067

        private HubConnection _connection;

        public DefaultAccessTokenAuthenticator(HubConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Not used as IsPreAuthRequired is false
        /// </summary>
        public void StartAuthentication()
        { }

        /// <summary>
        /// Prepares the request by adding two headers to it
        /// </summary>
        public void PrepareRequest(BestHTTP.HTTPRequest request)
        {
            if (this._connection.NegotiationResult == null)
                return;

#if !UNITY_WEBGL || UNITY_EDITOR
            // Add Authorization header to http requests, add access_token param to the uri otherwise
            if (BestHTTP.Connections.HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == BestHTTP.Connections.SupportedProtocols.HTTP)
                request.SetHeader("Authorization", "Bearer " + this._connection.NegotiationResult.AccessToken);
            else
#endif
                if (BestHTTP.Connections.HTTPProtocolFactory.GetProtocolFromUri(request.Uri) != BestHTTP.Connections.SupportedProtocols.WebSocket)
                    request.Uri = PrepareUriImpl(request.Uri);
        }

        public Uri PrepareUri(Uri uri)
        {
            if (this._connection.NegotiationResult == null)
                return uri;

            if (uri.Query.StartsWith("??"))
            {
                UriBuilder builder = new UriBuilder(uri);
                builder.Query = builder.Query.Substring(2);

                return builder.Uri;
            }

            // For WebSocket, PrepareRequest already changed the uri
            if (BestHTTP.Connections.HTTPProtocolFactory.GetProtocolFromUri(uri) == BestHTTP.Connections.SupportedProtocols.WebSocket)
                uri = PrepareUriImpl(uri);

            return uri;

        }

        private Uri PrepareUriImpl(Uri uri)
        {
            string query = string.IsNullOrEmpty(uri.Query) ? "" : uri.Query + "&";
            UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query + "access_token=" + this._connection.NegotiationResult.AccessToken);
            return uriBuilder.Uri;
        }

        public void Cancel()
        { }
    }
}
#endif
