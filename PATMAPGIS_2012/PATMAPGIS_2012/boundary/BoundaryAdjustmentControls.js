function init() {
	InitSelectedParcels();
	var elem1 = parent.parent.tbFrame;
	if (typeof elem1.addNewHandler == "function") {
		//custom function added	for BoundaryChange Use Map viewerfiles/toolbar.templ 10-oct-2013
		elem1.addNewHandler();
	}
}

function onSelectionChanged() {
	var map = parent.parent.mapFrame;
	if (typeof map.CustomRequestSelectedProperties == "function") {
		//custom function added	for BoundaryChange Use Map viewerfiles/ajaxmappane.templ 10-oct-2013
		var resp = map.CustomRequestSelectedProperties();
		CustomProcessSelectedFeatureSet(resp);
	}
}

function CustomProcessSelectedFeatureSet(resp) {
	InitSelectedParcels();

	if (!resp) {
		return;
	}
	
	var selFeatures = {};

	for (var layerName in resp) {
		if (layerName == "Source_filtered") {
			selFeatures[layerName] = resp[layerName];
			var count = selFeatures[layerName].length;
			for (var i = 0; i < count; i++) {
				var feat = selFeatures[layerName][i];
				CustomSetProperties(1, feat.values);
			}
		}
	}
}

function InitSelectedParcels() {
	var lst = document.getElementById('lstSelectedParcels');
	var hdnSelectedParcels = document.getElementById('SelectedParcels');
	var hdnSelectedISCParcels = document.getElementById('SelectedISCParcels');
	if (lst && hdnSelectedParcels && hdnSelectedISCParcels) {
		while (lst.options.length > 0) {
			lst.options.remove(0);
		}
		hdnSelectedParcels.value = '';
		hdnSelectedISCParcels.value = '';
	}
}

function CustomSetProperties(count, featvalues) {
	for (var i = 0; i < featvalues.length; i++) {
		if (featvalues[i].name == "P_ID") {
			//alert(featvalues[i].value);
			var lst = document.getElementById('lstSelectedParcels');
			var hdnSelectedParcels = document.getElementById('SelectedParcels');
			var hdnSelectedISCParcels = document.getElementById('SelectedISCParcels');
			if (lst && hdnSelectedParcels && hdnSelectedISCParcels) {
				addToSelection(lst, hdnSelectedParcels, featvalues[i].value, featvalues[i].value);
			}
		}
	}
}

function testfc() {
	//var elem = parent.parent.mapFrame.document.getElementById['body'];
	//	var elem = parent.parent.mapFrame.body;
	//	if (elem.addEventListener) {
	//		elem.addEventListener("contextmenu", function() { alert('t1'); }, false);
	//	}

	//	if (typeof elem.onmousedown == "function") {
	//		elem.click(function() {
	//			alert('test');
	//		});
	//		//		if (elem.addEventListener) {
	//		//			elem.addEventListener("onclick", yourFunction, false);
	//		//		}
	//		//		//elem.onclick.apply(elem);
	//	}
	//	////	if (parent.parent.mapFrame.body.addEventListener) {
	//		parent.parent.mapFrame.body.addEventListener("OnMouseDown", yourFunction, false);
	//	}
	//setOnSelectionChangedHandler();  
	//onSelectionChanged();
}

function setOnSelectionChangedHandler()
{
    if (top.map && top.map.addSelectionChangedHandler)
    {
        top.map.addSelectionChangedHandler('top.controlpanel.onSelectionChanged();');
    }
    else
    {
        window.setTimeout('setOnSelectionChangedHandler();', 1000);
    }
}

function onSelectionChangedOLD()
{
    var lst = document.getElementById('lstSelectedParcels');
    var hdnSelectedParcels = document.getElementById('SelectedParcels');
    var hdnSelectedISCParcels = document.getElementById('SelectedISCParcels');
    if (lst && hdnSelectedParcels && hdnSelectedISCParcels)
    {
        while (lst.options.length > 0)
        {
            lst.options.remove(0);
        }
        hdnSelectedParcels.value = '';
        hdnSelectedISCParcels.value = '';
        if (top.map && top.map.getMap)
        {
            var map = top.map.getMap();
            if (map)
            {
                var selection = map.getSelection();
                var layer = map.getMapLayer('Source');
                if (selection && layer)
                {
                    var mapObjects = selection.getMapObjectsEx(layer);
                
                    for (var i = 0; i < mapObjects.size(); i++)
                    {
                        addToSelection(lst, hdnSelectedParcels, mapObjects.item(i).getKey(), mapObjects.item(i).getName());
                    }
                }
                layer = map.getMapLayer('ISC_Parcels');
                if (selection && layer)
                {
                    var mapObjects = selection.getMapObjectsEx(layer);
                
                    for (var i = 0; i < mapObjects.size(); i++)
                    {
                        hdnSelectedISCParcels.value += mapObjects.item(i).getKey() + ';';
                    }
                }
            }
        }
    }
}

function addToSelection(lst, hdn, value, name)
{
    var oOption = document.createElement("OPTION");
    lst.options.add(oOption);
    oOption.innerText = name;
    oOption.value = value;
    hdn.value += value + ';';
}
