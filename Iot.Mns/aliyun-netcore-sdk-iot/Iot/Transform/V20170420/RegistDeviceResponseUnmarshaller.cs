/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using Aliyun.Acs.Core.Transform;
using Aliyun.Acs.Iot.Model.V20170420;
using System;
using System.Collections.Generic;

namespace Aliyun.Acs.Iot.Transform.V20170420
{
    public class RegistDeviceResponseUnmarshaller
    {
        public static RegistDeviceResponse Unmarshall(UnmarshallerContext context)
        {
			RegistDeviceResponse registDeviceResponse = new RegistDeviceResponse();

			registDeviceResponse.HttpResponse = context.HttpResponse;
			registDeviceResponse.RequestId = context.StringValue("RegistDevice.RequestId");
			registDeviceResponse.Success = context.BooleanValue("RegistDevice.Success");
			registDeviceResponse.ErrorMessage = context.StringValue("RegistDevice.ErrorMessage");
			registDeviceResponse.DeviceId = context.StringValue("RegistDevice.DeviceId");
			registDeviceResponse.DeviceName = context.StringValue("RegistDevice.DeviceName");
			registDeviceResponse.DeviceSecret = context.StringValue("RegistDevice.DeviceSecret");
			registDeviceResponse.DeviceStatus = context.StringValue("RegistDevice.DeviceStatus");
        
			return registDeviceResponse;
        }
    }
}