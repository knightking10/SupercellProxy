using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace SupercellProxy
{
    class WebAPI
    {
        private static HttpListener Listener;
        private static String URI = "http://localhost:" + Config.WebAPI_PORT + "/";

        /// <summary>
        /// Starts the WebAPI
        /// </summary>
        public static void Start()
        {
            try
            {
                if (!HttpListener.IsSupported)
                {
                    Logger.Log("Unable to start the API listener, your system does not support HttpListener's.", LogType.WARNING);
                    return;
                }

                // Create new HttpListener object
                Listener = new HttpListener();

                // Add URI
                // NOTE: The Listener only accepts http://localhost:PORT/ entries!
                // Entries like http://*:PORT/ won't work -> Access denied!
                Listener.Prefixes.Add(URI);

                // Start Listener
                Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                Listener.Start();

                Logger.Log("WebAPI started: " + URI, LogType.INFO);
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    while (Listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = (HttpListenerContext)c;
                            byte[] responseBuf = Encoding.UTF8.GetBytes(GetStatisticsHTML());
                            ctx.Response.ContentLength64 = responseBuf.Length;
                            ctx.Response.OutputStream.Write(responseBuf, 0, responseBuf.Length);
                            ctx.Response.OutputStream.Close();

                        }, Listener.GetContext());
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to start the WebAPI (" + ex.GetType() + ")!");
                Logger.Log("Please check if you configured a valid API port (" + Config.WebAPI_PORT + ")");

                if (ex is HttpListenerException)
                {
                    if (((HttpListenerException)ex).ErrorCode == 5)
                    {
                        Logger.Log("You configured invalid HttpListener prefixes.", LogType.EXCEPTION);
                        Logger.Log("Use the following prefix ONLY: http://localhost:" + Config.WebAPI_PORT + "/", LogType.EXCEPTION);
                    }
                }
            }

        }

        /// <summary>
        /// Stops the WebAPI
        /// </summary>
        public static void Stop()
        {
            Listener.Close();
        }

        /// <summary>
        /// Returns the connection list in the following format:
        /// CONNECTED-IP;ANOTHERCONNECTED-IP;
        /// </summary>
        private static String GetConnectionList()
        {
            if (Proxy.ClientPool.Count == 0)
                return "None";

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Proxy.ClientPool.Count; i++)
                sb.Append(Proxy.ClientPool[i].ClientRemoteAdr + " | ");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the HTML page for the API
        /// </summary>
        public static String GetStatisticsHTML()
        {
            return SupercellProxy.Properties.Resources.Statistics.ToString()
                .Replace("%PROXYVERSION%", Helper.AssemblyVersion)
                .Replace("%HOST%", Config.Host)
                .Replace("%TOTALCONNECTIONS%", Proxy.ClientPool.Count.ToString())
                .Replace("%TOTALCONNECTIONLIST%", GetConnectionList())
                .Replace("%TIME%", Helper.ActualTime);
        }
    }
}
