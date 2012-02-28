var AjaxHelper =
{
    ///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
    ///</summary>
    ///<param name="url">The url to send the ajax action to.</param>
    SendActionAndCallback: function(url, callback) {
        $.getJSON(url, function (result) {
            callback(result);
        });
    },



    ///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
    ///</summary>
    ///<param name="url">The url to send the ajax action to.</param>
    SendActionAndAlert: function(url) {
        $.getJSON(url, function (result) {
            alert(result.Message);
        });
    },
    

    ///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
    ///</summary>
    ///<param name="url">The url to send the ajax action to.</param>
    SendActionAndDisplayMessage: function(url, divId) {
        $.getJSON(url, function (result) {
            DisplayActionResultMessage(result, divId);
        });
    }
}


/* @summary: Displays the result message in a div, after applying the proper css class based on error/success. 
@param {object} result: The result object that contains a "Success"(bool) and "Message"(string) properties.
@param {string} divId: The div id that the message should appear in. */
function DisplayActionResultMessage(result, divId) {
    var msg = result.Success ? '<span class="message">' : '<span class="error">';
    msg = msg + result.Message + "</span>";
    $('#' + divId).html(msg);
}

///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
///</summary>
///<param name="url">The url to send the ajax action to.</param>
function SendActionAndDisplayResultInDiv(url, divId) {
    $.getJSON(url, function (result) {
        DisplayActionResultMessage(result, divId);
    });
}


///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
///</summary>
///<param name="url">The url to send the ajax action to.</param>
function SendActionAndDisplayResult(url) {
    $('#adminActionResult').html("");
    $.getJSON(url, function(result) {
        DisplayActionResultMessage(result, "adminActionResult");
    });
}


///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
///</summary>
///<param name="url">The url to send the ajax action to.</param>
function SendActionAndCallback(url, callback) {
    $.getJSON(url, function (result) {
        callback(result);
    });
}


///<summary>Sends an ajax get request to the url specified and displays the result message at the top of the page.
///</summary>
///<param name="url">The url to send the ajax action to.</param>
function SendActionAndDisplayResultInDivWithCallback(url, divId, callback) {
    $.getJSON(url, function(result) {
        DisplayActionResultMessage(result, divId);
        callback(result.Success);
    });
}


///<summary>Sends an ajax get request to the url specified and displays the result's "Message" property if there is an error
/// otherwise, displays the results "Data" property in the div specified.
///</summary>
///<param name="url">The url to send the ajax action to.</param>
function SendActionAndDisplayResultDataInDiv(url, divId) {
    $.getJSON(url, function(result) {
        var msg = "";
        if(result.Success)
            msg = result.Data;
        else
            msg = '<span class="error">' + result.Message + "</span>";
                        
        $('#' + divId).html(msg);
    });
}


///<summary>Loads a dropdown using json data from the url supplied.</summary>
///<param name="url">The url for the ajax fetch</param>
///<param name="optionsId">The id of the options/dropdown.</param>
///<param name="divIdForError">The id of the div to display an error if load fails.</param>
///<param name="selectedVal">The value to select in the dropdown after all the data is loaded.</param>
function LoadDropDown(url, optionsId, divIdForError, selectedVal) {
    
    $.getJSON(url, function (result) {
        var msg = "";
        var optionscontrol = document.getElementById(optionsId);

        if (result.Success) {
            
            optionscontrol.options.length = 0;

            // Now populate the new options.
            for (i = 0; i < result.Data.length; i++) {
                var optionval = result.Data[i];
                optionscontrol.options.add(new Option(optionval, optionval));
            }
            if (selectedVal && selectedVal != "") {
                $("#" + optionsId).val(selectedVal);
            }
        }
        else {
            msg = '<span class="error">' + result.Message + "</span>";
            $('#' + divIdForError).html(msg);
            optionscontrol.options.length = 0;
        }
    });
}


///<summary>Loads a dropdown using json data from the url supplied.</summary>
///<param name="dependentOptionsId">The id of the options/drop-down to use</param>
///<param name="url">The url for the ajax fetch</param>
///<param name="optionsId">The id of the options/dropdown.</param>
///<param name="divIdForError">The id of the div to display an error if load fails.</param>
///<param name="selectedVal">The value to select in the dropdown after all the data is loaded.</param>
function LoadDropDownDependent(dependentOptionsId, url, optionsId, divIdForError, selectedVal) {
    var ndx = document.getElementById(dependentOptionsId).selectedIndex;
    var val = document.getElementById(dependentOptionsId).options[ndx].value;
    var dependentval = $('#' + dependentOptionsId + '>option:selected').text();
    var finalurl = url + encodeURI(dependentval);
    LoadDropDown(finalurl, optionsId, divIdForError, selectedVal);
}


///<summary>Expands or collapses a section with a +, - text.</summary>
///<param name="id">The id of the div to expand collapse</param>
///<param name="idForExpandCollapseLink">The id of the anchor tag that 
//  will contain the + or -.
function ExpandCollapse(id, idForExpandCollapseLink, handleShowCollapse) {
    // First toggle.
    $("#" + id).toggle();

    if (handleShowCollapse) {
        // Check hidden state and set either the + (expand) or -(collapse) sign
        if ($("#" + id).is(':visible')) {
            $("#" + idForExpandCollapseLink).text("- ");
        }
        else {
            $("#" + idForExpandCollapseLink).text("+ ");
        }
    }
}


///<summary>Expands / Collapses all items supplied</summary>
///<param name="allNames">Comma delimited list of div to expand/collapse.</param>
///<param name="show">Whether or not to show the items.</param>
///<param name="suffixForLink>The suffix to apply to each name to reprent it's 
///   respective link containing the "+", "-".
function ExpandCollapseAll(allNames, show, suffixForLink) {
    var names = allNames.split(',');
    for (i = 0; i < names.length; i++) {
        var name = names[i];
        if (name && name.length > 0) {
            if (show) {
                $("#" + name).show();
                $("#" + name + suffixForLink).text("- ");
            }
            else {
                $("#" + name).hide();
                $("#" + name + suffixForLink).text("+ ");
            }
        }
    }
}
