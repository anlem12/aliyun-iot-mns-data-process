# aliyun-iot-mns-data-process

阿里云IOT数据转到MNS处理

DEMO中演示了设备的上线、下线状态通过MNS再转到APP的topic


修改app.config文件

    <add key="topicName" value="MNS名称"/>
	<add key="endpoint" value="MNS地址"/>

    <add key="regionId"  value="cn-shanghai"/>
    <add key="accessKeyId" value="阿里云key"/>
    <add key="secretAccessKey" value="阿里云Secret"/>
	
	
	此处为通过设备名称查出设备绑定的用户，api代码后续更新，演示可临时屏蔽
	<add key="iotapidomain" value="http://192.168.0.1"/>
   


