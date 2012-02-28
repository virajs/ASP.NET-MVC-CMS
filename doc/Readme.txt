Requirements
1. ASP.NET MVC 2 RC 
2. Visual Studio 2008
3. CommonLibrary.NET ( included )
4. Sql Server 2005 ( optional )


Quick Setup
1. Download source zip file or source code from svn
2. Open solution file studio 2008: <ROOT>\src\apps\CommonLibraryNet.Web.sln
3. Run the app
 
NOTE:
This uses In-Memory Repositories for the various models / entities so there is no dependency on a database.
See steps below for database configuration.
This In-Memory repository sample is used for quick setup, testing, and easily playing around w/ the app without formal install steps


Setup w/ Sql Database
1. Do steps 1 - 2 in "quick setup" above ( download source & open solution file ).
2. Run sql schema install file at <ROOT>\install\_install_models_all.sql ( app will auto create sample data ).
3. In Global.asax, in method "Configure()" change flag useRealData from false to true. e.g. [bool useRealData = true;]
4. Change db connection in <ROOT>\config\dev.config
5. Run the app.


Admin account
1. admin  user/pass : "admin", "password"
2. power  user/pass : "power", "password"
3. normal user/pass: "user1", "password"


Authentication
Custom Forms Authentication is used. This means:
1. Asp.Net Membership Providers are NOT used
2. A custom "Users" component is used.
3. After signing in and validating the user credentials, FormsAuthentication is activated
4. The username and roles are persisted in a encrypted cookie
5. In Global.asax Application_AuthenticateRequest() handler rebuilds the Context.User ( IPrincipal ) using the cookie information.