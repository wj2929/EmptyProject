function OnMouseOverCss(obj, strCss) 
{
    obj.setAttribute("class", strCss);
    obj.className = strCss;
}
function CheckFloat(str)    
{           
    if (/^(\+|-)?\d+($|\.\d+$)/.test( str ))     
    {    
       return true;    
    }     
    else     
    {      
       return false;    
    }    
}    
function CheckInteger(str)    
{           
    if (/^(\+|-)?\d+$/.test( str ))     
    {    
        return true;    
    }     
    else     
    {     
        return false;    
    }    
}
function CheckNumber(str)
{
    if(/^\d+$/.test(str))
    {
        return true;
    }
    else
    {
        return false;
    }
}
function CloseCurrrentWindow()          
{
     window.opener=null;
     window.open('','_self');
     window.close();
}
function   GetAllCheckValues(formId)    
{      
     var ids="";
     for(var i=0 ; i< document.forms[formId].length;i++)   
     {     
        if( document.forms[formId][i].type == "checkbox" && document.forms[formId][i].checked &&  document.forms[formId][i].id.indexOf("ckItem")!=-1)      
        {   
            ids=ids+document.forms[formId][i].value+',';
        }   
     }   
     return   ids;   
}  
function SelectAll(chAllId,formId)
{
    var allValue=document.forms[formId][chAllId].checked;
     for(var i=0 ; i< document.forms[formId].length;i++)   
     {     
         if( document.forms[formId][i].type == "checkbox")     
         {   
            document.forms[formId][i].checked   =   allValue; 
         }   
     }   
     return   false;   
}
function AddTreeNode(NodeId,NodeName,parentNodeId,bEdit)
{
    window.opener.addTreeNode(NodeId,NodeName,parentNodeId,bEdit);
}
function KeyPreeEnter(event)
{
    var evt = arguments[0] || window.event;
    keyCode = evt.keyCode ? evt.keyCode : (evt.which ? evt.which : evt.charCode);
    if (keyCode == 13)
    {
        try
        {
            Search();
        }
        catch (e)
        {
        }
    }
}


