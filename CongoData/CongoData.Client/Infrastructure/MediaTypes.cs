using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace CongoData.Client.Infrastructure {
    /// <summary>
    /// Helper class for get MediaType strings.
    /// </summary>
    public static class MediaTypes {
        private static MediaTypeHeaderValue _json;

        /// <summary>
        /// MediaType for JSON data.
        /// </summary>
        public static MediaTypeHeaderValue Json {
            get {
                if (_json == null) {
                    _json = new MediaTypeHeaderValue("application/json");
                }

                return _json;
            }
        }
    }
}