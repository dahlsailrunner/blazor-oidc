# Introduction
This is a sample Blazor Server app that uses OpenID Connect and OAuth2,
Serilog for logging, and includes a custom web component developed using StencilJS.

## OpenID Connect / OAuth2
The authentication functionality uses the `IdentityModel.AspNetCore` package.

It references a [demo identity server](https://demo.identityserver.io) and includes the following features:

* Authentication using Code Flow with PKCE
* Uses standard OIDC middleware in ASP.NET Core (`Microsoft.AspNetCore.Authentication.OpenIdConnect`)
* Supports deep linking (go to https://localhost:44350/counter from a new empty browser and you will end up there after authenticating)  
* Includes an authenticated API call with the bearer token added by a `DelegatingHandler` to keep application code simple (see code sample below)
* Automatic `access_token` refresh via the `IdentityModel.AspNetCore` package.

### Sample API calling code
Note the lack of the bearer token handling and error handling here -- they 
are common blocks of code that are simply included in the `StandardHttpHandler`.
```csharp
public ApiCallerService(IHttpContextAccessor httpContextAccessor)
{
    _httpContext = httpContextAccessor.HttpContext;
}

public async Task<string> GetResultFromApi()
{
    using (var http = new HttpClient(new StandardHttpHandler(_httpContext)))
    {
        var url = $"https://demo.identityserver.io/api/test";
        var result = await http.GetAsync(url);
        return await result.Content.ReadAsStringAsync();
    }
}
```

### Access token refreshes
The refresh of the `access_token` happens in the following call (found in the `Infrastructure/StandardHttpHandler.cs` ):

```csharp
var token = await _httpContext.GetUserAccessTokenAsync();
```

## StencilJS web component
[StencilJS ](https://stenciljs.com/) is a framework that lets you write 
custom web omponents that run in every browser and are usable by most frameworks.

So if you have some apps that are React, some are Vue, and some Angular, and 
you have other apps that just use plain JavaScript, these components could 
be re-used in every application without duplicating / rewriting the code.

I wanted to see how these would work in a Blazor Server app, and the result
can be found on the `Pages/Stencil.razor' page.  You can see some things that **do**
work and a standard-type binding that doesn't work.

The StencilJS component I'm using is in the `wwwroot/js/custom` folder, and it is the `dist` folder 
that results from building the component in this repo: https://github.com/jlloyd0539/custom-elements

Special thanks to Jeff Lloyd (jlloyd0539) for putting this together for me. 

Thomas Claudius Huber wrote [a blog post](https://www.thomasclaudiushuber.com/2020/02/14/initializing-web-components-in-blazor-via-js-interop/) that helped resolve the interop issues 
here.  I'm not sure if there will be a more direct way to achieve the binding, 
but for now it relies on the `wwwroot/js/custom-interop.js` file.

Here is the gist containing the code in the razor file:
```csharp
@page "/stencil"
@inject IJSRuntime JSRuntime

<h3>Stencil Component</h3>
<custom-elements-table @ref="navListRef"></custom-elements-table>

@code {
    private List<NavItem> navList;
    private ElementReference navListRef;

    protected override void OnInitialized()
    {
        navList = new List<NavItem>
        {
            new NavItem {Title = "Home", Icon = "fa fa-cog", Url = "/" },
            new NavItem {Title = "Somplace Else", Icon = "fa fa-bang", Url = "/elsewhere" }
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("jsInterop.setLeftNavItems", navListRef, navList);
        }
    }

    public class NavItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }
}
```

And then the `custom-interop.js` file contains the method to actually set the `items` property on the component:

```javascript 
var jsInterop = jsInterop || {};

jsInterop.setLeftNavItems = function (leftNavElement, navItems) {
    leftNavElement.items = navItems;
};
```

## Logging
Logging is done via `Serilog` and the code that sets this up can be found in 
`Program.cs` and then the `Infrastructure/SerilogHelper.cs` file.  This file 
could be moved to a NuGet package or shared library if you wanted to share 
logging logic across projects.