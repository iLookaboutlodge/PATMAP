
var isLoad = false;

function isLoaded()
{	
	isLoad = true;
}

function getMap()
{
     return parent.parent.GetMapFrame();
    

}

function selectMode()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.selectMode();
    }
}

function panMode()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.panMode();
    }
}

function panMode()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.panMode();
    }
}

function zoomInMode()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.zoomInMode();
    }
}

function zoomOutMode()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.zoomOutMode();
    }
}

function zoomPrevious()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.zoomPrevious();
    }
}

function unzoom()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.zoomOut();
    }
}

function zoomPrevious()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.zoomPrevious();
    }
}

function clearSelection()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        var sel = map.getSelection();
        if (sel)
        {
            sel.clear();
        }
    }
}

function printMap()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.printDlg();
    }
}

function copyMap()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.copyMap();
    }
}

function measureMap()
{
    var map = getMap();
    
    if (map && !map.isBusy())
    {
        map.viewDistance("M");
    }
}

function reload()
{
    var map = getMap();
    
    if (map)
    {
        map.stop();
        if (map.getUrl().length > 0)
        {
            mapLayers = map.getMapLayersEx();
            layerVisibility = "&LayerVisibility=";
            for (var i = 0; i < mapLayers.size(); i++)
            {
                if (mapLayers.item(i).getVisibility())
                {
                    layerVisibility = layerVisibility + mapLayers.item(i).getName() + "=1;";
                }
                else
                {
                    layerVisibility = layerVisibility + mapLayers.item(i).getName() + "=0;";
                }
            }        
            map.setUrl(map.getUrl() + layerVisibility);
         
        }
    }
}

function zoomBox(lat, lon, width, height)
{
    var map = getMap();
    var extents = map.getMapExtent(true, true);
    var mapWidth = extents.getMaxX() - extents.getMinX();
    var mapHeight = extents.getMaxY() - extents.getMinY();
    var mapRatio = mapWidth/mapHeight;
    var objRatio = width/height;
    if (mapRatio > objRatio)
    {
        width = height * mapRatio;
    }
 //   map.zoomWidth(lat, lon, width, 'M');
    parent.parent.mapFrame.ZoomToView(lat, lon, 500000, true);
}

function turnOnLayerByName(layerName)
{
    var map = getMap();
    
    if (map)
    {
        map.stop();
       // var layer = map.getMapLayer(layerName);
        var layer = map.GetLayers(layerName);
        
        if(layer)
        {
            layer.visible = true;
            map.Refresh(); 
        }
    }
}

function turnOffLayerByName(layerName)
{
    var map = getMap();
    
    if (map)
    {
        map.stop();
      //  var layer = map.getMapLayer(layerName);
        var layer = map.GetLayers(layerName);
        
        if(layer)
        {
            layer.visible = false;
            map.Refresh(); 
        }
    }
}

function switchLayers(onLayerName, offLayerName)
{
    var map = getMap();
    
    if (map)
    {
        var onLayer = map.getMapLayer(onLayerName);
        var offLayer = map.getMapLayer(offLayerName);
        
        if(onLayer && offLayer)
        {
            onLayer.setVisibility(true);
            offLayer.setVisibility(false);
            map.refresh();
        }
    }
}

function turnOnGroupByName(groupName)
{
    var map = getMap();
    
    if (map)
    {
        map.stop();
        var group = map.getMapLayerGroup(groupName);
        
        if(group)
        {
            group.setVisibility(true);
            map.refresh();
        }
    }
}

function turnOffGroupByName(groupName)
{
    var map = getMap();
    
    if (map)
    {
        map.stop();
        var group = map.getMapLayerGroup(groupName);
        
        if(group)
        {
            group.setVisibility(false);
            map.refresh();
        }
    }
}

var selectionChangedHandlers;

function addSelectionChangedHandler(handler)
{
    if (!selectionChangedHandlers)
    {
        selectionChangedHandlers = new Array(0);
    }
    for (var i = 0; i < selectionChangedHandlers.length; i++)
    {
        if (selectionChangedHandlers[i] == handler)
        {
            return;
        }
    }
    selectionChangedHandlers[selectionChangedHandlers.length] = handler;
}

function onSelectionChanged()
{
    if (selectionChangedHandlers)
    {
        for (var i = 0; i < selectionChangedHandlers.length; i++)
        {
            var handler = selectionChangedHandlers[i];
            if (handler)
            {
                try
                {
                    eval(handler);
                } catch (ex) {}
            }
        }
    }
}

function turnOnLayer(layer)
{   var map = getMap();
    layer.setVisibility(true);
    if(!layer.isVisible())
	{  if (layer.getVisibility())
	   {  var layerStyles = layer.getMapLayerStyles();
		  if (layerStyles.count > 0)
		  {  var max = 1;
		     var i = 0;
		     for (i = 0; i < layerStyles.count; i++)
		     {  if (layerStyles(i).getMaxDisplayRange() > max)
		        { max = layerStyles(i).getMaxDisplayRange();
		        }
		     }
		     map.setScale(max);
		     return false;
	      }//if layerstyles
	   }//if layer.getVisibility
	}//layer.isvisible
	return true;
}

function zoomToObject(lat, lon)
{	var map = getMap();
	if (!map.isBusy())
    {
        map.zoomScale(lat, lon, map.getScale());
    }
}

function getLayerMax(layer)
{	var map = getMap();
	var layerStyles = layer.getMapLayerStyles();
	var max = 1;
	if (layerStyles.count > 0)
	{	var i = 0;
		for (i = 0; i < layerStyles.count; i++)
		{	if (layerStyles(i).getMaxDisplayRange() > max)
		    { max = layerStyles(i).getMaxDisplayRange();
		    }
		}
	}//if layerstyles
	return max;
}

