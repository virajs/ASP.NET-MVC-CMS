<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<string>>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Import</h2>
    <fieldset>
        <%= Html.Label("Model") %><%= Html.DropDownList("Models", Html.ToDropDownList(Model[0], Model.ToArray())) %><br /><br />
        <%= Html.Label("Import Text") %><textarea id="importText" class="text-area"></textarea>
    </fieldset>
    <br />
    <input type="button" id="import" value="Import" class="action" onclick="DoImport();" /><br />
    <div id="importSuccessMessage" class="status_message" style="display:none"></div>
    <div id="importErrorMessage" class="status_error" style="display:none"></div><br /><br />
       
    <div>
    <h2>Notes:</h2>
    <ul>
        <li>You can import multiple models</li>
        <li>Each model entry must have a header (the name of the model surrounded by "[" "]" as in "[Event])</li>
        <li>The header must be the first line in each model.</li>
        <li>Model properties are in key ":" value format. e.g. Cost : $25</li>
        <li>Surround multi-line text values with double quotes ("")</li>
        <li>Use field "RefKey" field to refer to the entries you import. e.g. "RefKey" : Import1</li>
        <li>Some substituion values are available. e.g. ${t} for Today's date. ${t+1} for Today + 1 day.</li>
    </ul>
    </div><br />
    <h2>Example</h2>
    <input type="button" id="showexample" value="Show" class="action" onclick="ShowExampleData();" />
    <input type="button" id="hideexample" value="Hide" class="action" onclick="HideExampleData();" /><br /><br />    
    <div id="importExample"></div><br /><br />
    <h2>Schema</h2>  
    <div id="importSchema"></div><br /><br /><br />


    <script type="text/javascript">
        ///<summary>Display an example of the data to import for a specific model.
        function ShowExampleData() {
            
            // Show example input
            var model = $('#Models').val();
            SendActionAndDisplayResultDataInDiv('/tools/ImportExampleFor?model=' + model, "importExample");
            $('#importExample').show();

            // Also show the schema by default.
            var model = $('#Models').val();
            SendActionAndDisplayResultDataInDiv('/tools/ImportSchemaFor?model=' + model, "importSchema");
            $('#importSchema').show();
        }


        ///<summary>Display an example of the data to import for a specific model.
        function HideExampleData() {
            $('#importExample').hide();
            $('#importSchema').hide();
        }


        //<summary>Send the import content to the server for processing.</summary>
        function DoImport() {
            var model = $('#Models').val();            
            var content = $("#importText").val();
            var format = "ini";
            var url = '/tools/doimport';
            $("#importSucessMessage").html("");
            $("#importErrorMessage").html("");
            $.post(url, { Content: content, Model: model, Format: format }, function (data) {
                var message = data.Message;
                if (data.Success === "true") {
                    $("#importSucessMessage").show();
                    $("#importSucessMessage").html(message);
                }
                else {
                    $("#importErrorMessage").show();
                    $("#importErrorMessage").html(message);
                }
            }, "json");
        }
    </script> 

</asp:Content>