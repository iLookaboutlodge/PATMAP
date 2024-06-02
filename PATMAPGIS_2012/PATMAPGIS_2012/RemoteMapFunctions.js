function TmpRefresh() {
	var mapFrame = parent.parent.mapFrame;
	var tbFrame = parent.parent.tbFrame;
	//var elem = tbFrame.document.getElementsByName('testID')[0];
	var elem = tbFrame.document.getElementsByName('0')[0];
	if (typeof elem.onclick == "function") {
		elem.onclick.apply(elem);
	}
	//tbFrame.ExecuteCommand(18, false, '0');
	//mapFrame.ExecuteMapAction(20);
}
function openTablesLink(type) {
	//parent.parent.document.location = 'assmnt/tables.aspx';
	if (type == "assmnt") {
		top.location.href = 'assmnt/tables.aspx';
	} else if (type == "taxtools") {
		top.location.href = 'taxtools/tables.aspx';
	} else if (type == "boundary") {
		top.location.href = 'boundary/tables.aspx';
	}
}

function openGraphsLink(type) {
	if (type == "assmnt") {
		top.location.href = 'assmnt/graphs.aspx';
	} else if (type == "taxtools") {
		top.location.href = 'taxtools/graphs.aspx';
	} else if (type == "boundary") {
		top.location.href = 'boundary/graphs.aspx';
	}
}

function ProgressON() {
	var elem = document.getElementById("progressContainer");
	elem.style.display = "block";
}

function ProgressOFF() {
	var elem = document.getElementById("progressContainer");
	elem.style.display = "none";
}

function SetHiddenElemValue(hvalue) {
	try {
		var elem = document.getElementById("ctrltestid");
		if (elem) {
			elem.value = hvalue;
		}
	} catch (ex) {
		alert(ex);
	}
}

function GethdnLTTMapValue() {
	try {
		var elem = document.getElementById("hdnLTTMap");
		if (elem) {
			return elem.value;
		}
	} catch (ex) {
		alert(ex);
	}
	return "";
}

function StartDoRefreshMap() {
	SetHiddenElemValue("");
	var elem1 = parent.parent.tbFrame;
	if (typeof elem1.DoRefreshMap == "function") {
	    elem1.DoRefreshMap();
	}
}

function StartShowZoomedMap(q_x, q_y, width, height) {
	SetHiddenElemValue("");
	var elem1 = parent.parent.tbFrame;
	if (typeof elem1.ShowZoomedMap == "function") {
		elem1.ShowZoomedMap(q_x, q_y, width, height);
	}
}

function InitializeControlPageRequestHandler(sender, args) {
	var vLTTMap = GethdnLTTMapValue();
	if (vLTTMap.indexOf("LTTMap") != -1) {
		var coord = getMapCoordinates(vLTTMap);
		if (coord != "") {
			setTimeout("StartShowZoomedMap(" + coord + ");", 10);
		}
	} else if (vLTTMap.indexOf("BOUNDARYCHANGEMap") != -1) {
		var coord = getMapCoordinates(vLTTMap);
		if (coord != "") {
			setTimeout("StartShowZoomedMap(" + coord + ");", 10);
		}
	} else {
		StartDoRefreshMap();
	}
}

function EndControlPageRequestHandler(sender, args) {
	SetHiddenElemValue("Loaded");
	//setTimeout('screenBusyOff();ProgressOFF();', 4000);
	setTimeout('ProgressOFF();', 0);
	//	var vLTTMap = GethdnLTTMapValue();
	//	if (vLTTMap.indexOf("LTTMap") != -1) {
	//		var coord = getMapCoordinates(vLTTMap);
	//		if (coord != "") {
	//			//setTimeout("screenBusyOff();ProgressOFF();zoomBox('288375.82999999978', '6110736.4399999995', '4805.1200000006938', '3049.5200000004843')", 4000);
	//			//setTimeout("screenBusyOff();ProgressOFF();zoomBox('" + x + "', '" + y + "', '" + width + "', '" + height + "')", 4000);
	//			setTimeout("screenBusyOff();ProgressOFF();zoomBox(" + coord + ");", 4000);
	//		} else {
	//			alert("Wrong Coord values passed");
	//		}
	//	} else {
	//		setTimeout('screenBusyOff();ProgressOFF();', 4000);
	//	}
}

