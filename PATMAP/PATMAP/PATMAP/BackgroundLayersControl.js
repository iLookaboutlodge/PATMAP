function toggleLayer(layerName) {
    if (event.srcElement.checked) {
       
       // var map = document.getElementById("mapFrame");
        var map = parent.parent.mapFrame;
       // map.turnOnLayerByName(layerName);
        var layer = map.GetLayers(layerName);
        layer.visible = true;
        parent.parent.mapFrame.Refresh(); 
    }
    else {
        top.map.turnOffLayerByName(layerName);
    }
}

function toggleGroup(groupName) {
    if (event.srcElement.checked) {
        top.map.turnOnGroupByName(groupName);
    }
    else {
        top.map.turnOffGroupByName(groupName);
    }
}

function toggleGroupPanel(groupPanelID) {
    var panel = document.getElementById(groupPanelID);

    if (panel) {
        if (panel.style.display == 'none') {
            panel.style.display = 'inline';
        }
        else {
            panel.style.display = 'none';
        }
    }
}