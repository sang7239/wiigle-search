<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Search_Infrastructure" generation="1" functional="0" release="0" Id="97ae8bb5-033b-4532-899c-aaa46c1ce34f" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Search_InfrastructureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Front-End:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/LB:Front-End:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Back-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/MapBack-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Back-EndInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/MapBack-EndInstances" />
          </maps>
        </aCS>
        <aCS name="Front-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/MapFront-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Front-EndInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/MapFront-EndInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Front-End:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-End/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapBack-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Back-End/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapBack-EndInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Back-EndInstances" />
          </setting>
        </map>
        <map name="MapFront-End:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-End/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapFront-EndInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-EndInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Back-End" generation="1" functional="0" release="0" software="C:\Users\dejav\source\repos\Search-Infrastructure\Search-Infrastructure\csx\Debug\roles\Back-End" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Back-End&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Back-End&quot; /&gt;&lt;r name=&quot;Front-End&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Back-EndInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Back-EndUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Back-EndFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="Front-End" generation="1" functional="0" release="0" software="C:\Users\dejav\source\repos\Search-Infrastructure\Search-Infrastructure\csx\Debug\roles\Front-End" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Front-End&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Back-End&quot; /&gt;&lt;r name=&quot;Front-End&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-EndInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-EndUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-EndFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="Front-EndUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="Back-EndUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="Back-EndFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="Front-EndFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Back-EndInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="Front-EndInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="ee582fe3-3ba9-422f-9602-7313bab7febb" ref="Microsoft.RedDog.Contract\ServiceContract\Search_InfrastructureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="b07f87be-989e-4ba1-aaa4-b1e69f790ea7" ref="Microsoft.RedDog.Contract\Interface\Front-End:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Search_Infrastructure/Search_InfrastructureGroup/Front-End:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>