function zoomToSelected()
{
    var map = getMap();
    if (!map.isBusy())
   {  var sel = map.getSelection();
      if (sel != null)
      {  var objs = sel.getMapObjectsEx(null);
         if (objs.count == 1)
         {  
            var obj = objs.item(0)
            var layer = obj.getMapLayer();
            var type = layer.getLayerType();
			if (type == 'Point')
			{ var extent = obj.getExtent();
			  var lat = (extent.getMaxLat() + extent.getMinLat())/2;
			  var lon = (extent.getMaxLon() + extent.getMinLon())/2;
			  					  
			  var max = getLayerMax(layer);
			  
		      if (max == 1)
		      {  max = 999999999999;
		      }
		      //alert(max);
		      map.zoomScale(lat, lon, Math.min(2000, max));
		      
			}
			else
			{  //finishZoom = true;
			   var extent1 = obj.getExtent();
			   var lat = (extent1.getMaxLat() + extent1.getMinLat())/2;
			   var lon = (extent1.getMaxLon() + extent1.getMinLon())/2;
			   var extent = obj.getExtentEx(true);
			   var objWidth = (extent.getMaxX() - extent.getMinX()) * map.getMCSScaleFactor();
			   var objHeight = (extent.getMaxY() - extent.getMinY()) * map.getMCSScaleFactor();
			   var objRatio = objHeight/objWidth;
			   var width = map.getWidth('M');
			   var height = map.getHeight('M');
			   var mapRatio = height/width;
			   
			   var mapExtent = map.getMapExtent(true, true);
			   var Xdiff = (mapExtent.getMaxX() - mapExtent.getMinX()) * map.getMCSScaleFactor();
			   var scale = map.getScale();
			   var screenWidth = Xdiff/scale;
			   
			   if (objRatio / mapRatio >= 1)
			   {  height = objHeight;
			      width = height / mapRatio;
			   }
			   else
			   {  width = objWidth;
			   }
			   
			   var layer = obj.getMapLayer();
			   
			   var max = getLayerMax(layer);
		       
		       if ((width * 2) / screenWidth > max)
			   {  map.zoomScale(lat, lon, max);
			   }
			   else
			   {  map.zoomWidth(lat, lon, width * 2, 'M');
			   }
			   
			   //map.zoomSelected();
			   					
			} //if type==point
         }
         else
         {  map.zoomSelected();
         }
      }
   }
}
		

var selKey = "";
var selLayer = "";
var selLat = 0;
var selLon = 0;
var attempts = 0;	
var finishZoom = false;	

var multiSelKey = new Array();
var multiSelLat = new Array();
var multiSelLon = new Array();
function selectObject(key, layer, lat, lon)
{	var map = getMap();
  	if (map && !map.isBusy())
	{	var sel = map.getSelection();
		if (sel != null)
		{	sel.clear();
		    continueSelect(key, layer, lat, lon);
		}
	}
}

function selectMultipleObjects(keys, layer, lats, lons)
{
    multiSelKey = keys;
    multiSelLat = lats;
    multiSelLon = lons;
    var map = getMap();
  	if (map && !map.isBusy())
	{	var sel = map.getSelection();
		if (sel != null)
		{	sel.clear();
		    continueSelect("", layer, null, null);
		}
	}
}

function continueSelect(key, layerName, lat, lon)
{	
    var map = getMap();	
    if (key == "" && multiSelKey.length > 0)
    {
        key = multiSelKey.pop();
        lat = multiSelLat.pop();
        lon = multiSelLon.pop();
        attempts = 0;
    }
    if (key != "")
	{		
		attempts = attempts + 1;
		if (attempts > 3)
		{  attempts = 0;
			selKey = "";
			selLayer = "";
			selLat = 0;
			selLon = 0;
			return false;
		}
		if (key != undefined && key != "") {
		selKey = key;
		selLayer = layerName;
		selLat = lat;
		selLon = lon;		
		
		if (map && !map.isBusy())
		{	var sel = map.getSelection();
			
			if (sel != null)
			{	//alert(sel.getAsString(null, "'"));
				var layers = map.getMapLayers();
				var layer = null;
				for (var i = 0; i < layers.count; i++)
				{  var tempLayer = layers(i);
				   var tempLayerName = '' + tempLayer.getName();
				   if (tempLayerName.indexOf(layerName) >= 0 && tempLayerName == layerName)
				   {  layer = tempLayer;
				   }
				}
				
				if (layer != null)
				{  var layerOn = turnOnLayer(layer);
					if (layerOn)
					{   
						var obj = layer.getMapObject(key);
						if (obj != null)
						{  sel.addObject(obj, false);
							attempts = 0;
							selKey = "";
							selLayer = "";
							selLat = 0;
							selLon = 0;
							if (multiSelKey.length == 0)
							{
							    zoomToSelected();
							    return true;
							}
							else
							{
							    continueSelect("", layerName, null, null);
							}
						}
						else
						{  
						    if (multiSelKey.length == 0)
						    {
							    zoomToObject(lat, lon)
							}
							return false;
					    }//obj!=null
					}
					else
					{  return false; //Wait for layers
					}//layerOn
				} 
		        else
		        {  zoomToObject(lat, lon)
				   attempts = 0;
				   selKey = "";
				   selLayer = "";
				   selLat = 0;
				   selLon = 0;
		        }
			}
		  }
		}
	}
	return false;		  
}