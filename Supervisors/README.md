---
platforms: dotnet
author: mperez
---
# Pre-Requisites 

> This sample is for ASP.NET Core 5.0

> To see the Console output for the Post call, Go to Visual Studio Options >> Debugging >> General, then check the box that says 'Redirect all output Window text to the Immediate Window'. At that point you can view console output in the immediate window.
> Otherwise, the Response will "echo" the request by returning the request object 

![View Sample Here](ReadmeFiles/ImmediateWindow.png)

## How to run this sample

To run this sample:

> Pre-requisites: Install .NET Core 2.1 or later (for example for Windows) by following the instructions at [.NET and C# - Get Started in 10 Minutes](https://www.microsoft.com/net/core). In addition to developing on Windows, you can develop on [Linux](https://www.microsoft.com/net/core#linuxredhat), [Mac](https://www.microsoft.com/net/core#macos), or [Docker](https://www.microsoft.com/net/core#dockercmd).

### Step 1: Register the sample with your Azure AD tenant

#### Choose the Azure AD tenant where you want to create your applications

1. Sign in to the [Azure portal](https://portal.azure.com) using either a work or school account or a personal Microsoft account.
1. If your account is present in more than one Azure AD tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory**.
   Change your portal session to the desired Azure AD tenant.

#### Register the webApp (WebApp)

1. Navigate to the Microsoft identity platform for developers [App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) page.
1. Select **New registration**.
1. In **App registrations (Preview)** page, select **New registration**.
1. When the **Register an application page** appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `WebApp`.
   - In the **Supported account types** section, select **Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com)**.
   - In the Redirect URI (optional) section, select **Web** in the combo-box.
   - For the *Redirect URI*, enter the base URL for the sample. By default, this sample uses `https://localhost:44321/`.
   - Select **Register** to create the application.
1. On the app **Overview** page, find the **Application (client) ID** value and record it for later. You'll need it to configure the Visual Studio configuration file for this project.
1. In the list of pages for the app, select **Authentication**.
   - In the **Redirect URIs**, add a redirect URL of type Web and valued  `https://localhost:44321/signin-oidc`
   - In the **Advanced settings** section set **Logout URL** to `https://localhost:44321/signout-oidc`
   - In the **Advanced settings** | **Implicit grant** section, check **ID tokens** as this sample requires the [Implicit grant flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-implicit-grant-flow) to be enabled to sign-in the user.
   - Select **Save**.

> Note that unless the Web App calls a Web API no certificate or secret is needed.

### Step 2: Download/ Clone this sample code or build the application using a template

This sample was created from the dotnet core 2.2 [dotnet new mvc](https://docs.microsoft.com/dotnet/core/tools/dotnet-new?tabs=netcore2x) template with `SingleOrg` authentication, and then tweaked to let it support tokens for the Azure AD V2 endpoint. You can clone/download this repository or create the sample from the command line:

#### Option 1: Download/ clone this sample

You can clone this sample from your shell or command line:

  ```console
  git clone https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2.git
  ```

> Given that the name of the sample is pretty long, and so are the name of the referenced NuGet packages, you might want to clone it in a folder close to the root of your hard drive, to avoid file size limitations on Windows.

  In the **appsettings.json** file:
  
  - replace the `ClientID` value with the *Application ID* from the application you registered in Application Registration portal on *Step 1*.
  - replace the `TenantId` value with `common`

#### Option 2: Create the sample from the command line

1. Run the following command to create a sample from the command line using the `SingleOrg` template:
    ```console
    dotnet new mvc --auth SingleOrg --client-id <Enter_the_Application_Id_here> --tenant-id common
    ```

    > Note: Replace *`Enter_the_Application_Id_here`* with the *Application Id* from the application Id you just registered in the Application Registration Portal.

2. Open the **Startup.cs** file and in the `ConfigureServices` method, after the line containing `.AddAzureAD` insert the following code, which enables your application to sign in users with the Azure AD v2.0 endpoint, that is both Work and School and Microsoft Personal accounts.

    ```CSharp
    services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
    {
        options.Authority = options.Authority + "/v2.0/";
        options.TokenValidationParameters.ValidateIssuer = false;
    });
    ```
    
3. Still in **Startup.cs**, add the following `using` statements to the top of the file:

   ```CSharp
   using Microsoft.AspNetCore.Authentication.OAuth.Claims;
   using Microsoft.AspNetCore.Authentication.OpenIdConnect;
   using System.Security.Claims;
   ```

4. Modify `Views\Shared\_LoginPartial.cshtml` to have the following content:

    ```CSharp
    @using System.Security.Claims

    @if (User.Identity.IsAuthenticated)
    {
        var identity = User.Identity as ClaimsIdentity; // Azure AD V2 endpoint specific
        string preferred_username = identity.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
        <ul class="nav navbar-nav navbar-right">
            <li class="navbar-text">Hello @preferred_username</li>
            <li><a asp-area="AzureAD" asp-controller="Account" asp-action="SignOut">Sign out</a></li>
        </ul>
    }
    else
    {
        <ul class="nav navbar-nav navbar-right">
            <li><a asp-area="AzureAD" asp-controller="Account" asp-action="Signin">Sign in</a></li>
        </ul>
    }
    ```

    > Note: This change is needed because certain token claims from Azure AD V1 endpoint (on which the original .NET core template is based) are different than Azure AD V2 endpoint.

### Step 3: Run the sample

1. Build the solution and run it.

2. Open your web browser and make a request to the app. Accept the IIS Express SSL certificate if needed. The app immediately attempts to authenticate you via the Azure AD v2 endpoint. Sign in with your personal account or with work or school account.

## Optional: Restrict sign-in access to your application

By default, when you use the dotnet core template with `SingleOrg` authentication option and follow the instructions in this guide to configure the application to use the Azure Active Directory v2.0 endpoint, both personal accounts - like outlook.com, live.com, and others - as well as Work or school accounts from any organizations that are integrated with Azure AD can sign in to your application. These multi-tenant apps are typically used on SaaS applications.

To restrict accounts types that can sign in to your application, use one of the options:

### Option 1: Restrict access to only Work and School accounts

Open **appsettings.json** and replace the line containing the `TenantId` value with `organizations`:

```json
"TenantId": "organizations",
```

### Option 2: Restrict access to only Microsoft personal accounts

Open **appsettings.json** and replace the line containing the `TenantId` value with `consumers`:

```json
"TenantId": "consumers",
```