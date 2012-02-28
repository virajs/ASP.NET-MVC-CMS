/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading google maps.
********************************************************************** */


///<summary>Creates a new bing map object</summary>
function GoogleMap(mapsdivId) 
{
    this.Map = null;
    this.Geocoder = null;
    this.ViewId = mapsdivId;
    this.Init();
}


///<summary>Initializes the map.
///</summary>
GoogleMap.prototype.Init = function() 
{
    if (GBrowserIsCompatible()) {
        this.Map = new GMap2(document.getElementById(this.ViewId));
        this.Geocoder = new GClientGeocoder();

        // Default map to some arbitraty center.
        this.Map.setCenter(new GLatLng(37.4419, -122.1419), 13);

        // Sets up the map type control in the top-right corner.
        // This is "map, satellite, hybrid".
        var mapTypeControl = new GMapTypeControl();
        var topRight = new GControlPosition(G_ANCHOR_TOP_RIGHT, new GSize(10, 10));
        var bottomRight = new GControlPosition(G_ANCHOR_BOTTOM_RIGHT, new GSize(10, 10));
        this.Map.addControl(mapTypeControl, topRight);

        var map = this.Map;
        // This moves the type control to the bottom if user double-clicks the map to zoom in.
        GEvent.addListener(map, "dblclick", function() {
            map.removeControl(mapTypeControl);
            map.addControl(new GMapTypeControl(), bottomRight);
        });

        // This shows the zoom in/out, and move control on the left side.
        this.Map.addControl(new GSmallMapControl());
    }
}


///<summary> Find the location / address and display it on the map.</summary>
///<param name="address">e.g. 222 broadway, new york, new york</param>
GoogleMap.prototype.Locate = function(address) 
{
    if (this.Geocoder) 
    {
        var map = this.Map;
        this.Geocoder.getLatLng(address, function(point) 
        {
            if (!point) 
            {
                // alert(address + " not found");
            }
            else 
            {
                // This sets the center of the map to the address supplied.
                map.setCenter(point, 15);

                // Adds a marker to the map.
                var marker = new GMarker(point);
                map.addOverlay(marker);
                GEvent.addListener(marker, "click", function() 
                {
                    marker.openInfoWindowHtml(address);
                });
            }
        });
    }
}