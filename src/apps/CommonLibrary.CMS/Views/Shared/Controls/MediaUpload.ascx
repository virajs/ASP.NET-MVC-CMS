<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.MediaUploadViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
<%@ Import Namespace="ComLib.Data" %>

<% string displayWidth = Model.Width > 0 ? "width: " + Model.Width + "px;" : ""; %>

<div style="<%= displayWidth %>">
    <% if(Model.AllowExternalFiles) { %>
         Upload files from your computer or reference files on the web using urls in "Existing File Reference" field.<br />    
        <span class="example">http://www.flickr.com/johndoe/album1/green-earth.jpg</span>
    <% } %>
    <br /><br />
    
    <% for(int ndx = 0; ndx < Model.NumberOfUploadsAllowed; ndx++){ %>
        <div class="separator"><br /></div><br />
        <% if(Model.NumberOfUploadsAllowed > 1){ %><h3>Upload file : <%=ndx + 1 %></h3> <%} %>
    
        <p>
            <input type="file" name="file<%=ndx %>" id="file<%=ndx %>" class="file" />
        </p>

        <% if (Model.AllowExternalFiles)
            { %>
           
             <h4>or</h4>
             <p>
                <label for="Description">Existing File Reference</label>
                <input type="text" id="externalfile<%=ndx %>" class="small" /><br />
                <span class="example">http://www.flickr.com/johndoe/green-earth.jpg</span><br />
            </p>
        <% } %>

        <p>
            <label for="Title">Title</label> 
            <%= Html.TextBox("title" + ndx, "", new { @class = "small" })%>
        </p>

        <p>
            <label for="Description">Description</label>             
            <%= Html.TextArea("description" + ndx, new { @class = "small" })%>
        </p>

        <p>
            <label for="Description">Is Public</label>             
            <%= Html.CheckBox("ispublic" + ndx) %>
        </p>
    <%} %>
</div>