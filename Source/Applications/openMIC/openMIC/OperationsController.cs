﻿//******************************************************************************************************
//  OperationsController.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/20/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace openMIC
{
    /// <summary>
    /// Represents a REST based API for openMIC operations.
    /// </summary>
    public class OperationsController : ApiController
    {
        /// <summary>
        /// Validates that openMIC operations are responding as expected.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Queues task for operation.
        /// </summary>
        /// <param name="deviceAcronym">Acronym of device to queue.</param>
        [HttpGet]
        public HttpResponseMessage QueueTasks(string deviceAcronym)
        {
            Program.Host.SendRequest(Guid.Empty, User, $"Invoke {deviceAcronym} QueueTasks");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}