function execAutoRefresh() {
	ProgressON();
	//setTimeout('screenBusyOff();__doPostBack(\'AnalysisControl1$LinkButton1\',\'\');', 5000);
	__doPostBack('AnalysisControl1$LinkButton1', '');

	//__doPostBack('AnalysisControl1$LinkButton1','');
	//var mapFrame = parent.parent.mapFrame;
	//alert(mapFrame.map);
	//alert(mapFrame.GetSessionId());

	//alert(this.document.body.innerHTML);
	//	alert(parent.parent.document.body.innerHTML);
	//	if (mapFrame.mapInit) {
	//		clearTimeout(atest);
	//		alert('Initialized');
	//	} else {
	//	alert('test');
	//		atest=setTimeout("execAutoRefresh(elem1, elem2)", 500);
	//	}
	//	
	//alert(mapFrame.document.body.innerHTML);
	//	alert(mapFrame.mapInit);
	//if (document.forms.length > 0) {
	//var frm = document.forms[0];
	//var elem = frm.elements['AnalysisControl1_btnRefreshMap'];
	//__doPostBack('AnalysisControl1$btnRefreshMap', '');
	//	if (top.map && top.map.getMap && top.map.getMap() && !top.map.getMap().isBusy()) {

	//setTimeout('screenBusyOn();__doPostBack(\'AnalysisControl1$LinkButton1\',\'\');', 4000);

	//		} else {
	////				execAutoRefresh(elem1, elem2);
	//		}
	//setTimeout('screenBusyOn();__doPostBack(\'AnalysisControl1$btnRefreshMap\',\'\');', 5000);
	//		var found = false;
	//		var cnt = 0;
	//		for (var i = 0; i < frm.elements.length; i++) {
	//			if (typeof (frm.elements[i].id) != "undefined" && frm.elements[i].id.indexOf("btnRefreshMap") != -1) {
	//				alert(frm.elements[i].id);
	//				if (cnt > 20) {
	//					break;
	//				}
	//				cnt += 1;
	//			}
	//		}
	//}
	//var aaa = parent.parent.document.getElementsByName("tbFrame")[0];
	//var aaa111 = aaa.document.frames[0].document.getElementsByName('divRefresh')[0];
	//	var elem2 = parent.parent.document.getElementById(elem1);
	//	var elem3 = parent.parent.document.getElementById(elem2);
	//	alert(elem2);
	//	alert(elem3);
}

function execBAAutoRefresh() {
	ProgressON();
	//setTimeout('screenBusyOff();__doPostBack(\'AnalysisControl1$LinkButton1\',\'\');', 5000);
	__doPostBack('LinkButton1', '');
}

function getMapCoordinates(vLTTMap) {
	var aryLTT = vLTTMap.split(";");
	var x = 0;
	var y = 0;
	var width = 0;
	var height = 0;
	var fnd = false;
	if (aryLTT.length > 1) {
		var aryCoord = aryLTT[1].split(",");
		if (aryCoord.length > 3) {
			fnd = true;
			var x = aryCoord[0];
			var y = aryCoord[1];
			var width = aryCoord[2];
			var height = aryCoord[3];
		}
	}
	if (fnd) {
		return "'" + x + "', '" + y + "', '" + width + "', '" + height + "'";
	}
	return "";
} // end function

function selectObject(key, layer, lat, lon) {
	if (top.map && top.map.getMap && top.map.getMap() && !top.map.getMap().isBusy() && top.map.getMap().getSelection() && top.map.selectObject) {
		top.map.selectObject(key, layer, lat, lon);
	}
	else {
		window.setTimeout('selectObject("' + key + '", "' + layer + '", "' + lat + '", "' + lon + '");', 1000);
	}
}

function selectMultipleObjects(keys, layer, lats, lons) {
	if (top.map && top.map.getMap && top.map.getMap() && !top.map.getMap().isBusy() && top.map.getMap().getSelection() && top.map.selectMultipleObjects) {
		top.map.selectMultipleObjects(keys, layer, lats, lons);
	}
	else {
		window.keys = keys;
		window.lats = lats;
		window.lons = lons;
		window.setTimeout('selectMultipleObjects(window.keys, "' + layer + '", window.lats, window.lons);', 1000);
	}
}

