// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;

namespace Microsoft.AspNet.Http.Interfaces
{
    public interface IHttpConnectionFeature
    {
        IPAddress RemoteIpAddress { get; set; }
        IPAddress LocalIpAddress { get; set; }
        int RemotePort { get; set; }
        int LocalPort { get; set; }
        bool IsLocal { get; set; }
    }
}