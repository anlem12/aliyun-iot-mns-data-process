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
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Transform;
using Aliyun.Acs.Core.Utils;
using Aliyun.Acs.Iot.Transform;
using Aliyun.Acs.Iot.Transform.V20170420;
using System.Collections.Generic;

namespace Aliyun.Acs.Iot.Model.V20170420
{
    public class DevicePermitModifyRequest : RpcAcsRequest<DevicePermitModifyResponse>
    {
        public DevicePermitModifyRequest()
            : base("Iot", "2017-04-20", "DevicePermitModify")
        {
        }

		private string productKey;

		private long? ruleId;

		private string deviceName;

		private string topicFullName;

		private string grantType;

		public string ProductKey
		{
			get
			{
				return productKey;
			}
			set	
			{
				productKey = value;
				DictionaryUtil.Add(QueryParameters, "ProductKey", value.ToString());
			}
		}

		public long? RuleId
		{
			get
			{
				return ruleId;
			}
			set	
			{
				ruleId = value;
				DictionaryUtil.Add(QueryParameters, "RuleId", value.ToString());
			}
		}

		public string DeviceName
		{
			get
			{
				return deviceName;
			}
			set	
			{
				deviceName = value;
				DictionaryUtil.Add(QueryParameters, "DeviceName", value);
			}
		}

		public string TopicFullName
		{
			get
			{
				return topicFullName;
			}
			set	
			{
				topicFullName = value;
				DictionaryUtil.Add(QueryParameters, "TopicFullName", value);
			}
		}

		public string GrantType
		{
			get
			{
				return grantType;
			}
			set	
			{
				grantType = value;
				DictionaryUtil.Add(QueryParameters, "GrantType", value);
			}
		}

        public override DevicePermitModifyResponse GetResponse(Core.Transform.UnmarshallerContext unmarshallerContext)
        {
            return DevicePermitModifyResponseUnmarshaller.Unmarshall(unmarshallerContext);
        }
    }
}