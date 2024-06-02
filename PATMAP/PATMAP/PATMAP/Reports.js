function getMapValues(municipalityLayerName, schoolLayerName, parcelLayerName)
{
    if (top.map && top.map.getMap && top.map.getMap() && !top.map.getMap().isBusy() && top.map.getMap().getSelection())
    {
        var map = top.map.getMap();
        var sel = map.getSelection();
        
        var municipalityLayer = map.getMapLayer(municipalityLayerName); 
        var objs = sel.getMapObjectsEx(municipalityLayer);        
        if (objs.size() == 1)
        {
            targ = document.getElementById('MunicipalityID');
            targ.value = objs.item(0).getKey();
            return;
        }
        
        var schoolLayer = map.getMapLayer(schoolLayerName); 
        objs = sel.getMapObjectsEx(schoolLayer);
        if (objs.size() == 1)
        {
            targ = document.getElementById('SchoolID');
            targ.value = objs.item(0).getKey();
            return;
        }
        
        var parcelLayer = map.getMapLayer(parcelLayerName); 
        objs = sel.getMapObjectsEx(parcelLayer);
        if (objs.size() == 1)
        {
            targ = document.getElementById('ParcelID');
            targ.value = objs.item(0).getKey();
            return;
        }
    }
    
}
