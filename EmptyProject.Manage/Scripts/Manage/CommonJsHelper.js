function   SelectAll(chAllId,formId)    
{      
     var allValue=document.forms[formId][chAllId].checked;
     for(var i=0 ; i< document.forms[formId].length;i++)   
     {     
         if( document.forms[formId][i].type == "checkbox")     
         {   
            if(document.forms[formId][i].disabled == false)
            {
                document.forms[formId][i].checked = allValue; 
            }
         }   
     }   
     return   false;   
}

//返回树结点
function AddTreeNode(NodeId,NodeName,parentNodeId,bEdit)
{
    window.opener.addTreeNode(NodeId,NodeName,parentNodeId,bEdit);
}