function switchLayers(onLayerName, offLayerName) {
	if (top.map && top.map.getMap && top.map.getMap() && !top.map.getMap().isBusy() && top.map.getMap().getSelection() && top.map.selectObject) {
		top.map.switchLayers(onLayerName, offLayerName);
	}
	else {
		window.setTimeout('switchLayers("' + onLayerName + '", "' + offLayerName + '");', 1000);
	}
}
function getMap() {
	// alert("I am an alert boxy!");
	return document.getElementById('myMap');
	// return parent.GetMapFrame();
	alert(parent.parent.name);
	return parent.parent.name

}


function zoomBox(q_x, q_y, width, height) {
	var map = parent.parent.mapFrame;
	var q_CenterPoint = parent.parent.mapFrame.GetCenter()
	var mapHeight = map.GetMapHeight()
	var mapWidth = map.GetMapWidth()
	var q_CurrentScale = map.GetScale()
	var mapRatio = mapWidth / mapHeight;
	var objRatio = width / height;

	if (mapRatio > objRatio) {
		var q_Aspect = height / mapHeight * q_CurrentScale
	}
	else {
		var q_Aspect = width / mapWidth * q_CurrentScale
	}
	parent.parent.mapFrame.ZoomToView(q_x, q_y, q_Aspect, true);
}

function FindMapName() {
	if (typeof parent.parent.mapFrame.GetMapName == "function") {
		return parent.parent.mapFrame.GetMapName();
	}
	return '';
}

function RefreshMap() {
	//parent.parent.mapFrame.Refresh();
	ChangedRefreshMap();
}

function ChangedRefreshMap() {
	try {
		var elem = document.getElementById("ctrltestid");
		if (elem) {
			elem.value = "Loaded";
			var elem1 = parent.parent.tbFrame;
			if (typeof elem1.DoRefreshMap == "function") {
				elem1.DoRefreshMap();
			} else {
				alert("Procedure DoRefreshMap not found");
			}
		} else {
			alert("Element ctrltestid not found");
		}
	} catch (ex) {
		alert(ex);
	}
}

function screenBusyOn() {
    //var aaa = parent.parent.document.getElementsByName("tbFrame")[0];
    var aaa = parent.parent.tbFrame;
    //var aaa111 = aaa.document.frames[0].document.getElementsByName('divRefresh')[0];
    var aaa111 = aaa.document.getElementsByName('divRefresh')[0];
    aaa111.style.display = "inline";
}
function screenBusyOff() {
    //var aaa = parent.parent.document.getElementsByName("tbFrame")[0];
    var aaa = parent.parent.tbFrame;
    //var aaa111 = aaa.document.frames[0].document.getElementsByName('divRefresh')[0];
    var aaa111 = aaa.document.getElementsByName('divRefresh')[0];
    aaa111.style.display = "none";
}


//function screenBusyOff() {
//	var aaa = parent.parent.document.getElementsByName("tbFrame")[0];
//	var aaa111 = aaa.document.frames[0].document.getElementsByName('divRefresh')[0];
//	aaa111.style.display = "none";

//}


function qqq() {


	var aaa121 = parent.parent.document.getElementById("taskPaneFrame");
	alert(aaa121);
	alert("1");
	//document.getElementById

	var nnn = parent.parent.document.getElementById('AnalysisControl1_btnRefreshMap');

	alert(nnn.length);
	var aaa = parent.parent.document.getElementsByName("maparea")[0];

	//var aaa111 = aaa.document.frames[0].document.getElementsByName('AnalysisControl1_btnRefreshMap');

	var aaa111 = aaa.document.frames[2];


	alert(aaa111);
	var aaa12 = aaa111.document.getElementsByName("taskPaneFrame")[0];
	alert("XXXX");
	alert(aaa12);
	alert(aaa12.name);




	//var aaa1111ss = aaa.document.frames[0].document.getElementsByName('AnalysisControl1_btnRefreshMap')[0];
	var aaa1111ss = aaa12.document.getElementById('AnalysisControl1_btnRefreshMap');
	alert("======");
	alert(aaa1111ss);

	//aaa.disabled = true;
	//aaa111.setAttribute('disabled', 'disabled')
	//alert(aaa111);
	//    
	//    
	//    //var aaa = parent.parent.document.getElementsByName("maparea")[0];
	//    alert(aaa);
	//    alert("2");
	//    var aaa111 = aaa.document.frames[0].document.getElementsByName('Button1')[0];
	//    alert(aaa111);
	//    alert("3");
	//    ///aaa111.style.display = "none";maparea
	//    ///taskPaneFrame

}