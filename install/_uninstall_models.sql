IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Users_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Users_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Logs_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Logs_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
DROP TABLE [dbo].[Logs]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Configs_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Configs_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Configs_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Configs_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Configs]') AND type in (N'U'))
DROP TABLE [dbo].[Configs]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pages_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Pages_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
DROP TABLE [dbo].[Pages]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Parts_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Parts_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Parts_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Parts_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Parts]') AND type in (N'U'))
DROP TABLE [dbo].[Parts]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Posts_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Posts_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts]') AND type in (N'U'))
DROP TABLE [dbo].[Posts]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Events_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Events_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
DROP TABLE [dbo].[Events]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Links_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Links_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Links_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Links_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Links]') AND type in (N'U'))
DROP TABLE [dbo].[Links]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Tags_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Tags_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND type in (N'U'))
DROP TABLE [dbo].[Tags]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedbacks_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Feedbacks_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedbacks_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Feedbacks_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedbacks]') AND type in (N'U'))
DROP TABLE [dbo].[Feedbacks]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Faqs_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Faqs_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Faqs_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Faqs_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Faqs]') AND type in (N'U'))
DROP TABLE [dbo].[Faqs]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Profiles_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Profiles_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profiles]') AND type in (N'U'))
DROP TABLE [dbo].[Profiles]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Widgets_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Widgets_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Widgets]') AND type in (N'U'))
DROP TABLE [dbo].[Widgets]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WidgetInstances_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WidgetInstances_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WidgetInstances_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WidgetInstances_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WidgetInstances]') AND type in (N'U'))
DROP TABLE [dbo].[WidgetInstances]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuEntrys_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MenuEntrys_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuEntrys_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MenuEntrys_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuEntrys]') AND type in (N'U'))
DROP TABLE [dbo].[MenuEntrys]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Themes_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Themes_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Themes_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Themes_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Themes]') AND type in (N'U'))
DROP TABLE [dbo].[Themes]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Flags_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Flags_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Flags_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Flags_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Flags]') AND type in (N'U'))
DROP TABLE [dbo].[Flags]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Favorites_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Favorites_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Favorites_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Favorites_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Favorites]') AND type in (N'U'))
DROP TABLE [dbo].[Favorites]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comments_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Comments_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comments_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Comments_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comments]') AND type in (N'U'))
DROP TABLE [dbo].[Comments]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resources_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Resources_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resources_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Resources_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resources]') AND type in (N'U'))
DROP TABLE [dbo].[Resources]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OptionDefs_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OptionDefs_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs]') AND type in (N'U'))
DROP TABLE [dbo].[OptionDefs]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFiles_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFiles_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles]') AND type in (N'U'))
DROP TABLE [dbo].[MediaFiles]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFolders_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFolders_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders]') AND type in (N'U'))
DROP TABLE [dbo].[MediaFolders]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorys_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Categorys_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorys_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Categorys_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorys]') AND type in (N'U'))
DROP TABLE [dbo].[Categorys]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Countrys_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Countrys_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countrys]') AND type in (N'U'))
DROP TABLE [dbo].[Countrys]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[States_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[States_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[States_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[States_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[States]') AND type in (N'U'))
DROP TABLE [dbo].[States]
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Citys_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Citys_GetByFilter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Citys_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Citys_GetRecent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Citys]') AND type in (N'U'))
DROP TABLE [dbo].[Citys]
go

