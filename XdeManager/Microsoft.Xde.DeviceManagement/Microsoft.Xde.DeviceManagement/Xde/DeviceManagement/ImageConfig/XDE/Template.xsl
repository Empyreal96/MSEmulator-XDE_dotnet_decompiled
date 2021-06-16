<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
        <ADDONCONTAINER>
            <ADDON>
                <PLATFORMCONTAINER>
                    <PLATFORM _InstallChildrenOnly="true" ID="7E6B29E6-AAE1-41dc-9849-049507CBA2B0">
                        <DEVICECONTAINER>
                            <xsl:element name="DEVICE">
                                <xsl:attribute name="Name"></xsl:attribute>
                                <xsl:attribute name="ID"></xsl:attribute>
                                <xsl:attribute name="Protected">true</xsl:attribute>
                                <PROPERTYCONTAINER>
                                    <PROPERTY ID="OS_Version">6.4</PROPERTY>
                                    <PROPERTY ID="DeviceFamily">WCOS</PROPERTY>
                                    <PROPERTY ID="UapVersion">10.0.18257.0</PROPERTY>
                                    <PROPERTY ID="OS">default</PROPERTY>
                                    <PROPERTY ID="Emulator" Protected="true">true</PROPERTY>
                                    <PROPERTY ID="CpuName">x64</PROPERTY>
                                    <PROPERTY ID="LocalCcClientFile" Protected="true" _UseCcRelativePath="true">target\wce400\%cpu%\ConManClient3.exe</PROPERTY>
                                    <PROPERTY ID="LocalCcShutdownFile" Protected="true" _UseCcRelativePath="true">target\wce400\%cpu%\ClientShutdown3.exe</PROPERTY>

                                    <PROPERTY ID="RemoteCcClientFile" Protected="true">%FOLDERID_SharedData%\PhoneTools\%CcVersion%\CoreCon\bin\ConManClient3.exe</PROPERTY>
                                    <PROPERTY ID="RemoteCcShutdownFile" Protected="true">%FOLDERID_SharedData%\PhoneTools\%CcVersion%\CoreCon\bin\ClientShutdown3.exe</PROPERTY>
                                    <PROPERTY ID="RemoteCcTransportLoaderFile" Protected="true">%FOLDERID_SharedData%\PhoneTools\%CcVersion%\CoreCon\lib\eDbgTL3.dll</PROPERTY>
                                    <PROPERTY ID="LocalCcCMAcceptFile" Protected="true" _UseCcRelativePath="true">target\wce400\%cpu%\CMAccept3.exe</PROPERTY>

                                    <PROPERTY ID="RemoteCcCMAcceptFile" Protected="true">%FOLDERID_SharedData%\PhoneTools\%CcVersion%\CoreCon\bin\CMAccept3.exe</PROPERTY>

                                    <PROPERTY ID="B333580E-3924-492e-98E5-DF57E787591B" Protected="false">29F44359-B789-41FB-B585-27C34674783B</PROPERTY>
                                    <PROPERTY ID="D7C86969-EB5F-41e2-96CC-290683622203" Protected="true">F34E0961-C184-4443-B13E-09FE59ADCE6A</PROPERTY>
                                    <PROPERTY ID="TargetDevice Service" Protected="true">2A388862-6448-4DB3-8D43-2C5BC2AE41C7</PROPERTY>
                                    <!-- Transport service property overrides -->
                                    <PROPERTY ID="B333580E-3924-492e-98E5-DF57E787591B_ALL">
                                        <PROPERTYCONTAINER>
                                            <!-- TCP Transport -->
                                            <xsl:element name="PROPERTY">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_SERVICECATEGORIES_8_0/TRANSPORT_TRANSPORTNAME_TCPCONNECT"/>
                                                </xsl:attribute>
                                                <xsl:attribute name="ID">29F44359-B789-41FB-B585-27C34674783B</xsl:attribute>
                                                <xsl:attribute name="Protected">false</xsl:attribute>
                                                <PROPERTYCONTAINER>
                                                    <PROPERTY ID="default" Protected="false">no</PROPERTY>
                                                    <PROPERTY ID="type" Protected="false">tcp_connect</PROPERTY>
                                                    <PROPERTY ID="LocalTransportFile" Protected="true" _UseCcRelativePath="true">target\wce400\%cpu%\tcpconnectiona3.dll</PROPERTY>
                                                    <PROPERTY ID="RemoteTransportFile" Protected="true">%FOLDERID_SharedData%\PhoneTools\%CcVersion%\CoreCon\lib\tcpconnectiona3.dll</PROPERTY>
                                                    <PROPERTY ID="RemotePort" Protected="false">6791</PROPERTY>
                                                    <!-- Using XDE COM api's this gets filled -->
                                                    <PROPERTY ID="ConnectToPort" Protected="false">Invalid Value</PROPERTY>
                                                    <PROPERTY ID="LocalBindingIp" Protected="false">Invalid Value</PROPERTY>
                                                    <PROPERTY ID="ConnectToIp" Protected="false">Invalid Value</PROPERTY>
                                                    <PROPERTY ID="useCustomPort" Protected="false">no</PROPERTY>
                                                    <PROPERTY ID="authenticate" Protected="false">false</PROPERTY>
                                                    <PROPERTY ID="disableauthentication" Protected="false">yes</PROPERTY>
                                                </PROPERTYCONTAINER>
                                            </xsl:element>
                                        </PROPERTYCONTAINER>
                                    </PROPERTY>
                                    <!-- Bootstrap service property overrides -->
                                    <PROPERTY ID="D7C86969-EB5F-41e2-96CC-290683622203_ALL">
                                        <PROPERTYCONTAINER>
                                            <!-- Device Emulation Bootstrap -->
                                            <xsl:element name="PROPERTY">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_VISUALSTUDIO_SERVICECATEGORIES_8_0/STARTUP_STARTUPNAME_DEVICEEMULATION"/>
                                                </xsl:attribute>
                                                <xsl:attribute name="ID">F34E0961-C184-4443-B13E-09FE59ADCE6A</xsl:attribute>
                                                <xsl:attribute name="Protected">false</xsl:attribute>
                                                <PROPERTYCONTAINER>
                                                    <PROPERTY ID="default" Protected="false">no</PROPERTY>
                                                    <PROPERTY ID="type" Protected="false">Ssh</PROPERTY>
                                                    <PROPERTY ID="RemotePort" Protected="false">0</PROPERTY>
                                                    <PROPERTY ID="XdeVersion" Protected="false">10.0</PROPERTY>
                                                    <PROPERTY ID="XdeInstallLocation" Protected="false">%CSIDL_LOCAL_APPDATA%\Microsoft\WindowsApps</PROPERTY>
                                                    <PROPERTY ID="XdeExe" Protected="false">XdeConfig.exe</PROPERTY>
                                                    <PROPERTY ID="diffdisklocation" Protected="false">%CSIDL_LOCAL_APPDATA%\Microsoft\XDE</PROPERTY>
                                                    <PROPERTY ID="vhd" Protected="false">foo.vhdx</PROPERTY>
                                                    <PROPERTY ID="creatediffdisk" Protected="false">dd.foo.vhdx</PROPERTY>
                                                    <PROPERTY ID="snapshot" Protected="false"></PROPERTY>
                                                    <PROPERTY ID="name" Protected="false"></PROPERTY>
                                                    <PROPERTY ID="video" Protected="false">full</PROPERTY>
                                                    <PROPERTY ID="memsize" Protected="false">2048</PROPERTY>
                                                    <!--  language is in hexadecimal -->
                                                    <PROPERTY ID="language" Protected="false">409</PROPERTY>
                                                    <PROPERTY ID="sku" Protected="false">foo</PROPERTY>
                                                </PROPERTYCONTAINER>
                                            </xsl:element>
                                        </PROPERTYCONTAINER>
                                    </PROPERTY>
                                    <PROPERTY ID="OutputLocation">%CSIDL_PROGRAM_FILES%</PROPERTY>
                                    <PROPERTY ID="OutputLocation_ALL">
                                        <PROPERTYCONTAINER>
                                            <PROPERTY ID="\">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_ROOT"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_PERSONAL%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_PERSONAL"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_PROGRAMS%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_PROGRAMS"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_PROGRAM_FILES%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_PROGRAM_FILES"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_APPDATA%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_APPDATA"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_COMMON_APPDATA%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_COMMON_APPDATA"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_WINDOWS%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_WINDOWS"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_FONTS%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_FONTS"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_STARTMENU%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_STARTMENU"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                            <PROPERTY ID="%CSIDL_STARTUP%">
                                                <xsl:attribute name="Name">
                                                    <xsl:value-of select="LANGUAGE/MICROSOFT_WINDOWSCE_2_0/CSIDL_STARTUP"/>
                                                </xsl:attribute>
                                            </PROPERTY>
                                        </PROPERTYCONTAINER>
                                    </PROPERTY>
                                </PROPERTYCONTAINER>
                            </xsl:element>
                        </DEVICECONTAINER>
                        <FORMFACTORCONTAINER/>
                    </PLATFORM>
                </PLATFORMCONTAINER>
            </ADDON>
        </ADDONCONTAINER>
    </xsl:template>
</xsl:stylesheet>
