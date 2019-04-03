using System;
using System.Collections.Generic;

namespace App
{
    public class HttpRunnerSettings
    {
        public static string HttpClientName { get; } = "HttpRunner";

        /// <summary>
        /// Add request header:- Connection:close
        /// </summary>
        public bool ConnectionClose { get; set; }

        /// <summary>
        /// Add request header:- Host:{Host}
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The default implementation of IHttpClientFactory will pool the HttpMessageHandler instances
        /// created by the factory to reduce resource consumption. This setting configures the amount of
        /// time a handler can be pooled before it is scheduled for removal from the pool and disposal
        /// </summary>
        public TimeSpan PooledConnectionIdleTimeout { get; set; }

        /// <summary>
        /// A collection of strings that can be parsed by TimeSpan.Parse, that
        /// determines the number of requests to make and the delay before making
        /// request
        /// </summary>
        /// <remarks>
        /// Use a null value to indicate that no delay should be made before the
        /// request
        /// </remarks>
        public ICollection<string> RequestIntervals { get; set; }

        /// <summary>
        /// Whether or not to shutdown the console app after executing the http request(s)
        /// </summary>
        public bool ShutdownOnEnd { get; set; } = true;

        /// <summary>
        /// The Url to send a GET request to
        /// </summary>
        public string Url { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(ConnectionClose)}: {ConnectionClose}, {nameof(Host)}: {Host}, {nameof(PooledConnectionIdleTimeout)}: {PooledConnectionIdleTimeout}, {nameof(RequestIntervals)}: {string.Join(',', RequestIntervals)}, {nameof(ShutdownOnEnd)}: {ShutdownOnEnd}, {nameof(Url)}: {Url}";
        }
    }
}