# Habitat version of a Sitecore Reference Storefront
Solution "Storefront-Habitat" is a reworked version of the original "Storefront" solution with using methodology "Habitat".

## Installation instructions:

1. Install Commerce Server. For more information, go to [http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/c_CommerceServerInstaller.html#concept_h3f_5fw_ht](http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/c_CommerceServerInstaller.html#concept_h3f_5fw_ht).
2. Configure Commerce Server. For more information, go to [http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/CS_Install_Walkthrough.html#concept_fxc_qqd_3t](http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/CS_Install_Walkthrough.html#concept_fxc_qqd_3t).
3. Once Commerce Server is installed, set up an IIS site for housing the Commerce Server web services.
    1. Create a folder named C:\inetpub\CSServices\.
       Note: The website folder, IIS site name, and host file entries in the following examples are samples. Feel free to use the naming conventions that work best for your projects.
    1. In IIS, add a new website called CSServices. The physical path must point to the folder that you previously created, and the Host name must be set to CSServices.
    1. In your hosts file, which is located at C:\Windows\System32\drivers\etc\, add "127.0.0.1 CSServices" to the end of the file. This will allow you to browse to the web services via [http://CSServices/](#) .
4. Disable the Customer & Order Manager HTTPS Requirement.
   By default, the Customer and Order Manager requires that the Orders web service URL to be HTTPS.
   You can disable this requirement by opening C:\Program Files (x86)\Commerce Server 11\Business User Applications\CustomerAndOrdersManager.exe.config, searching for the term AllowHTTP, and changing the associated value from "False" to "True".
5. Install a Sitecore instance for the "storefront-habitat" solution on your server machine. Recommended Sitecore version is 8.1, update 2.
   Default location of the Sitecore's "Website" directory for the "storefront-habitat" solution is "C:\Websites\storefront-habitat.local\Website". Default URL is [https://storefront-habitat.local/](#). It is possible to use another directory or URL, but in this case you'll have to make some changes manually in config files of the solution to complete the installation (please refer to section 21).
6. Enable https for your Sitecore website. Use port 443 for the https binding.
7. On the sitecore instance install Commerce Connect by using the Sitecore Commerce Connect package (Sitecore Commerce Connect 8.* rev. ******.zip) that you downloaded previously. From the Sitecore Desktop browse to Development Tools -> Installation Wizard, and then follow the wizard steps to install the package.
8. Install Commerce Server Connect by using the Sitecore Commerce Server Connect update package (Sitecore Commerce Server Connect 8.0 rev. *.*.*.*.update) that you downloaded previously. Navigate to [http://<your site>/sitecore/admin/UpdateInstallationWizard.aspx](#), and then follow the steps in the wizard.
9. Install Merchandising Manager by using the Sitecore Merchandising Manager update package (Sitecore Merchandising Manager 8.0 rev. *.*.*.*.update) that you downloaded previously. Navigate to [http://<your site>/sitecore/admin/UpdateInstallationWizard.aspx](#), and then follow the steps in the wizard.
10. Copy CommerceServer.Core.config to the Storefront-Habitat\Website\App_Config from directory "WebSite\MergeFiles".
11. Open a PowerShell window, navigate to the Sitecore Website directory, and run the command Initialize-CSSite. For example C:\Websites\storefront-habitat.local\Website.
12. Set up the Web services for the Desktop Business Tools by running the following PowerShell commands:
```powershell
New-CSWebService -Name "CSSolutionStorefrontsite" -Resource Orders -IISSite "CSServices";
New-CSWebService -Name "CSSolutionStorefrontsite" -Resource Profiles -IISSite "CSServices";
New-CSWebService -Name "CSSolutionStorefrontsite" -Resource Marketing -IISSite "CSServices";
New-CSWebService -Name "CSSolutionStorefrontsite" -Resource Catalog -IISSite "CSServices";
```
13. Grant permissions on the Commerce Server databases for the web site and web services. If the application pools of the Sitecore site and Commerce Server web services use your account, or the account of another user in the Administrator group, this step can be skipped because the sites will have all the required permissions. If all of the web services use the same account for the application pools, then run the following command to set the roles for each subsystem:
    Grant-CSManagementPermissions -Name  "CSSolutionStorefrontsite"  -Identity "domain\user";
    In the command above replace "domain\user" with the account you need, for example:
    Grant-CSManagementPermissions -Name  "CSSolutionStorefrontsite"  -Identity "NT AUTHORITY\Network Service";
14. Set the inventory subsystem to display out of stock SKUs by running the following command:
    Set-CSSiteResourceProperty -Name "CSSolutionStorefrontsite" -Resource "Inventory" -PropertyName "f_display_oos_skus" -PropertyValue $true;
15. Configure Profile System Encryption. For more information, go to [http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/GenerateProfileEncryptionKeys.html#concept_ybq_dlc_kt](http://commercesdn.sitecore.net/SCpbCS81/SCpbCSRefSfGuide/en-us/GenerateProfileEncryptionKeys.html#concept_ybq_dlc_kt)
16. Deploy file "Commerce.Storefront.ProfileDatabase.dacpac" against the profiles database with the following commands. If you use the MS SQL Server 2012, change "120" in the path to "110".
    cd /d "C:\Program Files (x86)\Microsoft SQL Server\120\DAC\bin\"
    SqlPackage.exe /Action:Publish /SourceFile:"{Path}\Commerce.Storefront.ProfileDatabase.dacpac" /TargetDatabaseName:CSSolutionStorefrontSite_profiles /TargetServerName:(local). Replace {Path} with the path to the "ProfileDatabase.dacpac" file as it is stored on your machine.
17. Go to the location of the Sitecore site on your hard drive, and then:
    Merge the contents of \Website\MergeFiles\Merge.CommerceServer.config into the web.config.
    Merge the contents of \Website\MergeFiles\Merge.Commerce.Storefront.config into the web.config.
    Merge the contents of \Website\MergeFiles\Views\Merge.web.config into the \Website\Views\Web.config.
18. Make sure option "Enable 32-Bit Applications" is set to true on the IIS app pool which hosts the "storefront-habitat" sitecore website (IIS->Application Pools->right click on an app pool -> Advanced Settings -> Enable 32-Bit Applications)
19. Open the "Storefront-Habitat" solution in Visual Studio (storefront-habitat\Habitat\Habitat.sln).
20. (Optional) If it was chosen for the Sitecore Storefront-Habitat website to use a directory other than "C:\Websites\storefront-habitat.local\Website" or a URL other than [http://storefront-habitat.local/](#), find the following files in the Visual Studio Solution Explorer and make changes to them:
    Solution 'Habitat'\Configuration\TdsGlobal.config - change the values of nodes "<SitecoreWebUrl>" and "<SitecoreDeployFolder>".
    Solution 'Habitat'\Configuration\gulp-config.js - change properties "websiteRoot" and "sitecoreLibraries".
    Solution 'Habitat'\Configuration\publishsettings.targets - change the value of the "publishUrl" node.
    Solution 'Habitat'\Project\Habitat\App_Config\Include\Project\Habitat.Website.config - change the value of property "hostName".
21. Publish manually the following projects of the solution (in the Visual Studio Solution Explorer right click on each project from the list below and select menu item "Publish"):
    Solution 'Habitat'\Foundation\Assets\Sitecore.Foundation.Assets;
    Solution 'Habitat'\Foundation\Indexing\Sitecore.Foundation.Indexing;
    Solution 'Habitat'\Project\StorefrontToHabitat\Storefront.Habitat.Website.
    (Optional) Navigate to the sitecore admin panel (i.e. [http://storefront-habitat.local/sitecore](#)). If an error related to library "Sitecore.Kernel.dll" appears, publish manually project  "Solution 'Habitat'\Feature\Account\Sitecore.Feature.Account". It will also publish the "Sitecore.Kernel.dll" library to fix the issue.
22. Open file "web.config" of the "storefront-habitat" sitecore website (i.e. "C:\Websites\storefront-habitat.local\Website\Web.config") and insert the following lines after node "configSections":
```xml
<system.codedom>
    <compilers>
        <compiler compilerOptions="/langversion:6 /nowarn:1659;1699;1701" extension=".cs" language="c#;cs;csharp" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35">
        </compiler>
        <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:14 /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+"/>
    </compilers>
</system.codedom>
```
23. Go to the sitecore admin panel of your "Storefront-Habitat" website( [http://{SitecoreWebsite}/sitecore/](#) ). Open the content editor, click on "sitecore" in the content tree, and then click on the "SITECORE COMMERCE" tab at the top of the page. Then click on item "Update Data Templates".
24. Manually sync the sitecore instance with sitecore TDS project "Solution 'Habitat'\Foundation\CommerceServer\Sitecore.Foundation.CommerceServer.Master". (push all the sitecore items from the TDS project to the sitecore db (right click on the TDS project in the Visual Studio Solution Explorer and in the appeared pop-up window select options "Add to Sitecore" and\or "Update Sitecore" against each item and click the "Update" button)).
25. In the Visual Studio solution explorer, right click on solution "Habitat" (at the top) and select the "Deploy" menu item. Wait until the solution is built and deployed to IIS.
26. Go to the "Storefront-Habitat" website's admin panel, open the shell ( [http://{SitecoreWebsite}/sitecore/shell/](#) ) and publish the site (click on button "Start" -> Publish Site -> Tick all the chackboxes and click "Publish"). Wait for the website to be published.
27. Go to the website's main page ( [http://{SitecoreWebsite}/](#) ). If the products are not shown (at the bottom of the page), go to the sitecore shell ( [http://{SitecoreWebsite}/sitecore/shell/](#) ), click button "Start", and select the "Control Panel" option. The "Control Panel" page will be opened.
    a. Select option "Rebuild Search Indexes". In pop-up "Rebuild Search Indexes" click on the "Rebuild" button. Wait until the search indexes are rebuilt.
    b. Select option "Indexing manager". In pop-up "Indexing Manager" tick all check boxes and click the "Rebuild" button. Wait until the indexes are rebuilt.
28. The "Storefront-Habititat" website should be ready to use at this point.

# LICENSE & COPYRIGHT

Copyright 2016 EPAM Systems, Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.