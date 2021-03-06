IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs]') AND type in (N'U'))
DROP TABLE [dbo].[OptionDefs]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[OptionDefs]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[AppId] [int] NULL,
[Section] [nvarchar](50) NULL,
[Key] [nvarchar](50) NULL,
[ValType] [nvarchar](20) NULL,
[Values] [ntext] NULL,
[DefaultValue] [nvarchar](200) NULL,
[DisplayStyle] [nvarchar](20) NULL,
[IsRequired] [bit] NULL,
[IsBasicType] [bit] NULL,
[SortIndex] [int] NULL,
 CONSTRAINT [PK_OptionDefs] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OptionDefs_GetByFilter]
go

CREATE PROCEDURE [dbo].[OptionDefs_GetByFilter]
(
	@Filter varchar(250),
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )
	declare @sql as nvarchar(4000)
	
	-- create dynamic sql
	set @sql = 'SELECT OptionDefs.id FROM OptionDefs '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the OptionDefs in the page requested
	SELECT    OptionDefs.*
	FROM      OptionDefs
	WHERE	  OptionDefs.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OptionDefs_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OptionDefs_GetRecent]
go


CREATE PROCEDURE [dbo].[OptionDefs_GetRecent]
(
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )

	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts (id)
			SELECT Id 
			FROM   OptionDefs with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the OptionDefs in the page requested
	SELECT    OptionDefs.*
	FROM      OptionDefs
	WHERE	  OptionDefs.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
