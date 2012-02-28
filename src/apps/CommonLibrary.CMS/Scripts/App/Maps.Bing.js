/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading bing maps.
********************************************************************** */

///<summary>Creates a new bing map object</summary>
function BingMap(mapsdivId) 
{
    this.Map = null;
    this.PinId = 0;
    this.ViewId = mapsdivId;
    this.Init();
}


///<summary>Initializes the map.
///</summary>
BingMap.prototype.Init = function() 
{
    this.Map = new VEMap(this.ViewId);
    this.Map.LoadMap();
}


///<summary> Find the location / address and display it on the map.</summary>
///<param name="address">e.g. 222 broadway, new york, new york</param>
BingMap.prototype.Locate = function(address) 
{
    try 
    {
        this.Map.Find(null, address);
        this.AddPushpinToCenter(address);
    }
    catch (e) {
        alert(e.message);
    }
}


///<summary> Find the location / address and display it on the map.</summary>
///<param name="address">e.g. 222 broadway, new york, new york</param>
BingMap.prototype.AddPushpinToCenter = function(address) 
{
    var shape = new VEShape(VEShapeType.Pushpin, this.Map.GetCenter());
    shape.SetTitle('Address');
    shape.SetDescription(address);
    this.PinId++;
    this.Map.AddShape(shape);
}