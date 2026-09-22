using System;
using System.Collections.Generic;
using System.Text;

namespace Application.options
{
    public static class RequestOptions
    {
        public static readonly HttpRequestOptionsKey<bool> RequireAuth =
            new("RequireAuth");
    }
}
