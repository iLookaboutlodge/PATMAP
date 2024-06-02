/****************************************
<confirmDelete>
- Displays a prompt that asks user to confirm deletion of a record.
- Accepts no parameter
****************************************/
function confirmPrompt(message)
{
    return confirm(message);
}

/****************************************
<printPage>
- Prints current page.
- Accepts no parameter
****************************************/
function printPage()
{
    window.print();
    return false;
} 

/****************************************
<showObj>
- Shows or hides a document object.
- Accepts two parameters. Both strings variables.
- objName is the name of the object to show or hide.
- mode can be either "show" or "hide"
****************************************/
function showObj(objName, mode, control)
{	 
    var element;
    var xPos, yPos;
        
	element = document.getElementById(objName);		
	
	if (element)
	{
		if (mode == "show")
		{
		    xPos = getPos(control).xPos + 22;
	        yPos = getPos(control).yPos + 20;   
	 
			element.style.visibility = "visible";
			element.style.left = xPos+"px";
			element.style.top = yPos+"px";
		}
		else
		{			
			element.style.visibility = "hidden";
		}				
	}
}

/****************************************
<hideObj>
- Hides a document object with a delay.
- Accepts one parameter; a string variable.
- objName is the name of the object to hide.
****************************************/
function hideObj(objName) {
	showObj(objName, 'hide', '')
}

/****************************************
<getPos>
- Gets the position of the object.
- Accepts one parameter; a string variable.
- Returns two integers; x and y coordinates.
****************************************/
function getPos(control)
{
    var element;
    var xPos, yPos; 
    
    xPos = 0;
    yPos = 0;
         
	element = control;	
	
	if (element)
	{
	    if (element.offsetParent)
	    {
	        do
	        {
	            xPos += element.offsetLeft;
	            yPos += element.offsetTop;	            
	           
	        } while (element = element.offsetParent)
	    }
	}		
	
	return {xPos:xPos,yPos:yPos}
}

/****************************************
<openWindow>
- Opens a pop window.
- Accepts two parameter; two string variable.
****************************************/
function openWindow(url,settings)
{    
    window.open(url,'newWin',settings);
}

/****************************************
<openWindow>
- Opens a pop window.
- Accepts two parameter; two string variable.
****************************************/
function openReportWindow(url,settings)
{
    var win;    
    win = window.open(url,'newWin',settings);
    win.location.reload(true);
}

/****************************************
<displayImage>
- Display an image.
****************************************/
function displayImage(url)
{
    var element;
    
    element = document.getElementById('image');
    element.src = url;
} 
 