function openColourWindow()
{
    url = 'ColourChooser.aspx';
    colourChooser = window.open(url, '_blank', 'height=340,width=420,resizable=no,scrollbars=no,status=no,titlebar=no,toolbar=no');
}

function setColour(index, color)
{
    var hdn = document.getElementById('hdnFillColorIndex');
    hdn.value = index;
    document.forms[0].submit();
}

function getThemeName()
{
    var hdn = document.getElementById('hdnThemeSetName');
    var themeName = window.prompt("Enter New Theme Name", "");
    if (themeName == null)
    {
        return false;
    }
    hdn.value = themeName;
    return true;
}